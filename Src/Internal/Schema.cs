using Statics.Mapper.Internal.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using static Statics.Mapper.Internal.Collections;

namespace Statics.Mapper.Internal
{
    internal class Schema
    {
        public List<SourceNode> SourceNodes { get; set; }
        public List<DestinationNode> DestinationNodes { get; set; }
        public SourceNode SourceRootNode { get; set; }
        public DestinationNode DestinationRootNode { get; set; }

        static readonly string Value = "Value";
        static readonly char DOT = '.';

        public Schema(Type source, Type destination, List<IMapperOptionArguments>? mapOptions)
        {
            SourceNodes = GetNodes(source, (node) => node.HasGetMethod)
               .Select(s => new SourceNode(s)).ToList();

            SourceNodes.ForEach(s =>
            {
                s.ParentNode = GetParentNode(s);
                s.ParentNodes = GetParentNodes(s);
                s.Member = s.ParentNode?.Members.FirstOrDefault(w => w.Name == MapperTypeInfo.GetName(s.Name));
            });

            DestinationNodes = GetNodes(destination, (node) =>
                (node.MemberOf.IsGenericType && TypeAdapters.ContainsAdapterGenericTypeDefinition(node.MemberOf.GetGenericTypeDefinition())) ||
                (node.HasSetMethod && (node.HasDefaultConstructor || node.Type.IsValueType))
            )
            .Select(s => new DestinationNode(s)).ToList();

            DestinationNodes.ForEach(d =>
            {
                d.ParentNode = GetParentNode(d);
                d.ParentNodes = GetParentNodes(d);
                d.Member = d.ParentNode?.Members.FirstOrDefault(w => w.Info.Name == MapperTypeInfo.GetName(d.Name))?.Info;
            });

            SourceRootNode = SourceNodes.First(w => w.Depth == 0);
            DestinationRootNode = DestinationNodes.First(w => w.Depth == 0);

            AdaptMapperConfigOptions(
                SourceRootNode,
                DestinationRootNode,
                mapOptions,
                out List<IMapperOptionArguments> options,
                out bool withRootOption);

            if (options.Count == 0)
            {
                Map(SourceRootNode, DestinationRootNode, true, true);
                OnChange();
            }
            else
            {
                if (!withRootOption)
                    Map(SourceRootNode, DestinationRootNode, true, true);

                ApplyOptions(options);
            }
        }

