using Air.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Air.Mapper.Internal
{
    internal class SourceNode : INode
    {
        public string Name { get;  }
        public int Depth { get; }
        public Type Type { get; }
        public Type NullableUnderlyingType { get; }
        public LocalBuilder Local { get; set; }
        public LocalBuilder NullableLocal { get; set; }
        public bool IsStatic { get; }
        public MemberInfo MemberInfo { get; set; }

        public bool Load { get; set; }

        public IEnumerable<MemberInfo> Members { get; }

        public List<DestinationNode> DestinationNodes { get; set; }

        public SourceNode ParentNode { get; set; }
        public List<SourceNode> ParentNodes { get; set; }
       
        public SourceNode(TypeNode node) 
        {
            Name = node.Name;
            Depth = node.Depth;
            Type = node.Type;
            IsStatic = node.IsStatic;
            NullableUnderlyingType = node.NullableUnderlyingType;
            Members = node.Members;

            DestinationNodes = new List<DestinationNode>();
        }
    }
}
