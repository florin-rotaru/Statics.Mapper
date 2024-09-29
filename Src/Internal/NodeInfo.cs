using System;
using System.Collections.Generic;

namespace Statics.Mapper.Internal
{
    internal class NodeInfo
    {
        public string Name { get; set; }
        public int Depth { get; set; }
        public Type Type { get; set; }
        public bool IsStatic { get; set; }
        public Type? NullableUnderlyingType { get; set; }
        public List<MapperTypeMemberInfo> Members { get; set; }

        public object? Value { get; set; }

        public NodeInfo(string name, int depth, Type type, bool isStatic, bool useNullableUnderlyingTypeMembers)
        {
            Name = name;
            Depth = depth;
            Type = type;
            IsStatic = isStatic;
            NullableUnderlyingType = Nullable.GetUnderlyingType(type);

            Members = MapperTypeInfo.GetMembers(
                useNullableUnderlyingTypeMembers && NullableUnderlyingType != null ? NullableUnderlyingType : type,
                false);
        }
    }
}