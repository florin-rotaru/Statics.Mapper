using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Statics.Mapper.Internal
{
    internal static class TypeAdapters
    {
        private static readonly ConcurrentDictionary<Type, Type> Adapters = new(
            new Dictionary<Type, Type>
            {
                { typeof(KeyValuePair<,>), typeof(KeyValue<,>) }
            });

        public static string ToMethodName(Type genericType) =>
            $"To{genericType.Name.Split('`')[0]}";

        public static bool ContainsAdapterGenericTypeDefinition(Type genericTypeDefinition) =>
            Adapters.ContainsKey(genericTypeDefinition);

        public static bool TryGetAdapterGenericTypeDefinition(Type genericTypeDefinition, out Type adapterGenericTypeDefinition) =>
            Adapters.TryGetValue(genericTypeDefinition, out adapterGenericTypeDefinition);
    }
}
