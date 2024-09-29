using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Statics.Mapper.Internal
{
    internal class SourceNode(NodeInfo node) : INode
    {
        public string Name { get; } = node.Name;
        public int Depth { get; } = node.Depth;
        public Type Type { get; } = node.Type;
        public bool IsStatic { get; } = node.IsStatic;
        public Type? NullableUnderlyingType { get; } = node.NullableUnderlyingType;
        public LocalBuilder? Local { get; set; }
        public LocalBuilder? NullableLocal { get; set; }
        public bool Load { get; set; }
        public MapperTypeMemberInfo? Member { get; set; }
        public List<MapperTypeMemberInfo> Members { get; } = node.Members;
        public List<DestinationNode> DestinationNodes { get; set; } = [];
        public SourceNode? ParentNode { get; set; }
        public List<SourceNode>? ParentNodes { get; set; }
    }
}
