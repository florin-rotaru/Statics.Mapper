using Air.Reflection;
using Air.Reflection.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using static Air.Mapper.Internal.Collections;

namespace Air.Mapper.Internal
{
    internal class Schema
    {
        public IEnumerable<SourceNode> SourceNodes { get; private set; }
        public IEnumerable<DestinationNode> DestinationNodes { get; private set; }
        public SourceNode SourceRootNode { get; private set; }
        public DestinationNode DestinationRootNode { get; private set; }

        private static readonly string Value = "Value";
        private static readonly char DOT = '.';

        public Schema(Type source, Type destination, IEnumerable<IMapOption> mapOptions)
        {
            SourceNodes = GetNodes(source, (node) => node.HasGetMethod)
               .Select(s => new SourceNode(s)).ToList();

            SourceNodes.ToList().ForEach(s =>
            {
                s.ParentNode = GetParentNode(s);
                s.ParentNodes = GetParentNodes(s).ToList();
                s.MemberInfo = s.ParentNode?.Members.FirstOrDefault(w => w.Name == TypeInfo.GetName(s.Name));
            });

            DestinationNodes = GetNodes(destination, (node) =>
                (node.MemberOf.IsGenericType && TypeAdapters.ContainsAdapterGenericTypeDefinition(node.MemberOf.GetGenericTypeDefinition())) ||
                (node.HasSetMethod && (node.HasDefaultConstructor || node.Type.IsValueType))
            )
            .Select(s => new DestinationNode(s)).ToList();

            DestinationNodes.ToList().ForEach(d =>
            {
                d.ParentNode = GetParentNode(d);
                d.ParentNodes = GetParentNodes(d).ToList();
                d.MemberInfo = d.ParentNode?.Members.FirstOrDefault(w => w.Info.Name == TypeInfo.GetName(d.Name))?.Info;
            });

            SourceRootNode = SourceNodes.First(w => w.Depth == 0);
            DestinationRootNode = DestinationNodes.First(w => w.Depth == 0);

            AdaptMapperConfigOptions(
                SourceRootNode,
                DestinationRootNode,
                mapOptions,
                out List<IMapOption> options,
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

        private void AdaptMapperConfigOptions(
            SourceNode sourceNode,
            DestinationNode destinationNode,
            IEnumerable<IMapOption> mapperConfigOptions,
            out List<IMapOption> outOptions,
            out bool withRootOption)
        {
            withRootOption = false;

            if (mapperConfigOptions == null)
            {
                outOptions = new List<IMapOption>();
                return;
            }

            outOptions = mapperConfigOptions.ToList();

            int optionIndex = outOptions.FindLastIndex(o =>
                GetDestinationMemberName(o) == string.Empty &&
                o.Name == nameof(MapOptions<Type, Type>.Ignore));
            if (optionIndex != -1)
            {
                outOptions = outOptions.GetRange(optionIndex, outOptions.Count - optionIndex);
                withRootOption = true;
            }

            optionIndex = outOptions.FindLastIndex(o =>
                GetDestinationMemberName(o) == string.Empty &&
                o.Name == nameof(MapOptions<Type, Type>.Map));
            if (optionIndex != -1)
            {
                outOptions = outOptions.GetRange(optionIndex, outOptions.Count - optionIndex);
                withRootOption = true;
            }

            for (int i = 0; i < outOptions.Count; i++)
            {
                switch (outOptions[i].Name)
                {
                    case nameof(MapOptions<Type, Type>.Ignore):
                        {
                            if (destinationNode.Depth != 0)
                            {
                                IgnoreOption ignoreOption = new IgnoreOption(outOptions[i]);
                                outOptions[i] = new IgnoreOption(
                                    destinationNode.Depth != 0 ?
                                    string.Concat(destinationNode.Name, DOT, ignoreOption.DestinationMemberName) :
                                    ignoreOption.DestinationMemberName).AsMapOption();
                            }
                        }
                        break;
                    case nameof(MapOptions<Type, Type>.Map):
                        {
                            if (sourceNode.Depth != 0 || destinationNode.Depth != 0)
                            {
                                MapOption mapOption = new MapOption(outOptions[i]);
                                outOptions[i] = new MapOption(
                                    sourceNode.Depth != 0 ?
                                    string.Concat(sourceNode.Name, DOT, mapOption.SourceMemberName) :
                                    mapOption.SourceMemberName,

                                    destinationNode.Depth != 0 ?
                                    string.Concat(destinationNode.Name, DOT, mapOption.DestinationMemberName) :
                                    mapOption.DestinationMemberName,

                                    mapOption.Expand,
                                    mapOption.UseMapperConfig).AsMapOption();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private IEnumerable<IMapOption> GetMapperConfigOptions(SourceNode sourceNode, DestinationNode destinationNode) =>
            (IEnumerable<IMapOption>)
                typeof(MapperConfig<,>)
                    .MakeGenericType(new Type[] { sourceNode.Type, destinationNode.Type })
                    .GetMethod(nameof(MapperConfig<Type, Type>.GetOptions))
                    .Invoke(null, null);

        private string GetDestinationMemberName(IMapOption option)
        {
            return option.Name switch
            {
                nameof(MapOptions<Type, Type>.Ignore) => new IgnoreOption(option).DestinationMemberName,
                nameof(MapOptions<Type, Type>.Map) => new MapOption(option).DestinationMemberName,
                _ => null,
            };
        }

        private void ApplyOptions(IEnumerable<IMapOption> options)
        {
            foreach (IMapOption option in options)
            {
                switch (option.Name)
                {
                    case nameof(MapOptions<Type, Type>.Ignore):
                        ApplyIgnoreOption(option);
                        break;
                    case nameof(MapOptions<Type, Type>.Map):
                        ApplyMapOption(option);
                        break;
                    default:
                        throw new NotImplementedException($"Option {option.Name} not implemented!");
                }
            }
        }

        private void ApplyIgnoreOption(IMapOption option)
        {
            IgnoreOption ignoreOption = new IgnoreOption(option);
            ApplyIgnoreOption(new[] { ignoreOption.DestinationMemberName });
        }

        private void ApplyMapOption(IMapOption option)
        {
            MapOption mapOption = new MapOption(option);
            ApplyMapOption(mapOption.SourceMemberName, mapOption.DestinationMemberName, mapOption.Expand, mapOption.UseMapperConfig);
        }

        private void Map(SourceNode sourceNode, DestinationNode destinationNode, bool expand, bool useMapperConfig)
        {
            if (!EvaluateMap(sourceNode, destinationNode))
                return;

            List<IMapOption> options = null;
            bool withRootOption = false;

            if (useMapperConfig)
                AdaptMapperConfigOptions(
                    sourceNode,
                    destinationNode,
                    GetMapperConfigOptions(sourceNode, destinationNode),
                    out options,
                    out withRootOption);

            foreach (DestinationNodeMember destinationNodeMember in destinationNode.Members)
            {
                if (!destinationNodeMember.Info.HasSetMethod)
                    continue;

                if (useMapperConfig &&
                    withRootOption &&
                    options.Any(o => GetDestinationMemberName(o) == NodeMemberName(destinationNode.Name, destinationNodeMember.Info.Name)))
                    continue;

                MemberInfo sourceNodeMember =
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

        private bool EvaluateMap(SourceNode sourceNode, DestinationNode destinationNode)
        {
            if (sourceNode.IsStatic && 
                destinationNode.IsStatic &&
                sourceNode.Type == destinationNode.Type)
                return false;

            List<SourceNode> sourceNodes = GetParentNodes(sourceNode).ToList();
            for (int sn = sourceNodes.Count; sn-- > 0;)
            {
                if (!sourceNodes[sn].IsStatic)
                    continue;

                List<DestinationNode> destinationNodes = GetParentNodes(destinationNode).ToList();
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

            SourceNode sourceNode = null;
            MemberInfo sourceNodeMember = null;
            DestinationNode destinationNode = null;
            DestinationNodeMember destinationNodeMember = null;

            if (sourceMemberIsNode != destinationMemberIsNode)
                throw new InvalidOperationException();

            if (sourceMemberIsNode && destinationMemberIsNode)
            {
                sourceNode = SourceNodes.First(w => w.Name == sourceMember);
                destinationNode = DestinationNodes.First(w => w.Name == destinationMember);

                if (!EvaluateMap(sourceNode, destinationNode))
                    throw new InvalidOperationException(
                        $"Cannot map from {sourceNode.Name} static [{sourceNode.Type}] to {destinationNode.Name} static [{destinationNode.Type}].");

                Map(sourceNode, destinationNode, expand, useMapperConfig);
                OnChange();

                return;
            }

            sourceNode = SourceNodes.FirstOrDefault(w => w.Name == TypeInfo.GetNodeName(sourceMember));
            sourceNodeMember = sourceNode.Members.FirstOrDefault(w => w.Name == TypeInfo.GetName(sourceMember));

            destinationNode = DestinationNodes.FirstOrDefault(w => w.Name == TypeInfo.GetNodeName(destinationMember));
            destinationNodeMember = destinationNode.Members.FirstOrDefault(w => w.Info.Name == TypeInfo.GetName(destinationMember));

            if (sourceNode == null ||
                sourceNodeMember == null ||
                destinationNode == null ||
                destinationNodeMember == null)
                throw new InvalidOperationException($"Cannot map from {sourceMember} to {destinationMember}.");

            if (!EvaluateMap(sourceNode, destinationNode))
                throw new InvalidOperationException(
                    $"Cannot map from {sourceNode.Name} static [{sourceNode.Type}] to {destinationNode.Name} static [{destinationNode.Type}].");

            if (!TryMapMember(
                sourceNode,
                sourceNodeMember,
                destinationNode,
                destinationNodeMember))
                throw new InvalidOperationException($"Cannot map from {sourceMember} to {destinationMember}.");

            OnChange();
        }

        private void Ignore(DestinationNode destinationNode)
        {
            destinationNode.Load = false;

            foreach (DestinationNodeMember destinationNodeMember in destinationNode.Members)
                destinationNodeMember.Map = false;

            IEnumerable<DestinationNode> childNodes = GetChildNodes(destinationNode, childNode => childNode.Load);
            foreach (DestinationNode childNode in childNodes)
                Ignore(childNode);
        }

        private void ApplyIgnoreOption(string[] destinationMembers)
        {
            for (int d = 0; d < destinationMembers.Length; d++)
            {
                string destinationMember = ResolveDestinationMemberName(destinationMembers[d]);

                DestinationNode node = DestinationNodes.FirstOrDefault(w => w.Name == destinationMember);
                if (node != null)
                {
                    Ignore(node);
                    continue;
                }

                node = DestinationNodes.FirstOrDefault(w => w.Name == TypeInfo.GetNodeName(destinationMember));
                if (node == null) continue;

                DestinationNodeMember member = node.Members.FirstOrDefault(w => w.Info.Name == TypeInfo.GetName(destinationMember));
                if (member == null) continue;

                member.Map = false;
            }

            OnChange();
        }

        public void ForEachDestinationNode(Func<DestinationNode, bool> predicate, Action<DestinationNode> action)
        {
            foreach (DestinationNode destinationNode in DestinationNodes)
                if (predicate(destinationNode))
                    action(destinationNode);
        }

        private Dictionary<string, List<SourceNode>> SourceChildNodes = null;
        private void SetSourceChildNodes()
        {
            SourceChildNodes = new Dictionary<string, List<SourceNode>>();

            List<SourceNode> childNodes = SourceNodes.Where(n => n.ParentNode?.Name == SourceRootNode.Name).ToList();

            SourceChildNodes.Add(SourceRootNode.Name, childNodes);

            if (childNodes.Count == 0)
                return;

            Queue<SourceNode> queue = new Queue<SourceNode>(childNodes);

            while (queue.Count != 0)
            {
                SourceNode node = queue.Dequeue();
                childNodes = SourceNodes.Where(n => n.ParentNode?.Name == node.Name).ToList();
                SourceChildNodes.Add(node.Name, childNodes);

                if (childNodes.Count != 0)
                {
                    SourceNode parentNode = node.ParentNode;
                    while (parentNode != null)
                    {
                        SourceChildNodes[parentNode.Name].AddRange(childNodes);
                        parentNode = parentNode.ParentNode;
                    }

                    childNodes.ForEach(n => queue.Enqueue(n));
                }
            }
        }

        private static bool CanMapCollection(
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

            if (TypeInfo.IsBuiltIn(destinationType))
                return ILGenerator.CanEmitConvert(sourceType, destinationType);

            return true;
        }

        public static bool CanLoadAndSetCollection(
            MemberInfo sourceMember,
            MemberInfo destinationMember)
        {
            if (!(sourceMember.HasGetMethod &&
                destinationMember.HasSetMethod))
                return false;

            return CanMapCollection(sourceMember.Type, destinationMember.Type);
        }

        private bool TryMapMember(
            SourceNode sourceNode,
            MemberInfo sourceNodeMember,
            DestinationNode destinationNode,
            DestinationNodeMember destinationNodeMember)
        {
            destinationNodeMember.SourceNode = sourceNode;
            destinationNodeMember.SourceNodeMember = sourceNodeMember;
            destinationNodeMember.Status = Status.Failed;
            destinationNodeMember.IsCollection = IsCollection(destinationNodeMember.Info.Type);

            destinationNodeMember.Map = !destinationNodeMember.IsCollection ?
                ILGenerator.CanEmitLoadAndSetValue(sourceNodeMember, destinationNodeMember.Info) :
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

        private void OnChange()
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

                foreach (DestinationNodeMember destinationNodeMember in destinationNode.Members)
                {
                    if (destinationNodeMember.Status != Status.Successful)
                        continue;

                    LoadSourceNodePath(destinationNodeMember.SourceNode);
                }
            }
        }

        private string ResolveSourceMemberName(string memberName)
        {
            if (!memberName.Contains(Value))
                return memberName;

            List<string> segments = memberName.Split(DOT).ToList();

            for (int i = 0; i < segments.Count; i++)
            {
                if (segments[i] == Value)
                {
                    SourceNode sourceNode = SourceNodes
                         .FirstOrDefault(n => n.Name == TypeInfo.GetNodeName(string.Join(DOT, segments.ToArray(), 0, i + 1)));
                    if (sourceNode != null && sourceNode.NullableUnderlyingType != null)
                    {
                        segments.RemoveAt(i);
                        i--;
                    }
                }
            }

            return string.Join(DOT, segments);
        }

        private void LoadSourceNodePath(SourceNode sourceNode)
        {
            sourceNode.Load = true;

            string[] segments = sourceNode.Name.Split(DOT, StringSplitOptions.RemoveEmptyEntries).ToArray();

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

        private void LoadDestinationNodePath(DestinationNode destinationNode)
        {
            destinationNode.Load = true;

            string[] segments = destinationNode.Name.Split(DOT, StringSplitOptions.RemoveEmptyEntries).ToArray();

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

        private string ResolveDestinationMemberName(string memberName)
        {
            if (!memberName.Contains(Value))
                return memberName;

            List<string> segments = memberName.Split(DOT).ToList();

            for (int i = 0; i < segments.Count; i++)
            {
                if (segments[i] == Value)
                {
                    DestinationNode destinationNode = DestinationNodes
                        .FirstOrDefault(n => n.Name == TypeInfo.GetNodeName(string.Join(DOT, segments.ToArray(), 0, i + 1)));
                    if (destinationNode != null && destinationNode.NullableUnderlyingType != null)
                    {
                        segments.RemoveAt(i);
                        i--;
                    }
                }
            }

            return string.Join(DOT, segments);
        }

        private bool IsSourceNode(string memberName) =>
            memberName == string.Empty || SourceNodes.Any(n => n.Name == memberName);

        private bool IsDestinationNode(string memberName) =>
            memberName == string.Empty || DestinationNodes.Any(n => n.Name == memberName);

        public IEnumerable<DestinationNodeMember> GetDestinationNodeMembers(DestinationNode destinationNode, Func<DestinationNodeMember, bool> predicate) =>
            destinationNode.Members.Where(predicate);

        public static MemberInfo GetMember(string nodeName, IEnumerable<TypeNode> nodes) =>
            nodes.First(w => w.Name == TypeInfo.GetNodeName(nodeName))
                .Members.First(w => w.Name == TypeInfo.GetName(nodeName));

        public static MemberInfo GetMember(string nodeName, IEnumerable<SourceNode> nodes) =>
            nodes.First(w => w.Name == TypeInfo.GetNodeName(nodeName))
                .Members.First(w => w.Name == TypeInfo.GetName(nodeName));

        public static MemberInfo GetMember(string nodeName, IEnumerable<DestinationNode> nodes) =>
            nodes.First(w => w.Name == TypeInfo.GetNodeName(nodeName))
                .Members.First(w => w.Info.Name == TypeInfo.GetName(nodeName)).Info;

        public SourceNode GetParentNode(SourceNode node) =>
            node.Name != string.Empty ? SourceNodes.FirstOrDefault(w => w.Name == TypeInfo.GetNodeName(node.Name)) : null;

        public IEnumerable<SourceNode> GetParentNodes(SourceNode node)
        {
            List<SourceNode> returnValue = new List<SourceNode>();

            if (node.Name == string.Empty)
                return returnValue;

            SourceNode parentNode = GetParentNode(node);

            while (parentNode != null)
            {
                returnValue.Add(parentNode);
                parentNode = GetParentNode(parentNode);
            }

            return returnValue;
        }

        public DestinationNode GetParentNode(DestinationNode node) =>
            node.Name != string.Empty ? DestinationNodes.FirstOrDefault(w => w.Name == TypeInfo.GetNodeName(node.Name)) : null;

        public IEnumerable<DestinationNode> GetParentNodes(DestinationNode node)
        {
            List<DestinationNode> returnValue = new List<DestinationNode>();

            if (node.Name == string.Empty)
                return returnValue;

            DestinationNode parentNode = GetParentNode(node);

            while (parentNode != null)
            {
                returnValue.Add(parentNode);
                parentNode = GetParentNode(parentNode);
            }

            return returnValue;
        }

        public IEnumerable<SourceNode> GetChildNodes(SourceNode sourceNode, Func<SourceNode, bool> predicate = null, int depth = 1)
        {
            List<SourceNode> returnValue;
            bool filter(SourceNode n) =>
                (depth == 0 || n.Depth <= sourceNode.Depth + depth) && (predicate == null || predicate(n));

            if (SourceChildNodes != null)
            {
                SourceChildNodes.TryGetValue(sourceNode.Name, out returnValue);
                return returnValue.Where(filter).ToList();
            }

            SetSourceChildNodes();
            SourceChildNodes.TryGetValue(sourceNode.Name, out returnValue);

            return returnValue.Where(predicate);
        }

        public IEnumerable<DestinationNode> GetChildNodes(DestinationNode node, Func<DestinationNode, bool> predicate) =>
            DestinationNodes.Where(destinationNode =>
                destinationNode.ParentNode?.Name == node.Name &&
                predicate(destinationNode));

        public static IEnumerable<TypeNode> GetNodes(Type type, Func<MemberInfo, bool> predicate)
        {
            List<TypeNode> returnValue = new List<TypeNode>();

            //List<TypeNode> nodes = TypeInfo.GetNodes(type, true, 0, 0, new Type[] { typeof(Type) }).ToList();
            List<TypeNode> nodes = TypeInfo.GetNodes(type, true).ToList();

            nodes.Sort((l, r) => l.Depth.CompareTo(r.Depth));

            for (int n = 0; n < nodes.Count; n++)
            {
                if (nodes[n].Depth == 0 ||
                    (
                       predicate(GetMember(nodes[n].Name, nodes)) &&
                       returnValue.Exists(w => w.Name == TypeInfo.GetNodeName(nodes[n].Name)
                    )))
                    returnValue.Add(nodes[n]);
            }

            return returnValue;
        }
    }
}
