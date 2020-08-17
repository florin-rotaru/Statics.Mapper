using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Air.Mapper.Internal
{
    internal class Collections
    {
        public class Collection
        {
            public Type GenericTypeDefinition { get; }
            public Type LocalGenericTypeDefinition { get; }
            public bool IsImmutable { get; }

            public Type MakeLocalGenericType(params Type[] typeArguments) =>
                LocalGenericTypeDefinition.MakeGenericType(typeArguments);

            public Collection(Type genericTypeDefinition, Type localGenericTypeDefinition, bool isImmutable)
            {
                GenericTypeDefinition = genericTypeDefinition;
                LocalGenericTypeDefinition = localGenericTypeDefinition;
                IsImmutable = isImmutable;
            }
        }

        public static IEnumerable<Collection> MaintainableGenericCollections { get; } = new List<Collection>()
        {
            new Collection(typeof(IImmutableList<>), typeof(List<>), true),
            new Collection(typeof(IImmutableDictionary<,>), typeof(List<>), true),
            new Collection(typeof(IImmutableSet<>),typeof(List<>), true),
            new Collection(typeof(IImmutableQueue<>), typeof(List<>), true),
            new Collection(typeof(IImmutableStack<>), typeof(List<>), true),

            new Collection(typeof(IList<>), typeof(List<>), false),
            new Collection(typeof(IDictionary<,>), typeof(Dictionary<,>), false),
            new Collection(typeof(IProducerConsumerCollection<>), typeof(ConcurrentBag<>), false),

            new Collection(typeof(ICollection<>), typeof(List<>), false),

            new Collection(typeof(List<>), typeof(List<>), false),
            new Collection(typeof(Dictionary<,>), typeof(Dictionary<,>), false),
            new Collection(typeof(HashSet<>), typeof(HashSet<>), false),
            new Collection(typeof(LinkedList<>), typeof(LinkedList<>), false),
            new Collection(typeof(Queue<>), typeof(Queue<>), false),
            new Collection(typeof(SortedDictionary<,>), typeof(SortedDictionary<,>), false),
            new Collection(typeof(SortedList<,>), typeof(SortedList<,>), false),
            new Collection(typeof(SortedSet<>), typeof(SortedSet<>), false),
            new Collection(typeof(Stack<>), typeof(Stack<>), false),
            new Collection(typeof(BlockingCollection<>), typeof(BlockingCollection<>), false),

            new Collection(typeof(ConcurrentBag<>), typeof(ConcurrentBag<>), false),
            new Collection(typeof(ConcurrentDictionary<,>), typeof(ConcurrentDictionary<,>), false),
            new Collection(typeof(ConcurrentQueue<>), typeof(ConcurrentQueue<>), false),
            new Collection(typeof(ConcurrentStack<>), typeof(ConcurrentStack<>), false),

            new Collection(typeof(ImmutableArray<>), typeof(List<>), true),
            new Collection(typeof(ImmutableDictionary<,>), typeof(List<>), true),
            new Collection(typeof(ImmutableHashSet<>), typeof(List<>), true),
            new Collection(typeof(ImmutableList<>), typeof(List<>), true),
            new Collection(typeof(ImmutableQueue<>), typeof(List<>), true),
            new Collection(typeof(ImmutableSortedDictionary<,>), typeof(List<>), true),
            new Collection(typeof(ImmutableSortedSet<>), typeof(List<>), true),
            new Collection(typeof(ImmutableStack<>), typeof(List<>), true)
        };

        public static IEnumerable<Type> GetGenericTypeDefinitionBaseTypes(Type type)
        {
            Type baseType = type.BaseType;
            while (baseType != typeof(object))
            {
                if (baseType.IsGenericType)
                    yield return baseType.GetGenericTypeDefinition();
                baseType = baseType.BaseType;
            }
        }

        public static bool CanMaintainCollection(Type collectionType)
        {
            if (collectionType.IsInterface)
                return collectionType.IsGenericType && (
                    MaintainableGenericCollections.Any(t => t.GenericTypeDefinition == collectionType.GetGenericTypeDefinition()) ||
                    collectionType.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            bool isArrayOrHasDefaultConstructor = collectionType.IsArray || collectionType.GetConstructor(Type.EmptyTypes) != null;

            return
                (
                    collectionType.IsGenericType &&
                    MaintainableGenericCollections.Any(t => t.GenericTypeDefinition == collectionType.GetGenericTypeDefinition())
                ) ||
                (
                    isArrayOrHasDefaultConstructor &&
                    GetGenericTypeDefinitionBaseTypes(collectionType)
                        .Intersect(MaintainableGenericCollections.Select(s => s.GenericTypeDefinition))
                        .Any()
                ) ||
                (
                    isArrayOrHasDefaultConstructor &&
                    collectionType.GetInterfaces().Where(ci => ci.IsGenericType).Select(ci => ci.GetGenericTypeDefinition())
                        .Intersect(MaintainableGenericCollections.Select(s => s.GenericTypeDefinition))
                        .Any()
                ) ||
                collectionType.GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(new[] { GetIEnumerableArgument(collectionType) }) }) != null;
        }

        public static bool IsAssignableFrom(Type type, Type genericTypeInterface) =>
            Reflection.TypeInfo.ImplementsGenericTypeInterface(type, genericTypeInterface) ||
            (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == genericTypeInterface);

        public static bool IsCollection(Type type) =>
            !Reflection.TypeInfo.IsBuiltIn(type) &&
            IsAssignableFrom(type, typeof(IEnumerable<>));

        public static List<PropertyInfo> GetIndexParameters(Type type, Func<PropertyInfo, bool> predicate = null) =>
            type.GetProperties().Where(p => p.GetIndexParameters().Length != 0 && (predicate == null || predicate(p))).ToList();

        public static bool ToKeyValuePairCollectionArgumentPredicate(Type argument) =>
           argument.IsGenericType &&
           argument.GetGenericTypeDefinition() == typeof(KeyValuePair<,>);

        public static Type GetIEnumerableArgument(Type collectionType) =>
            collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(IEnumerable<>) ?
            collectionType.GenericTypeArguments[0] :
            Reflection.TypeInfo.GetGenericTypeInterface(collectionType, typeof(IEnumerable<>)).GenericTypeArguments[0];

        private static bool IEnumerableParameterTypePredicate(Type parameterType, Func<Type, bool> enumerableArgumentPredicate = null) =>
            parameterType.IsGenericType && (
            (
                parameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                (
                    enumerableArgumentPredicate == null ||
                    enumerableArgumentPredicate(parameterType.GenericTypeArguments[0])
                )
            ) ||
            (
                Reflection.TypeInfo.ImplementsGenericTypeInterface(parameterType, typeof(IEnumerable<>)) &&
                (
                    enumerableArgumentPredicate == null ||
                    enumerableArgumentPredicate(Reflection.TypeInfo.GetGenericTypeInterface(parameterType, typeof(IEnumerable<>)).GenericTypeArguments[0])
                )
            ));

        private static bool IEnumerableParametersPredicate(ParameterInfo[] parameters, Func<Type, bool> enumerableArgumentPredicate = null) =>
                parameters.Length == 1 &&
                IEnumerableParameterTypePredicate(parameters[0].ParameterType, enumerableArgumentPredicate);

        public static MethodInfo[] GetGenericIEnumerableParameterTypeMethods(Type type, Func<Type, bool> enumerableArgumentPredicate = null) =>
            type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(c => IEnumerableParametersPredicate(c.GetParameters(), enumerableArgumentPredicate))
                .ToArray();

        public static ConstructorInfo GetGenericIEnumerableParameterTypeConstructor(Type collectionType, Func<Type, bool> enumerableArgumentPredicate = null) =>
            collectionType.GetConstructors().FirstOrDefault(c => IEnumerableParametersPredicate(c.GetParameters(), enumerableArgumentPredicate));

        public static ConstructorInfo GetGenericIEnumerableAddMethod(Type collectionType, Func<Type, bool> enumerableArgumentPredicate = null) =>
            collectionType.GetConstructors().FirstOrDefault(c => IEnumerableParametersPredicate(c.GetParameters(), enumerableArgumentPredicate));
    }
}
