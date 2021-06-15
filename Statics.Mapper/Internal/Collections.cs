using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Statics.Mapper.Internal
{
    internal class Collections
    {
        public class MaintainableCollectionInfo
        {
            public Type GenericTypeDefinition { get; }
            public Type LocalGenericTypeDefinition { get; }

            public bool IsImmutable { get; }

            public MaintainableCollectionInfo(
                Type genericTypeDefinition,
                Type localGenericTypeDefinition)
            {
                GenericTypeDefinition = genericTypeDefinition;
                LocalGenericTypeDefinition = localGenericTypeDefinition;
                IsImmutable = IsImmutable(genericTypeDefinition);
            }

            public MaintainableCollectionInfo(
                Type genericTypeDefinition) : this(genericTypeDefinition, genericTypeDefinition)
            { }
        }

        public class ImmutableCollectionBuilderInfo
        {
            public Type GenericTypeDefinition { get; }
            public Type BuilderGenericTypeDefinition { get; }

            public ImmutableCollectionBuilderInfo(
                Type genericTypeDefinition,
                Type builderGenericTypeDefinition)
            {
                GenericTypeDefinition = genericTypeDefinition;
                BuilderGenericTypeDefinition = builderGenericTypeDefinition;
            }
        }

        public static IEnumerable<MaintainableCollectionInfo> MaintainableGenericCollections { get; } = new List<MaintainableCollectionInfo>()
        {
            new MaintainableCollectionInfo(typeof(IImmutableList<>), typeof(List<>)),
            new MaintainableCollectionInfo(typeof(IImmutableDictionary<,>), typeof(List<>)),
            new MaintainableCollectionInfo(typeof(IImmutableSet<>),typeof(List<>)),
            new MaintainableCollectionInfo(typeof(IImmutableQueue<>), typeof(List<>)),
            new MaintainableCollectionInfo(typeof(IImmutableStack<>), typeof(List<>)),

            new MaintainableCollectionInfo(typeof(IList<>), typeof(List<>)),
            new MaintainableCollectionInfo(typeof(IDictionary<,>), typeof(Dictionary<,>)),
            new MaintainableCollectionInfo(typeof(IProducerConsumerCollection<>), typeof(ConcurrentBag<>)),

            new MaintainableCollectionInfo(typeof(ICollection<>), typeof(List<>)),

            new MaintainableCollectionInfo(typeof(List<>)),
            new MaintainableCollectionInfo(typeof(Dictionary<,>)),
            new MaintainableCollectionInfo(typeof(HashSet<>)),
            new MaintainableCollectionInfo(typeof(LinkedList<>)),
            new MaintainableCollectionInfo(typeof(Queue<>)),
            new MaintainableCollectionInfo(typeof(SortedDictionary<,>)),
            new MaintainableCollectionInfo(typeof(SortedList<,>)),
            new MaintainableCollectionInfo(typeof(SortedSet<>)),
            new MaintainableCollectionInfo(typeof(Stack<>)),
            new MaintainableCollectionInfo(typeof(BlockingCollection<>)),

            new MaintainableCollectionInfo(typeof(ConcurrentBag<>)),
            new MaintainableCollectionInfo(typeof(ConcurrentDictionary<,>)),
            new MaintainableCollectionInfo(typeof(ConcurrentQueue<>)),
            new MaintainableCollectionInfo(typeof(ConcurrentStack<>)),

            new MaintainableCollectionInfo(typeof(ImmutableArray<>), typeof(List<>)),
            new MaintainableCollectionInfo(typeof(ImmutableDictionary<,>), typeof(List<>)),
            new MaintainableCollectionInfo(typeof(ImmutableHashSet<>), typeof(List<>)),
            new MaintainableCollectionInfo(typeof(ImmutableList<>), typeof(List<>)),
            new MaintainableCollectionInfo(typeof(ImmutableQueue<>), typeof(List<>)),
            new MaintainableCollectionInfo(typeof(ImmutableSortedDictionary<,>), typeof(List<>)),
            new MaintainableCollectionInfo(typeof(ImmutableSortedSet<>), typeof(List<>)),
            new MaintainableCollectionInfo(typeof(ImmutableStack<>), typeof(List<>))
        };

        public static IEnumerable<ImmutableCollectionBuilderInfo> ImmutableGenericCollectionBuilders = new List<ImmutableCollectionBuilderInfo>()
        {
            new ImmutableCollectionBuilderInfo(typeof(IImmutableList<>), typeof(ImmutableList)),
            new ImmutableCollectionBuilderInfo(typeof(IImmutableDictionary<,>), typeof(ImmutableDictionary)),
            new ImmutableCollectionBuilderInfo(typeof(IImmutableSet<>), typeof(ImmutableHashSet)),
            new ImmutableCollectionBuilderInfo(typeof(IImmutableQueue<>), typeof(ImmutableQueue)),
            new ImmutableCollectionBuilderInfo(typeof(IImmutableStack<>), typeof(ImmutableStack)),
            new ImmutableCollectionBuilderInfo(typeof(ImmutableArray<>), typeof(ImmutableArray)),
            new ImmutableCollectionBuilderInfo(typeof(ImmutableDictionary<,>), typeof(ImmutableDictionary)),
            new ImmutableCollectionBuilderInfo(typeof(ImmutableHashSet<>), typeof(ImmutableHashSet)),
            new ImmutableCollectionBuilderInfo(typeof(ImmutableList<>), typeof(ImmutableList)),
            new ImmutableCollectionBuilderInfo(typeof(ImmutableQueue<>), typeof(ImmutableQueue)),
            new ImmutableCollectionBuilderInfo(typeof(ImmutableSortedDictionary<,>), typeof(ImmutableSortedDictionary)),
            new ImmutableCollectionBuilderInfo(typeof(ImmutableSortedSet<>), typeof(ImmutableSortedSet)),
            new ImmutableCollectionBuilderInfo(typeof(ImmutableStack<>), typeof(ImmutableStack))
        };

        public static bool IsImmutable(Type genericTypeDefinition)
        {
            string collectionsImmutableNamespace = typeof(IImmutableList<Type>).Namespace;
            return genericTypeDefinition.Namespace == collectionsImmutableNamespace ||
                genericTypeDefinition.GetInterfaces().Any(i => i.Namespace == collectionsImmutableNamespace);
        }

        public static IEnumerable<Type> GetGenericTypeDefinitionBaseTypes(Type type)
        {
            Type baseType = type.BaseType;
            if (baseType == null)
            {
                if (type.IsGenericType)
                    yield return type;
            }
            else
            {
                while (baseType != typeof(object))
                {
                    if (baseType.IsGenericType)
                        yield return baseType.GetGenericTypeDefinition();
                    baseType = baseType.BaseType;
                }
            }
        }

        public static MethodInfo GetCollectionAddMethod(Type collectionType)
        {
            if (IsAssignableFrom(collectionType, typeof(ICollection<>)))
                return typeof(ICollection<>).MakeGenericType(GetIEnumerableArgument(collectionType)).GetMethod(nameof(ICollection<Type>.Add));

            string[] addMethodNames = new string[] { "Add", "TryAdd", "Push", "Enqueue" };
            Type iEnumerableArgument = GetIEnumerableArgument(collectionType);

            return collectionType.GetMethods().FirstOrDefault(m =>
                addMethodNames.Contains(m.Name) &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == iEnumerableArgument);
        }

        private static Type MakeLocalGenericType(
            Type destinationType,
            MaintainableCollectionInfo maintainableCollectionInfo)
        {
            Type destinationIEnumerableArgument = GetIEnumerableArgument(destinationType);

            int genericTypeDefinitionArgumentsLength = maintainableCollectionInfo.GenericTypeDefinition.GetGenericArguments().Length;
            int localTypeDefinitionArgumentsLength = maintainableCollectionInfo.LocalGenericTypeDefinition.GetGenericArguments().Length;

            if (genericTypeDefinitionArgumentsLength == localTypeDefinitionArgumentsLength)
                return maintainableCollectionInfo.LocalGenericTypeDefinition.MakeGenericType(destinationType.GenericTypeArguments);
            else if (genericTypeDefinitionArgumentsLength > localTypeDefinitionArgumentsLength)
                return maintainableCollectionInfo.LocalGenericTypeDefinition.MakeGenericType(destinationIEnumerableArgument);
            else
                return maintainableCollectionInfo.LocalGenericTypeDefinition.MakeGenericType(new Type[]
                    {
                        destinationIEnumerableArgument.GenericTypeArguments[0],
                        destinationIEnumerableArgument.GenericTypeArguments[1]
                    });
        }

        public static bool TryGetCollectionLocalType(
            Type sourceType,
            Type destinationType,
            out Type localType)
        {
            localType = null;
            MaintainableCollectionInfo maintainableCollectionInfo;
            Type intersectType;

            Type destinationIEnumerableArgument = GetIEnumerableArgument(destinationType);
            Type destinationGenericTypeDefinition =
                destinationType.IsGenericType ? destinationType.GetGenericTypeDefinition() : null;

            bool loopIndex =
                sourceType.IsArray ||
                IsAssignableFrom(sourceType, typeof(IList<>)) ||
                IsAssignableFrom(sourceType, typeof(IReadOnlyList<>)) ||
                IsAssignableFrom(sourceType, typeof(ICollection<>)) ||
                IsAssignableFrom(sourceType, typeof(IProducerConsumerCollection<>)) ||
                sourceType.GetMethods().Any(m =>
                    m.Name == nameof(List<Type>.ToArray) &&
                    m.GetParameters().Length == 0 &&
                    m.ReturnType == GetIEnumerableArgument(sourceType).MakeArrayType());

            if (destinationType.IsInterface)
            {
                if (destinationGenericTypeDefinition == typeof(IEnumerable<>))
                {
                    if (loopIndex)
                        localType = destinationIEnumerableArgument.MakeArrayType();
                    else
                        localType = typeof(List<>).MakeGenericType(destinationIEnumerableArgument);
                }
                else
                {
                    maintainableCollectionInfo = MaintainableGenericCollections.FirstOrDefault(c => c.GenericTypeDefinition == destinationGenericTypeDefinition);
                    if (maintainableCollectionInfo != null)
                        localType = MakeLocalGenericType(destinationType, maintainableCollectionInfo);
                }

                return localType != null;
            }


            bool hasDefaultConstructor = destinationType.GetConstructor(Type.EmptyTypes) != null;

            if (destinationType.IsArray)
            {
                if (loopIndex)
                    localType = destinationType;
                else
                    localType = typeof(List<>).MakeGenericType(destinationIEnumerableArgument);
            }
            else if (
                hasDefaultConstructor &&
                MaintainableGenericCollections.Select(s => s.GenericTypeDefinition)
                    .Intersect(GetGenericTypeDefinitionBaseTypes(destinationType))
                    .Any())
            {
                intersectType = MaintainableGenericCollections.Select(s => s.GenericTypeDefinition)
                    .Intersect(GetGenericTypeDefinitionBaseTypes(destinationType))
                    .FirstOrDefault();

                localType = MakeLocalGenericType(destinationType, MaintainableGenericCollections.First(t => t.GenericTypeDefinition == intersectType));
            }
            else if (
                hasDefaultConstructor &&
                GetCollectionAddMethod(destinationType) != null)
            {
                localType = destinationType;
            }
            else if (destinationType.GetConstructors().Any(c =>
                c.GetParameters().Length == 1 &&
                c.GetParameters()[0].ParameterType == typeof(IEnumerable<>).MakeGenericType(destinationIEnumerableArgument)))
            {
                localType = typeof(List<>).MakeGenericType(destinationIEnumerableArgument);
            }
            else if (
                IsImmutable(destinationGenericTypeDefinition) &&
                MaintainableGenericCollections.Any(s => s.GenericTypeDefinition == destinationGenericTypeDefinition))
            {
                localType = MakeLocalGenericType(destinationType, MaintainableGenericCollections.First(t => t.GenericTypeDefinition == destinationGenericTypeDefinition));
            }

            return localType != null;
        }

        public static bool CanMaintainCollection(
            Type sourceType,
            Type destinationType) =>
            TryGetCollectionLocalType(sourceType, destinationType, out _);

        public static bool IsAssignableFrom(Type type, Type genericTypeInterface) =>
            Reflection.TypeInfo.ImplementsGenericTypeInterface(type, genericTypeInterface) ||
            (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == genericTypeInterface);

        public static bool IsCollection(Type type) =>
            type.IsArray || (!Reflection.TypeInfo.IsBuiltIn(type) && IsAssignableFrom(type, typeof(IEnumerable<>)));

        public static Type GetIEnumerableArgument(Type collectionType) =>
            collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(IEnumerable<>) ?
            collectionType.GenericTypeArguments[0] :
            Reflection.TypeInfo.GetGenericTypeInterface(collectionType, typeof(IEnumerable<>)).GenericTypeArguments[0];
    }
}
