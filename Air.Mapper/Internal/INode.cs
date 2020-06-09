using Air.Reflection;
using System;
using System.Reflection.Emit;

namespace Air.Mapper.Internal
{
    internal interface INode
    {
        string Name { get; set; }
        int Depth { get; set; }
        Type Type { get; set; }
        Type NullableUnderlyingType { get; set; }
        LocalBuilder Local { get; set; }
        LocalBuilder NullableLocal { get; set; }
        bool IsStatic { get; set; }

        MemberInfo MemberInfo { get; set; }
    }
}
