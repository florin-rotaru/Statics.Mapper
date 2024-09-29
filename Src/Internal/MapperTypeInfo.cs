using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Statics.Mapper.Internal
{
    internal static class MapperTypeInfo
    {
        const char DOT = '.';

        public static readonly Type[] NumericBuiltInTypes =
        [
            typeof(sbyte),
            typeof(byte),

            typeof(short),
            typeof(ushort),

            typeof(int),
            typeof(uint),

            typeof(long),
            typeof(ulong),

            typeof(float),
            typeof(double),
            typeof(decimal)
        ];

        /// <summary>
        /// Gets the underlying type code of the specified System.Type and compares it against numeric ones
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumeric(Type type)
        {
            Type nType = Nullable.GetUnderlyingType(type) ?? type;
            for (int i = 0; i < NumericBuiltInTypes.Length; i++)
                if (NumericBuiltInTypes[i] == nType)
                    return true;

            return false;
        }

        public static bool IsEnumerable(Type type) =>
            type == typeof(IEnumerable) || typeof(IEnumerable).IsAssignableFrom(type);

        public static bool IsStatic(Type type) =>
            type.IsAbstract && type.IsSealed;

        static readonly Type[] BuiltInTypes =
        [
            typeof(bool),

            typeof(char),

            typeof(sbyte),
            typeof(byte),

            typeof(short),
            typeof(ushort),

            typeof(int),
            typeof(uint),

            typeof(long),
            typeof(ulong),

            typeof(float),
            typeof(double),

            typeof(decimal),

            typeof(string),

            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(Enum),
            typeof(Guid),
            typeof(TimeSpan),

            typeof(object)
        ];

        static bool BuiltInTypesContains(Type type)
        {
            for (int i = 0; i < BuiltInTypes.Length; i++)
                if (BuiltInTypes[i] == type)
                    return true;

            return false;
        }

        public static bool IsEnum(Type type) =>
            type.IsEnum || (Nullable.GetUnderlyingType(type) ?? type).IsEnum;

        public static bool IsBuiltIn(Type type) =>
            type.IsPrimitive ||
            IsEnum(type) ||
            BuiltInTypesContains(type) ||
            BuiltInTypesContains(Nullable.GetUnderlyingType(type) ?? type);

        /// <summary>
        /// Determines whether the type implements generic type interface (example: IEnumerable<>)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericTypeInterface"></param>
        /// <returns></returns>
        public static bool ImplementsGenericTypeInterface(Type type, Type genericTypeInterface) =>
           type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeInterface);

        /// <summary>
        /// Returns the interface which is of type generic 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericTypeInterface"></param>
        /// <returns></returns>
        public static Type? GetGenericTypeInterface(Type type, Type genericTypeInterface) =>
            type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeInterface);

        public static List<NodeInfo> GetNodes(
            Type type,
            bool useNullableUnderlyingTypeMembers,
            int recursion = 0,
            int depth = 0,
            Type[]? ignoreNodeTypes = null)
        {
            if (recursion < 0)
                throw new ArgumentException(null, nameof(recursion));

            List<NodeInfo> nodes = [];

            Queue<NodeInfo> queue = new([
                new NodeInfo(string.Empty, 0, type, IsStatic(type), useNullableUnderlyingTypeMembers)
            ]);

            while (queue.Count > 0)
            {
                NodeInfo queueNode = queue.Dequeue();

                nodes.Add(queueNode);

                foreach (MapperTypeMemberInfo member in queueNode.Members)
                {
                    if (member.IsEnum ||
                        member.IsBuiltIn ||
                        member.IsEnumerable ||
                        (depth != 0 && queueNode.Depth >= depth) ||
                        (ignoreNodeTypes != null && ignoreNodeTypes.Contains(member.Type)))
                        continue;

                    NodeInfo node = new(
                        queueNode.Name == string.Empty ? member.Name : queueNode.Name + DOT + member.Name,
                        queueNode.Depth + 1,
                        member.Type,
                        member.IsStatic,
                        useNullableUnderlyingTypeMembers);

                    int occurrence = nodes.Count(n =>
                        n.Depth < node.Depth &&
                        (
                            n.Type == node.Type ||
                            n.Type == node.NullableUnderlyingType
                        ));

                    if (occurrence <= recursion)
                        queue.Enqueue(node);
                    else
                        nodes.Add(node);
                }
            }

            return nodes;
        }

        public static string GetNodeName(string member)
        {
            if (member != null)
                for (int c = member.Length - 1; c > -1; c--)
                    if (member[c] == DOT)
                        return member[..c];

            return string.Empty;
        }

        static readonly ConcurrentDictionary<Type, object?> ValueTypesDefaultValue = new();

        static object? GetValueTypeDefaultValue(Type valueType)
        {
            if (ValueTypesDefaultValue.TryGetValue(valueType, out object? value))
            {
                return value;
            }
            else
            {
                value = Activator.CreateInstance(valueType);
                ValueTypesDefaultValue.TryAdd(valueType, value);
                return value;
            }
        }

        public static object? GetDefaultValue(Type type) =>
            type.IsValueType && Nullable.GetUnderlyingType(type) == null ? GetValueTypeDefaultValue(type) : default;

        public static object? GetDefaultValue(Type type, FieldInfo field)
        {
            try
            {
                object? instance = Activator.CreateInstance(type);
                return field.GetValue(instance);
            }
            catch
            {
                return GetDefaultValue(field.FieldType);
            }
        }

        public static object? GetDefaultValue(Type type, PropertyInfo property)
        {
            try
            {
                object? instance = Activator.CreateInstance(type);
                return property.GetValue(instance, null);
            }
            catch
            {
                return GetDefaultValue(property.PropertyType);
            }
        }

        public static string GetName(string member)
        {
            if (member != null)
                for (int c = member.Length - 1; c > -1; c--)
                    if (member[c] == DOT)
                        return member[(c + 1)..];

            return member ?? string.Empty;
        }

        public static List<MapperTypeMemberInfo> GetMembers(Type type, bool useNullableUnderlyingTypeMembers = false)
        {
            Type targetType = useNullableUnderlyingTypeMembers ? Nullable.GetUnderlyingType(type) ?? type : type;

            List<MapperTypeMemberInfo> members = new(
                targetType.GetProperties()
                    .Where(p => p.DeclaringType != null && p.GetIndexParameters().Length == 0)
                    .Select(s => { return new MapperTypeMemberInfo(targetType, s); }));

            members.AddRange(
                targetType.GetFields().Select(s => { return new MapperTypeMemberInfo(targetType, s); })
            );

            return members;
        }
    }
}
