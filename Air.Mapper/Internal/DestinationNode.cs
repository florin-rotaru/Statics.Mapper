using Air.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Air.Mapper.Internal
{
    internal class DestinationNode : INode
    {
        public string Name { get; }
        public int Depth { get; }
        public Type Type { get; }
        public Type NullableUnderlyingType { get; }
        public LocalBuilder Local { get; set; }
        public LocalBuilder NullableLocal { get; set; }
        public bool IsStatic { get; }
        public MemberInfo MemberInfo { get; set; }

        public bool Loaded { get; set; }
        public bool Load { get; set; }

        public IEnumerable<DestinationNodeMember> Members { get; }
        public DestinationNode ParentNode { get; set; }
        public List<DestinationNode> ParentNodes { get; set; }

        public int MembersMapCount { get; set; }

        public bool IsKeyValuePair { get; }

        public DestinationNode(TypeNode node)
        {
            Name = node.Name;
            Depth = node.Depth;
            IsStatic = node.IsStatic;
            NullableUnderlyingType = node.NullableUnderlyingType;

            Type = node.Type;

            if (Type.IsGenericType &&
                Type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
            {
                Type = typeof(KeyValue<,>).MakeGenericType(new Type[] { Type.GenericTypeArguments[0], Type.GenericTypeArguments[1] });
                IsKeyValuePair = true;
                Members = TypeInfo.GetMembers(Type).Select(s => new DestinationNodeMember(s)).ToList();
            }
            else
            {
                Members = node.Members.Select(s => new DestinationNodeMember(s)).ToList();
            }
        }
    }
}