        static void AdaptMapperConfigOptions(
            SourceNode sourceNode,
            DestinationNode destinationNode,
            List<IMapperOptionArguments>? mapOptions,
            out List<IMapperOptionArguments> outMapOptions,
            out bool withRootOption)
        {
            withRootOption = false;

            if (mapOptions == null)
            {
                outMapOptions = [];
                return;
            }

            outMapOptions = new List<IMapperOptionArguments>(mapOptions);

            int optionIndex = outMapOptions.FindLastIndex(o =>
                GetDestinationMemberName(o).Count == 0 &&
                o.Name == nameof(MapperMapOptions<Type, Type>.Ignore));

            if (optionIndex != -1)
            {
                outMapOptions = outMapOptions.GetRange(optionIndex, outMapOptions.Count - optionIndex);
                withRootOption = true;
            }

            optionIndex = outMapOptions.FindLastIndex(o =>
                GetDestinationMemberName(o).Count == 0 &&
                o.Name == nameof(MapperMapOptions<Type, Type>.Map));

            if (optionIndex != -1)
            {
                outMapOptions = outMapOptions.GetRange(optionIndex, outMapOptions.Count - optionIndex);
                withRootOption = true;
            }

            for (int i = 0; i < outMapOptions.Count; i++)
            {
                switch (outMapOptions[i].Name)
                {
                    case nameof(MapperMapOptions<Type, Type>.Ignore):
                        {
                            if (destinationNode.Depth != 0)
                            {
                                IgnoreOption ignoreOption = new(outMapOptions[i]);
                                outMapOptions[i] = new IgnoreOption(
                                    destinationNode.Depth != 0 ?
                                    [string.Concat(destinationNode.Name, DOT, ignoreOption.DestinationMemberNames)] :
                                    ignoreOption.DestinationMemberNames).AsMapOptionArguments();
                            }
                        }
                        break;
                    case nameof(MapperMapOptions<Type, Type>.Map):
                        {
                            if (sourceNode.Depth != 0 || destinationNode.Depth != 0)
                            {
                                MapOption mapOption = new(outMapOptions[i]);
                                outMapOptions[i] = new MapOption(
                                    sourceNode.Depth != 0 ?
                                    string.Concat(sourceNode.Name, DOT, mapOption.SourceMemberName) :
                                    mapOption.SourceMemberName,
                                    destinationNode.Depth != 0 ?
                                    string.Concat(destinationNode.Name, DOT, mapOption.DestinationMemberName) :
                                    mapOption.DestinationMemberName,
                                    mapOption.Expand,
                                    mapOption.UseMapperConfig).AsMapOptionArguments();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

#pragma warning disable CS8600, CS8602, CS8603 // Converting null literal or possible null value to non-nullable type. Dereference of a possibly null reference. Possible null reference return.
        static List<IMapperOptionArguments> GetMapperConfigOptions(SourceNode sourceNode, DestinationNode destinationNode) =>
            (List<IMapperOptionArguments>)
                typeof(MapperConfig<,>)
                    .MakeGenericType([sourceNode.Type, destinationNode.Type])
                    .GetMethod(nameof(MapperConfig<Type, Type>.GetOptions))
                    .Invoke(null, null);
#pragma warning restore CS8600, CS8602, CS8603 // Converting null literal or possible null value to non-nullable type. Dereference of a possibly null reference. Possible null reference return.

        static List<string> GetDestinationMemberName(IMapperOptionArguments option)
        {
            return option.Name switch
            {
                nameof(MapperMapOptions<Type, Type>.Ignore) => new IgnoreOption(option).DestinationMemberNames,
                nameof(MapperMapOptions<Type, Type>.Map) => [new MapOption(option).DestinationMemberName],
                _ => throw new NotImplementedException()
            };
        }

        void ApplyOptions(List<IMapperOptionArguments> options)
        {
            foreach (IMapperOptionArguments option in options)
            {
                switch (option.Name)
                {
                    case nameof(MapperMapOptions<Type, Type>.Ignore):
                        ApplyIgnoreOption(option);
                        break;
                    case nameof(MapperMapOptions<Type, Type>.Map):
                        ApplyMapOption(option);
                        break;
                    default:
                        throw new NotImplementedException($"Option {option.Name} not implemented!");
                }
            }
        }

        void ApplyMapOption(IMapperOptionArguments option)
        {
            MapOption mapOption = new(option);
            ApplyMapOption(mapOption.SourceMemberName, mapOption.DestinationMemberName, mapOption.Expand, mapOption.UseMapperConfig);
        }

#pragma warning disable CS8605 // Unboxing a possibly null value.
        static bool UseMapperMap(SourceNode sourceNode, DestinationNode destinationNode) =>
            (bool)typeof(MapperConfig<,>)
                .MakeGenericType([sourceNode.Type, destinationNode.Type])
                .GetProperties(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .First(p => p.Name == nameof(MapperConfig<Type, Type>.UsePredefinedMap))
                .GetValue(null, null);
#pragma warning restore CS8605 // Unboxing a possibly null value.

        static void SetUseMapper(DestinationNode destinationNode, bool value)
        {
            if (destinationNode.ParentNodes == null)
                return;

            foreach (DestinationNode parentNode in destinationNode.ParentNodes)
                parentNode.UseMapper = false;

            destinationNode.UseMapper = value;
        }

        void MapperMap(SourceNode sourceNode, DestinationNode destinationNode)
        {
            LoadDestinationNodePath(destinationNode);
            destinationNode.SourceNode = sourceNode;
            SetUseMapper(destinationNode, true);
        }

        void Map(SourceNode sourceNode, DestinationNode destinationNode, bool expand, bool useMapperConfig)
        {
            if (!EvaluateMap(sourceNode, destinationNode))
                return;

            if (useMapperConfig && UseMapperMap(sourceNode, destinationNode))
            {
                MapperMap(sourceNode, destinationNode);
                return;
            }

            SetUseMapper(destinationNode, false);

            List<IMapperOptionArguments> options = [];
            bool withRootOption = false;

            var test = GetMapperConfigOptions(sourceNode, destinationNode);

            if (useMapperConfig)
                AdaptMapperConfigOptions(
                    sourceNode,
                    destinationNode,
                    GetMapperConfigOptions(sourceNode, destinationNode),
                    out options,
                    out withRootOption);

            options ??= [];

            foreach (DestinationNodeMember destinationNodeMember in destinationNode.Members)
            {
                if (!destinationNodeMember.Info.HasSetMethod)
                    continue;

                if (useMapperConfig &&
                    withRootOption &&
                    options.Exists(o => GetDestinationMemberName(o).Contains(NodeMemberName(destinationNode.Name, destinationNodeMember.Info.Name))))
                    continue;

                MapperTypeMemberInfo? sourceNodeMember =
                    sourceNode.Members.FirstOrDefault(w => w.Name.Equals(destinationNodeMember.Info.Name, StringComparison.OrdinalIgnoreCase));

                if (sourceNodeMember == null)
                    continue;

                if (sourceNodeMember.IsBuiltIn != destinationNodeMember.Info.IsBuiltIn)
                    continue;

                if (sourceNodeMember.Type == destinationNodeMember.Info.Type &&
                    (sourceNodeMember.Type.IsInterface || sourceNodeMember.Type.IsAbstract) &&
                    (destinationNodeMember.Info.Type.IsInterface || destinationNodeMember.Info.Type.IsAbstract))
                {
                    TryMapMember(sourceNode, sourceNodeMember, destinationNode, destinationNodeMember);
                }
                else if (IsSourceNode(NodeMemberName(sourceNode.Name, sourceNodeMember.Name)) &&
                    IsDestinationNode(NodeMemberName(destinationNode.Name, destinationNodeMember.Info.Name)))
                {
                    SourceNode memberSourceNode = SourceNodes.First(node => node.Name == NodeMemberName(sourceNode.Name, sourceNodeMember.Name));
                    DestinationNode memberDestinationNode = DestinationNodes.First(node => node.Name == NodeMemberName(destinationNode.Name, destinationNodeMember.Info.Name));

                    if (expand)
                        Map(memberSourceNode, memberDestinationNode, expand, useMapperConfig);
                }
                else
                {
                    TryMapMember(sourceNode, sourceNodeMember, destinationNode, destinationNodeMember);
                }
            }

            if (useMapperConfig)
                ApplyOptions(options);
        }

        bool EvaluateMap(SourceNode? sourceNode, DestinationNode? destinationNode)
        {
            if (sourceNode == null || destinationNode == null)
                return false;

            if (sourceNode.IsStatic &&
                destinationNode.IsStatic &&
                sourceNode.Type == destinationNode.Type)
                return false;

            List<SourceNode> sourceNodes = GetParentNodes(sourceNode);
            for (int sn = sourceNodes.Count; sn-- > 0;)
            {
                if (!sourceNodes[sn].IsStatic)
                    continue;

                List<DestinationNode> destinationNodes = GetParentNodes(destinationNode);
                for (int dn = destinationNodes.Count; dn-- > 0;)
                {
                    if (!destinationNodes[dn].IsStatic)
                        continue;

                    if (sourceNodes[sn].Type == destinationNodes[dn].Type)
                        return false;
                }
            }

            return true;
        }

        public void ApplyMapOption(string sourceMember, string destinationMember, bool expand, bool useMapperConfig)
        {
            sourceMember = ResolveSourceMemberName(sourceMember);
            destinationMember = ResolveDestinationMemberName(destinationMember);

            bool sourceMemberIsNode = IsSourceNode(sourceMember);
            bool destinationMemberIsNode = IsDestinationNode(destinationMember);

            SourceNode? sourceNode;
            MapperTypeMemberInfo? sourceNodeMember = null;
            DestinationNode? destinationNode;
            DestinationNodeMember? destinationNodeMember = null;

            InvalidOperationException mapeException = new($"Cannot map from {sourceMember} to {destinationMember}.");

            if (sourceMemberIsNode != destinationMemberIsNode)
                throw new InvalidOperationException();

            if (sourceMemberIsNode)
            {
                sourceNode = SourceNodes.First(w => w.Name == sourceMember);
                destinationNode = DestinationNodes.First(w => w.Name == destinationMember);

                if (!EvaluateMap(sourceNode, destinationNode))
                    throw mapeException;

                Map(sourceNode, destinationNode, expand, useMapperConfig);
                OnChange();

                return;
            }

            sourceNode = SourceNodes.FirstOrDefault(w => w.Name == MapperTypeInfo.GetNodeName(sourceMember));
            sourceNodeMember = sourceNode?.Members.FirstOrDefault(w => w.Name == MapperTypeInfo.GetName(sourceMember));

            destinationNode = DestinationNodes.FirstOrDefault(w => w.Name == MapperTypeInfo.GetNodeName(destinationMember));
            destinationNodeMember = destinationNode?.Members.FirstOrDefault(w => w.Info.Name == MapperTypeInfo.GetName(destinationMember));

            if (sourceNode == null ||
                sourceNodeMember == null ||
                destinationNode == null ||
                destinationNodeMember == null)
                throw mapeException;

            if (!EvaluateMap(sourceNode, destinationNode))
                throw mapeException;

            if (!TryMapMember(
                sourceNode,
                sourceNodeMember,
                destinationNode,
                destinationNodeMember))
                throw new InvalidOperationException($"Cannot map from {sourceMember} to {destinationMember}.");

            SetUseMapper(destinationNode, false);
            OnChange();
        }

        void Ignore(DestinationNode destinationNode)
        {
            destinationNode.Load = false;

            foreach (DestinationNodeMember destinationNodeMember in destinationNode.Members)
                destinationNodeMember.Map = false;

            IEnumerable<DestinationNode> childNodes = GetChildNodes(destinationNode, childNode => childNode.Load);

            if (!childNodes.Any())
                SetUseMapper(destinationNode, false);

            foreach (DestinationNode childNode in childNodes)
                Ignore(childNode);
        }

        void ApplyIgnoreOption(List<string> destinationMembers)
        {
            for (int d = 0; d < destinationMembers.Count; d++)
            {
                string destinationMember = ResolveDestinationMemberName(destinationMembers[d]);

                DestinationNode? node = DestinationNodes.FirstOrDefault(w => w.Name == destinationMember);
                if (node != null)
                {
                    Ignore(node);
                    continue;
                }

                node = DestinationNodes.FirstOrDefault(w => w.Name == MapperTypeInfo.GetNodeName(destinationMember));
                if (node == null) continue;

                DestinationNodeMember? member = node.Members.FirstOrDefault(w => w.Info.Name == MapperTypeInfo.GetName(destinationMember));
                if (member == null) continue;

                member.Map = false;

                SetUseMapper(node, false);
            }

            OnChange();
        }

        void ApplyIgnoreOption(IMapperOptionArguments option)
        {
            IgnoreOption ignoreOption = new(option);
            ApplyIgnoreOption(ignoreOption.DestinationMemberNames);
        }

        public void ForEachDestinationNode(Func<DestinationNode, bool> predicate, Action<DestinationNode> action)
        {
            foreach (DestinationNode destinationNode in DestinationNodes)
                if (predicate(destinationNode))
                    action(destinationNode);
        }

        Dictionary<string, List<SourceNode>>? _sourceChildNodes = null;

        Dictionary<string, List<SourceNode>> GetSourceChildNodes()
        {
            if (_sourceChildNodes != null)
                return _sourceChildNodes;

            _sourceChildNodes = [];

            List<SourceNode> childNodes = SourceNodes.Where(n => n.ParentNode?.Name == SourceRootNode.Name).ToList();

            _sourceChildNodes.Add(SourceRootNode.Name, childNodes);

            if (childNodes.Count == 0)
                return _sourceChildNodes;

            Queue<SourceNode> queue = new(childNodes);

            while (queue.Count != 0)
            {
                SourceNode node = queue.Dequeue();
                childNodes = SourceNodes.Where(n => n.ParentNode?.Name == node.Name).ToList();
                _sourceChildNodes.Add(node.Name, childNodes);

                if (childNodes.Count != 0)
                {
                    SourceNode? parentNode = node.ParentNode;
                    while (parentNode != null)
                    {
                        _sourceChildNodes[parentNode.Name].AddRange(childNodes);
                        parentNode = parentNode.ParentNode;
                    }

                    childNodes.ForEach(queue.Enqueue);
                }
            }

            return _sourceChildNodes;
        }

        static bool CanMapCollection(
            Type sourceType,
            Type destinationType)
        {
            if (IsCollection(sourceType) !=
                IsCollection(destinationType))
                return false;

            if (sourceType.IsArray && sourceType.GetArrayRank() != 1)
                return false;

            if (destinationType.IsArray && destinationType.GetArrayRank() != 1)
                return false;

            if (!CanMaintainCollection(sourceType, destinationType))
                return false;

            return CanMapTypes(
                GetIEnumerableArgument(sourceType),
                GetIEnumerableArgument(destinationType));
        }

        public static bool CanMapTypes(
           Type sourceType,
           Type destinationType)
        {
            if (IsCollection(sourceType) ||
                IsCollection(destinationType))
                return CanMapCollection(sourceType, destinationType);

            if (destinationType.IsInterface || destinationType.IsAbstract)
                return sourceType == destinationType;

            if (MapperTypeInfo.IsBuiltIn(destinationType))
                return IL.CanEmitConvert(sourceType, destinationType);

            return true;
        }

        public static bool CanLoadAndSetCollection(
            MapperTypeMemberInfo sourceMember,
            MapperTypeMemberInfo destinationMember)
        {
            if (!(sourceMember.HasGetMethod &&
                destinationMember.HasSetMethod))
                return false;

            return CanMapCollection(sourceMember.Type, destinationMember.Type);
        }

        bool TryMapMember(
            SourceNode sourceNode,
            MapperTypeMemberInfo sourceNodeMember,
            DestinationNode destinationNode,
            DestinationNodeMember destinationNodeMember)
        {
            destinationNodeMember.SourceNode = sourceNode;
            destinationNodeMember.SourceNodeMember = sourceNodeMember;
            destinationNodeMember.Status = Status.Failed;
            destinationNodeMember.IsCollection = IsCollection(destinationNodeMember.Info.Type);

            destinationNodeMember.Map = !destinationNodeMember.IsCollection ?
                IL.CanEmitLoadAndSetValue(sourceNodeMember, destinationNodeMember.Info) :
                CanLoadAndSetCollection(sourceNodeMember, destinationNodeMember.Info);

            if (destinationNodeMember.Map)
            {
                destinationNodeMember.Status = Status.Successful;
                LoadDestinationNodePath(destinationNode);
            }

            return destinationNodeMember.Map;
        }

        public static string NodeMemberName(string nodeName, string memberName) =>
            string.IsNullOrEmpty(nodeName) ? memberName : nodeName + DOT + memberName;

        void OnChange()
        {
            foreach (SourceNode sourceNode in SourceNodes)
                sourceNode.Load = false;

            foreach (DestinationNode destinationNode in DestinationNodes)
            {
                destinationNode.MembersMapCount = 0;

                if (!destinationNode.Load)
                    continue;

                destinationNode.MembersMapCount =
                   destinationNode.Members.Count(n => n.Map && n.Status == Status.Successful);

                if (destinationNode.UseMapper)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    LoadSourceNodePath(destinationNode.SourceNode);
#pragma warning restore CS8604 // Possible null reference argument.
                }
                else
                {
                    foreach (DestinationNodeMember destinationNodeMember in destinationNode.Members)
                    {
                        if (destinationNodeMember.Status != Status.Successful)
                            continue;

                        LoadSourceNodePath(destinationNodeMember.SourceNode);
                    }
                }
            }
        }

        string ResolveSourceMemberName(string memberName)
        {
            if (!memberName.Contains(Value))
                return memberName;

            List<string> segments = [.. memberName.Split(DOT)];

            for (int i = 0; i < segments.Count; i++)
            {
                if (segments[i] == Value)
                {
                    SourceNode? sourceNode = SourceNodes
                         .FirstOrDefault(n => n.Name == MapperTypeInfo.GetNodeName(string.Join(DOT, [.. segments], 0, i + 1)));
                    if (sourceNode != null && sourceNode.NullableUnderlyingType != null)
                    {
                        segments.RemoveAt(i);
                        i--;
                    }
                }
            }

            return string.Join(DOT, segments);
        }

        void LoadSourceNodePath(SourceNode sourceNode)
        {
            sourceNode.Load = true;

            string[] segments = [.. sourceNode.Name.Split(DOT, StringSplitOptions.RemoveEmptyEntries)];

            for (int i = 0; i < segments.Length; i++)
            {
                string nodeName = string.Join(DOT, segments, 0, i + 1);

                foreach (SourceNode node in SourceNodes)
                {
                    if (node.Load)
                        continue;

                    if (node.Name == nodeName || node.Name == string.Empty)
                        node.Load = true;
                }
            }
        }

        void LoadDestinationNodePath(DestinationNode destinationNode)
        {
            destinationNode.Load = true;

            string[] segments = [.. destinationNode.Name.Split(DOT, StringSplitOptions.RemoveEmptyEntries)];

            for (int i = 0; i < segments.Length; i++)
            {
                string nodeName = string.Join(DOT, segments, 0, i + 1);

                foreach (DestinationNode node in DestinationNodes)
                {
                    if (node.Load)
                        continue;

                    if (node.Name == nodeName || node.Name == string.Empty)
                        node.Load = true;
                }
            }
        }

        string ResolveDestinationMemberName(string memberName)
        {
            if (!memberName.Contains(Value))
                return memberName;

            List<string> segments = [.. memberName.Split(DOT)];

            for (int i = 0; i < segments.Count; i++)
            {
                if (segments[i] == Value)
                {
                    DestinationNode? destinationNode = DestinationNodes
                        .FirstOrDefault(n => n.Name == MapperTypeInfo.GetNodeName(string.Join(DOT, [.. segments], 0, i + 1)));
                    if (destinationNode != null && destinationNode.NullableUnderlyingType != null)
                    {
                        segments.RemoveAt(i);
                        i--;
                    }
                }
            }

            return string.Join(DOT, segments);
        }

        bool IsSourceNode(string memberName) =>
            memberName == string.Empty || SourceNodes.Any(n => n.Name == memberName);

        bool IsDestinationNode(string memberName) =>
            memberName == string.Empty || DestinationNodes.Any(n => n.Name == memberName);

        public static IEnumerable<DestinationNodeMember> GetDestinationNodeMembers(DestinationNode destinationNode, Func<DestinationNodeMember, bool> predicate) =>
            destinationNode.Members.Where(predicate);

        public static MapperTypeMemberInfo GetMember(string nodeName, IEnumerable<NodeInfo> nodes) =>
            nodes.First(w => w.Name == MapperTypeInfo.GetNodeName(nodeName))
                .Members.First(w => w.Name == MapperTypeInfo.GetName(nodeName));

        public static MapperTypeMemberInfo GetMember(string nodeName, IEnumerable<SourceNode> nodes) =>
            nodes.First(w => w.Name == MapperTypeInfo.GetNodeName(nodeName))
                .Members.First(w => w.Name == MapperTypeInfo.GetName(nodeName));

        public static MapperTypeMemberInfo GetMember(string nodeName, IEnumerable<DestinationNode> nodes) =>
            nodes.First(w => w.Name == MapperTypeInfo.GetNodeName(nodeName))
                .Members.First(w => w.Info.Name == MapperTypeInfo.GetName(nodeName)).Info;

        public SourceNode? GetParentNode(SourceNode node) =>
            node.Name != string.Empty ? SourceNodes.FirstOrDefault(w => w.Name == MapperTypeInfo.GetNodeName(node.Name)) : null;

        public List<SourceNode> GetParentNodes(SourceNode node)
        {
            List<SourceNode> parentNodes = [];

            if (node.Name == string.Empty)
                return parentNodes;

            SourceNode? parentNode = GetParentNode(node);

            while (parentNode != null)
            {
                parentNodes.Add(parentNode);
                parentNode = GetParentNode(parentNode);
            }

            return parentNodes;
        }

        public DestinationNode? GetParentNode(DestinationNode node) =>
            node.Name != string.Empty ? DestinationNodes.FirstOrDefault(w => w.Name == MapperTypeInfo.GetNodeName(node.Name)) : null;

        public List<DestinationNode> GetParentNodes(DestinationNode node)
        {
            List<DestinationNode> parentNodes = [];

            if (node.Name == string.Empty)
                return parentNodes;

            DestinationNode? parentNode = GetParentNode(node);

            while (parentNode != null)
            {
                parentNodes.Add(parentNode);
                parentNode = GetParentNode(parentNode);
            }

            return parentNodes;
        }

        public List<SourceNode> GetChildNodes(SourceNode sourceNode, Func<SourceNode, bool>? predicate = null, int depth = 1)
        {
            bool filter(SourceNode n) =>
                (depth == 0 || n.Depth <= sourceNode.Depth + depth) && (predicate == null || predicate(n));

            return GetSourceChildNodes().TryGetValue(sourceNode.Name, out List<SourceNode>? sourceChildNodes)
                ? sourceChildNodes.Where(filter).ToList()
                : [];
        }

        public List<DestinationNode> GetChildNodes(DestinationNode node, Func<DestinationNode, bool> predicate) =>
            DestinationNodes
                .Where(destinationNode =>
                    destinationNode.ParentNode?.Name == node.Name &&
                    predicate(destinationNode))
                .ToList();

        public static List<NodeInfo> GetNodes(Type type, Func<MapperTypeMemberInfo, bool> predicate)
        {
            List<NodeInfo> nodes = [];
            List<NodeInfo> typeNodes = MapperTypeInfo.GetNodes(type, true);
            typeNodes.Sort((l, r) => l.Depth.CompareTo(r.Depth));

            for (int n = 0; n < typeNodes.Count; n++)
            {
                if (typeNodes[n].Depth == 0 ||
                    (
                       predicate(GetMember(typeNodes[n].Name, typeNodes)) &&
                       nodes.Exists(w => w.Name == MapperTypeInfo.GetNodeName(typeNodes[n].Name)
                    )))
                    nodes.Add(typeNodes[n]);
            }

            return nodes;
        }
    }
}
