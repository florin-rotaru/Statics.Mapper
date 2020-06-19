using Air.Reflection;
using Air.Reflection.Emit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper.Internal
{
    internal class Schema
    {
        public List<SourceNode> SourceNodes { get; private set; }
        public List<DestinationNode> DestinationNodes { get; private set; }
        public SourceNode SourceRootNode { get; private set; }
        public DestinationNode DestinationRootNode { get; private set; }

        private List<IMapOption> MapOptions { get; set; }

        private static readonly string Value = "Value";
        private static readonly char DOT = '.';

        public Schema(Type source, Type destination, List<IMapOption> mapOptions)
        {
            MapOptions = mapOptions;
            SetDefaultSchema(source, destination);
            ApplyOptions();
        }

        public void SetDefaultSchema(Type source, Type destination)
        {
            SourceNodes = GetNodes(source, (node) => node.HasGetMethod)
               .Select(s => new SourceNode(s)).ToList();

            SourceNodes.ForEach(s =>
            {
                s.ParentNode = GetParentNode(s);
                s.ParentNodes = GetParentNodes(s);
                s.MemberInfo = s.ParentNode?.Members.FirstOrDefault(w => w.Name == TypeInfo.GetName(s.Name));
            });

            DestinationNodes = GetNodes(destination, (node) => node.HasSetMethod && (node.HasDefaultConstructor || node.Type.IsValueType))
                .Select(s => new DestinationNode(s)).ToList();

            DestinationNodes.ForEach(d =>
            {
                d.ParentNode = GetParentNode(d);
                d.ParentNodes = GetParentNodes(d);
                d.MemberInfo = d.ParentNode?.Members.FirstOrDefault(w => w.Info.Name == TypeInfo.GetName(d.Name))?.Info;
            });

            SourceRootNode = SourceNodes.First(w => w.Depth == 0);
            DestinationRootNode = DestinationNodes.First(w => w.Depth == 0);

            Map(SourceRootNode, DestinationRootNode, MapOptions.Count != 0);

            if (MapOptions.Count == 0)
            {
                foreach (DestinationNode destinationNode in GetChildNodes(DestinationRootNode))
                {
                    foreach (SourceNode sourceNode in GetChildNodes(SourceRootNode, 1, n => n.Depth == destinationNode.Depth))
                    {
                        if (destinationNode.MemberInfo.Name == sourceNode.MemberInfo.Name)
                        {
                            MapperMap(sourceNode, destinationNode);
                            break;
                        }
                    }
                }
            }

            OnChange();
        }

        public void ApplyOptions()
        {
            for (int i = 0; i < MapOptions.Count; i++)
            {
                switch (MapOptions[i].Name)
                {
                    case nameof(MapOptions<IMapOption, IMapOption>.Ignore):
                        Ignore(MapOptions[i]);
                        break;
                    case nameof(MapOptions<IMapOption, IMapOption>.Map):
                        Map(MapOptions[i]);
                        break;
                    default:
                        throw new NotImplementedException($"Option {MapOptions[i].Name} not implemented!");
                }
            }
        }

        private void MapperMap(SourceNode sourceNode, DestinationNode destinationNode)
        {
            LoadDestinationNodePath(destinationNode);
            destinationNode.SourceNode = sourceNode;
            SetUseMapper(destinationNode, true);
        }

        private void Map(IMapOption option)
        {
            MapOption mapOption = new MapOption(option);
            Map(mapOption.SourceMemberName, mapOption.DestinationMemberName, mapOption.Expand);
        }

        private void Map(SourceNode sourceNode, DestinationNode destinationNode, bool expand)
        {
            foreach (var destinationNodeMember in destinationNode.Members)
            {
                if (!destinationNodeMember.Info.HasSetMethod)
                    continue;

                MemberInfo sourceNodeMember =
                    sourceNode.Members.FirstOrDefault(w => w.Name.Equals(destinationNodeMember.Info.Name, StringComparison.OrdinalIgnoreCase));

                if (sourceNodeMember == null)
                    continue;

                if (sourceNodeMember.IsBuiltIn != destinationNodeMember.Info.IsBuiltIn)
                    continue;

                if (IsSourceNode(NodeMemberName(sourceNode.Name, sourceNodeMember.Name)) &&
                    IsDestinationNode(NodeMemberName(destinationNode.Name, destinationNodeMember.Info.Name)))
                {
                    SourceNode memberSourceNode = SourceNodes.First(node => node.Name == NodeMemberName(sourceNode.Name, sourceNodeMember.Name));
                    DestinationNode memberDestinationNode = DestinationNodes.First(node => node.Name == NodeMemberName(destinationNode.Name, destinationNodeMember.Info.Name));

                    if (expand)
                        Map(memberSourceNode, memberDestinationNode, expand);
                }
                else
                {
                    TryMapMember(sourceNode, sourceNodeMember, destinationNode, destinationNodeMember);
                }
            }
        }

        private bool EvaluateMap(SourceNode sourceNode, DestinationNode destinationNode)
        {
            if (sourceNode.IsStatic && destinationNode.IsStatic &&
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

                break;
            }

            return true;
        }

        public void Map(string sourceMember, string destinationMember, bool expand)
        {
            sourceMember = ResolveSourceMemberName(sourceMember);
            destinationMember = ResolveDestinationMemberName(destinationMember);

            bool sourceMemberIsNode = IsSourceNode(sourceMember);
            bool destinationMemberIsNode = IsDestinationNode(destinationMember);

            SourceNode sourceNode = null;
            MemberInfo sourceNodeMember = null;
            DestinationNode destinationNode = null;
            DestinationNodeMember destinationNodeMember = null;

            if (sourceMemberIsNode && destinationMemberIsNode)
            {
                sourceNode = SourceNodes.First(w => w.Name == sourceMember);
                destinationNode = DestinationNodes.First(w => w.Name == destinationMember);

                if (expand &&
                    sourceNode.Depth != 0 &&
                    destinationNode.Depth != 0)
                {
                    MapperMap(sourceNode, destinationNode);
                    OnChange();
                }
                else
                {
                    if (!EvaluateMap(sourceNode, destinationNode))
                        throw new InvalidOperationException($"Cannot map from {sourceMember} to {destinationMember}.");

                    Map(sourceNode, destinationNode, expand);
                    SetUseMapper(destinationNode, false);
                    OnChange();
                }

                return;
            }

            try
            {
                if (sourceMemberIsNode != destinationMemberIsNode)
                    throw new InvalidOperationException();

                sourceNode = SourceNodes.First(w => w.Name == TypeInfo.GetNodeName(sourceMember));
                sourceNodeMember = sourceNode.Members.First(w => w.Name == TypeInfo.GetName(sourceMember));

                destinationNode = DestinationNodes.First(w => w.Name == TypeInfo.GetNodeName(destinationMember));
                destinationNodeMember = destinationNode.Members.First(w => w.Info.Name == TypeInfo.GetName(destinationMember));

                if (!EvaluateMap(sourceNode, destinationNode))
                    throw new InvalidOperationException();

                if (!TryMapMember(
                    sourceNode,
                    sourceNodeMember,
                    destinationNode,
                    destinationNodeMember))
                    throw new InvalidOperationException();

                SetUseMapper(destinationNode, false);
                OnChange();
            }
            catch
            {
                throw new InvalidOperationException($"Cannot map from {sourceMember} to {destinationMember}.");
            }
        }

        private void Ignore(IMapOption option)
        {
            IgnoreOption ignoreOption = new IgnoreOption(option);
            Ignore(new[] { ignoreOption.DestinationMemberName });
        }

        private void Ignore(DestinationNode destinationNode)
        {
            destinationNode.Load = false;

            for (int i = 0; i < destinationNode.Members.Count; i++)
                destinationNode.Members[i].Map = false;

            List<DestinationNode> childNodes = GetChildNodes(destinationNode, childNode => childNode.Load);

            if (childNodes.Count == 0)
                SetUseMapper(destinationNode, false);

            for (int i = 0; i < childNodes.Count; i++)
                Ignore(childNodes[i]);
        }

        private void Ignore(string[] destinationMembers)
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

                SetUseMapper(node, false);
            }

            OnChange();
        }

        public void ForEachSourceNode(Func<SourceNode, bool> predicate, Action<SourceNode> action)
        {
            for (int i = 0; i < SourceNodes.Count; i++)
                if (predicate(SourceNodes[i]))
                    action(SourceNodes[i]);
        }

        public void ForEachDestinationNode(Func<DestinationNode, bool> predicate, Action<DestinationNode> action)
        {
            for (int i = 0; i < DestinationNodes.Count; i++)
                if (predicate(DestinationNodes[i]))
                    action(DestinationNodes[i]);
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

        private static bool IsCollection(Type type)
        {
            if (type.IsArray)
                return true;

            if (!type.IsGenericType)
                return false;

            if (type.GenericTypeArguments.Length == 1)
            {
                if (!type.GetInterfaces().Where(i => i.IsGenericType).Select(i => i.GetGenericTypeDefinition()).Contains(typeof(IEnumerable<>)))
                    return false;

                return type.GetConstructors().FirstOrDefault(c =>
                    c.GetParameters().Length == 1 &&
                    c.GetParameters()[0].ParameterType.IsGenericType &&
                    c.GetParameters()[0].ParameterType.GenericTypeArguments.Length == 1 &&
                    c.GetParameters()[0].ParameterType.GenericTypeArguments[0] == type.GenericTypeArguments[0] &&
                    typeof(IEnumerable<>).IsAssignableFrom(c.GetParameters()[0].ParameterType.GetGenericTypeDefinition())) != null;
            }

            if (type.GenericTypeArguments.Length == 2)
            {
                if (!type.GetInterfaces().Where(i => i.IsGenericType).Select(i => i.GetGenericTypeDefinition()).Contains(typeof(IDictionary<,>)))
                    return false;

                return type.GetInterfaces().Where(i => i.IsGenericType).Select(i => i.GetGenericTypeDefinition()).Contains(typeof(IDictionary<,>)) &&
                    type.GetConstructors().FirstOrDefault(c =>
                    c.GetParameters().Length == 1 &&
                    c.GetParameters()[0].ParameterType.IsGenericType &&
                    c.GetParameters()[0].ParameterType.GenericTypeArguments.Length == 2 &&
                    c.GetParameters()[0].ParameterType.GenericTypeArguments[0] == type.GenericTypeArguments[0] &&
                    c.GetParameters()[0].ParameterType.GenericTypeArguments[1] == type.GenericTypeArguments[1] &&
                    typeof(IDictionary<,>).IsAssignableFrom(c.GetParameters()[0].ParameterType.GetGenericTypeDefinition())) != null;
            }

            return false;
        }

        private static bool CanReadCollection(MemberInfo sourceNodeMember) =>
            sourceNodeMember.HasGetMethod && IsCollection(sourceNodeMember.Type);

        private static bool CanMaintainCollection(MemberInfo destinationNodeMember) =>
            destinationNodeMember.HasSetMethod && IsCollection(destinationNodeMember.Type);

        public static bool CanLoadAndSetCollectionValue(
            MemberInfo sourceNodeMember,
            MemberInfo destinationNodeMember) =>
                CanReadCollection(sourceNodeMember) &&
                CanMaintainCollection(destinationNodeMember) &&
                !destinationNodeMember.Type.IsInterface &&
                (sourceNodeMember.Type.IsArray || sourceNodeMember.Type.GenericTypeArguments.Length == 1) ==
                (destinationNodeMember.Type.IsArray || destinationNodeMember.Type.GenericTypeArguments.Length == 1);

        private bool TryMapMember(
            SourceNode sourceNode,
            MemberInfo sourceNodeMember,
            DestinationNode destinationNode,
            DestinationNodeMember destinationNodeMember)
        {
            if (ILGenerator.CanEmitLoadAndSetValue(sourceNodeMember, destinationNodeMember.Info))
            {
                destinationNodeMember.IsCollection = IsCollection(destinationNodeMember.Info.Type);
                destinationNodeMember.Map = true;
                destinationNodeMember.SourceNode = sourceNode;
                destinationNodeMember.SourceNodeMember = sourceNodeMember;
                destinationNodeMember.Status = Status.Successful;
                LoadDestinationNodePath(destinationNode);
                return true;
            }

            if (CanLoadAndSetCollectionValue(sourceNodeMember, destinationNodeMember.Info))
            {
                destinationNodeMember.IsCollection = true;
                destinationNodeMember.Map = true;
                destinationNodeMember.SourceNode = sourceNode;
                destinationNodeMember.SourceNodeMember = sourceNodeMember;
                destinationNodeMember.Status = Status.Successful;
                LoadDestinationNodePath(destinationNode);
                return true;
            }

            destinationNodeMember.Map = false;
            destinationNodeMember.SourceNode = sourceNode;
            destinationNodeMember.SourceNodeMember = sourceNodeMember;
            destinationNodeMember.Status = Status.Failed;
            return false;
        }

        public static string NodeMemberName(string nodeName, string memberName) =>
            string.IsNullOrEmpty(nodeName) ? memberName : nodeName + DOT + memberName;

        private void OnChange()
        {
            for (int s = 0; s < SourceNodes.Count; s++)
                SourceNodes[s].Load = false;

            foreach (DestinationNode destinationNode in DestinationNodes)
            {
                destinationNode.MembersMapCount = 0;

                if (!destinationNode.Load)
                    continue;

                destinationNode.MembersMapCount =
                   destinationNode.Members.Count(n => n.Map && n.Status == Status.Successful);

                if (destinationNode.UseMapper)
                {
                    LoadSourceNodePath(destinationNode.SourceNode);
                }
                else
                {
                    for (int m = 0; m < destinationNode.Members.Count; m++)
                    {
                        if (destinationNode.Members[m].Status != Status.Successful)
                            continue;

                        LoadSourceNodePath(destinationNode.Members[m].SourceNode);
                    }
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

        private void SetUseMapper(DestinationNode destinationNode, bool value)
        {
            foreach (DestinationNode parentNode in destinationNode.ParentNodes)
                parentNode.UseMapper = false;

            destinationNode.UseMapper = value;
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
            memberName == string.Empty || SourceNodes.Exists(w => w.Name == memberName);

        private bool IsDestinationNode(string memberName) =>
            memberName == string.Empty || DestinationNodes.Exists(w => w.Name == memberName);

        public List<DestinationNodeMember> GetDestinationNodeMembers(DestinationNode destinationNode, Func<DestinationNodeMember, bool> predicate) =>
            destinationNode.Members.Where(predicate).ToList();

        public static MemberInfo GetMember(string nodeName, List<TypeNode> nodes) =>
            nodes.First(w => w.Name == TypeInfo.GetNodeName(nodeName))
                .Members.First(w => w.Name == TypeInfo.GetName(nodeName));

        public static MemberInfo GetMember(string nodeName, List<SourceNode> nodes) =>
            nodes.First(w => w.Name == TypeInfo.GetNodeName(nodeName))
                .Members.First(w => w.Name == TypeInfo.GetName(nodeName));

        public static MemberInfo GetMember(string nodeName, List<DestinationNode> nodes) =>
            nodes.First(w => w.Name == TypeInfo.GetNodeName(nodeName))
                .Members.First(w => w.Info.Name == TypeInfo.GetName(nodeName)).Info;

        public SourceNode GetParentNode(SourceNode node) =>
            node.Name != string.Empty ? SourceNodes.FirstOrDefault(w => w.Name == TypeInfo.GetNodeName(node.Name)) : null;
        
        public List<SourceNode> GetParentNodes(SourceNode node)
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

        public List<DestinationNode> GetParentNodes(DestinationNode node)
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

        public List<SourceNode> GetChildNodes(SourceNode sourceNode, int depth = 1, Func<SourceNode, bool> predicate = null)
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

            return returnValue.Where(predicate).ToList();
        }

        public List<DestinationNode> GetChildNodes(DestinationNode node, Func<DestinationNode, bool> predicate) =>
            DestinationNodes.Where(destinationNode =>
                destinationNode.ParentNode?.Name == node.Name &&
                predicate(destinationNode))
            .ToList();

        public List<DestinationNode> GetChildNodes(DestinationNode node) =>
            DestinationNodes.Where(destinationNode => 
                destinationNode.ParentNode?.Name == node.Name)
            .ToList();

        public static List<TypeNode> GetNodes(Type type, Func<MemberInfo, bool> predicate)
        {
            List<TypeNode> returnValue = new List<TypeNode>();
            List<TypeNode> nodes = TypeInfo.GetNodes(type, true);
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
