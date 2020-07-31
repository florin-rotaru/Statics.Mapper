using Air.Reflection;
using System;

namespace Air.Mapper.Internal
{
    internal class Argument
    {
        public Type Type { get; private set; }
        public bool IsBuiltIn { get; private set; }
        public Type UnderlyingType { get; private set; }

        public Argument(Type type)
        {
            Type = type;
            IsBuiltIn = TypeInfo.IsBuiltIn(type);
            UnderlyingType = Nullable.GetUnderlyingType(type);
        }
    }
}
