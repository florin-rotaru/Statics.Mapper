using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Statics.Mapper.Internal
{
    internal class DestinationNode : INode
    {
        public string Name { get; }
        public int Depth { get; }
        public Type Type { get; }
        public bool IsStatic { get; }
        public Type? TypeAdapter { get; }
        public Type? NullableUnderlyingType { get; }
        public LocalBuilder? Local { get; set; }
        public LocalBuilder? NullableLocal { get; set; }
        public MapperTypeMemberInfo? Member { get; set; }

        public bool Loaded { get; set; }
        public bool Load { get; set; }

        public List<DestinationNodeMember> Members { get; }
        public DestinationNode? ParentNode { get; set; }
        public List<DestinationNode>? ParentNodes { get; set; }

        public int MembersMapCount { get; set; }

        public SourceNode? SourceNode { get; set; }
        public bool UseMapper { get; set; }

        public DestinationNode(NodeInfo node)
        {
            Name = node.Name;
            Depth = node.Depth;
            IsStatic = node.IsStatic;
            NullableUnderlyingType = node.NullableUnderlyingType;

            Type = node.Type;

            if (Type.IsGenericType &&
                TypeAdapters.TryGetAdapterGenericTypeDefinition(Type.GetGenericTypeDefinition(), out Type adapterGenericTypeDefinition))
            {
                TypeAdapter = adapterGenericTypeDefinition.MakeGenericType(Type.GenericTypeArguments);
                Members = MapperTypeInfo.GetMembers(TypeAdapter).Select(s => new DestinationNodeMember(s)).ToList();
            }
            else
            {
                Members = node.Members.Select(s => new DestinationNodeMember(s)).ToList();
            }
        }
    }
}
