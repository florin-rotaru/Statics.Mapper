using Air.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Air.Mapper.Internal
{
    internal class DestinationNode : INode
    {
        public string Name { get; set; }
        public int Depth { get; set; }
        public Type Type { get; set; }
        public Type NullableUnderlyingType { get; set; }
        public LocalBuilder Local { get; set; }
        public LocalBuilder NullableLocal { get; set; }
        public bool IsStatic { get; set; }

        public bool Loaded { get; set; }
        public bool Load { get; set; }

        public List<DestinationNodeMember> Members { get; set; }
        public DestinationNode ParentNode { get; set; }
        public List<DestinationNode> ParentNodes { get; set; }
        public MemberInfo MemberInfo { get; set; }

        public int MembersMapCount { get; set; }

        public DestinationNode(TypeNode node)
        {
            Name = node.Name;
            Depth = node.Depth;
            Type = node.Type;
            IsStatic = node.IsStatic;
            NullableUnderlyingType = node.NullableUnderlyingType;
            Members = node.Members.Select(s => new DestinationNodeMember(s)).ToList();
        }
    }
}
