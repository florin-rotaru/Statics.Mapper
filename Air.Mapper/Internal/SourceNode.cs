using Air.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Air.Mapper.Internal
{
    internal class SourceNode : INode
    {
        public string Name { get; set; }
        public int Depth { get; set; }
        public Type Type { get; set; }
        public Type NullableUnderlyingType { get; set; }
        public LocalBuilder Local { get; set; }
        public LocalBuilder NullableLocal { get; set; }
        public bool IsStatic { get; set; }

        public bool Load { get; set; }

        public List<MemberInfo> Members { get; set; }

        public List<DestinationNode> DestinationNodes { get; set; }

        public SourceNode ParentNode { get; set; }
        public List<SourceNode> ParentNodes { get; set; }
        public MemberInfo MemberInfo { get; set; }

        public SourceNode()
        {
            DestinationNodes = new List<DestinationNode>();
        }

        public SourceNode(TypeNode node) : this()
        {
            Name = node.Name;
            Depth = node.Depth;
            Type = node.Type;
            IsStatic = node.IsStatic;
            NullableUnderlyingType = node.NullableUnderlyingType;
            Members = node.Members;
        }
    }
}
