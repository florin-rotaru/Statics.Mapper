using Air.Reflection;
using System;
using System.Reflection.Emit;

namespace Air.Mapper.Internal
{
    internal interface INode
    {
        string Name { get; }
        int Depth { get; }
        Type Type { get; }
        Type NullableUnderlyingType { get; }
        LocalBuilder Local { get; set; }
        LocalBuilder NullableLocal { get; set; }
        bool IsStatic { get; }

        MemberInfo MemberInfo { get; set; }
    }
}
