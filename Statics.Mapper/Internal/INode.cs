using Statics.Reflection;
using System;
using System.Reflection.Emit;

namespace Statics.Mapper.Internal
{
    internal interface INode
    {
        string Name { get; }
        int Depth { get; }
        Type Type { get; }
        bool IsStatic { get; }
        Type NullableUnderlyingType { get; }
        LocalBuilder Local { get; set; }
        LocalBuilder NullableLocal { get; set; }

        MemberInfo MemberInfo { get; set; }
    }
}
