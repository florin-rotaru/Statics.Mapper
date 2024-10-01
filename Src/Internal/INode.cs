using System;
using System.Reflection.Emit;

namespace Statics.Mapper.Internal
{
    internal interface INode
    {
        string Name { get; }
        int Depth { get; }
        Type Type { get; set; }
        bool IsStatic { get; }
        Type? NullableUnderlyingType { get; }
        LocalBuilder? Local { get; set; }
        LocalBuilder? NullableLocal { get; set; }

        MapperTypeMemberInfo? Member { get; set; }
    }
}
