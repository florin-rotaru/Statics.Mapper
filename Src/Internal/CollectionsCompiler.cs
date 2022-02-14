using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using static Statics.Mapper.Internal.Collections;

namespace Statics.Mapper.Internal
{
    internal partial class Compiler
    {
        private LocalBuilder LoopIndexLocal { get; set; }
        private LocalBuilder LoopLengthLocal { get; set; }
        private List<LocalBuilder> CollectionSourceLocals { get; set; } = new();
        private List<LocalBuilder> CollectionSourceLocalEnumerators { get; set; } = new();
        private List<LocalBuilder> CollectionDestinationLocals { get; set; } = new();
        private Dictionary<(Type, Type), LocalBuilder> CollectionMapperMapLocals { get; set; } = new Dictionary<(Type, Type), LocalBuilder>();

        private LocalBuilder GetOrAddLoopIndexLocal()
        {
            if (LoopIndexLocal != null)
                return LoopIndexLocal;

            LoopIndexLocal = IL.DeclareLocal(typeof(int));
            return LoopIndexLocal;
        }

        private LocalBuilder GetOrAddLoopLengthLocal()
        {
            if (LoopLengthLocal != null)
                return LoopLengthLocal;

            LoopLengthLocal = IL.DeclareLocal(typeof(int));
            return LoopLengthLocal;
        }

        private LocalBuilder GetOrAddCollectionSourceLocal(Type sourceType)
        {
            LocalBuilder sourceLocal = CollectionSourceLocals.FirstOrDefault(l => l.LocalType == sourceType);
            if (sourceLocal == null)
            {
                sourceLocal = IL.DeclareLocal(sourceType);
                CollectionSourceLocals.Add(sourceLocal);
            }

            return sourceLocal;
        }

        private static MethodInfo GetEnumerator(Type collectionType) =>
            collectionType.GetMethod(nameof(IEnumerable.GetEnumerator), Type.EmptyTypes) ??
                typeof(IEnumerable<>).MakeGenericType(GetIEnumerableArgument(collectionType))
                    .GetMethod(nameof(IEnumerable.GetEnumerator), Type.EmptyTypes);

        private LocalBuilder GetOrAddCollectionSourceLocalEnumerator(Type sourceType)
        {
            Type enumerator = GetEnumerator(sourceType).ReturnType;

            LocalBuilder sourceLocalEnumerator =
                CollectionSourceLocalEnumerators.FirstOrDefault(l => l.LocalType == enumerator);

            if (sourceLocalEnumerator == null)
            {
                sourceLocalEnumerator = IL.DeclareLocal(enumerator);
                CollectionSourceLocalEnumerators.Add(sourceLocalEnumerator);
            }

            return sourceLocalEnumerator;
        }

        private LocalBuilder GetOrAddCollectionDestinationLocal(Type destinationType)
        {
            LocalBuilder destinationLocal = CollectionDestinationLocals.FirstOrDefault(l => l.LocalType == destinationType);
            if (destinationLocal == null)
            {
                destinationLocal = IL.DeclareLocal(destinationType);
                CollectionDestinationLocals.Add(destinationLocal);
            }

            return destinationLocal;
        }

        private LocalBuilder GetOrAddCollectionMapperMapLocal(Type sourceType, Type destinationType)
        {
            if (!CollectionMapperMapLocals.TryGetValue((sourceType, destinationType), out LocalBuilder mapperLocal))
            {
                mapperLocal = IL.DeclareLocal(typeof(Func<,>).MakeGenericType(new[] { sourceType, destinationType }));
                CollectionMapperMapLocals.Add((sourceType, destinationType), mapperLocal);
            }

            return mapperLocal;
        }

        private void DeclareCollectionSourceLocals(CollectionInfo collectionInfo)
        {
            if (collectionInfo.UseArrayCopyTo)
            {
                collectionInfo.LoopLength = GetOrAddLoopLengthLocal();
            }
            else if (collectionInfo.SourceType.IsArray ||
               IsAssignableFrom(collectionInfo.SourceType, typeof(IList<>)) ||
               IsAssignableFrom(collectionInfo.SourceType, typeof(IReadOnlyList<>)))
            {
                if (collectionInfo.DestinationNodeMember != null)
                    collectionInfo.SourceLocal = GetOrAddCollectionSourceLocal(collectionInfo.SourceType);

                collectionInfo.LoopIndex = GetOrAddLoopIndexLocal();
                collectionInfo.LoopLength = GetOrAddLoopLengthLocal();
            }
            else if (
                IsAssignableFrom(collectionInfo.SourceType, typeof(ICollection<>)) ||
                IsAssignableFrom(collectionInfo.SourceType, typeof(IProducerConsumerCollection<>)) ||
                collectionInfo.SourceType.GetMethods().Any(m =>
                    m.Name == ToArray &&
                    m.GetParameters().Length == 0 &&
                    m.ReturnType == collectionInfo.SourceArgument.MakeArrayType()))
            {
                collectionInfo.SourceLocal = GetOrAddCollectionSourceLocal(collectionInfo.SourceArgument.MakeArrayType());

                collectionInfo.LoopIndex = GetOrAddLoopIndexLocal();
                collectionInfo.LoopLength = GetOrAddLoopLengthLocal();
            }
            else
            {
                collectionInfo.SourceLocalEnumerator = GetOrAddCollectionSourceLocalEnumerator(collectionInfo.SourceType);
            }
        }

        private void DeclareCollectionDestinationLocals(CollectionInfo collectionInfo)
        {
            TryGetCollectionLocalType(
                collectionInfo.SourceType,
                collectionInfo.DestinationType,
                out Type destinationLocalType);

            collectionInfo.DestinationLocal = GetOrAddCollectionDestinationLocal(destinationLocalType);
        }

        private void DeclareCollectionMapperMapLocal(CollectionInfo collectionInfo)
        {
            if (Reflection.TypeInfo.IsBuiltIn(collectionInfo.SourceArgument))
                return;

            collectionInfo.MapperMapMethodLocal = GetOrAddCollectionMapperMapLocal(collectionInfo.SourceArgument, collectionInfo.DestinationArgument);
        }

        private void DeclareCollectionLocals(CollectionInfo collectionInfo)
        {
            DeclareCollectionSourceLocals(collectionInfo);
            DeclareCollectionDestinationLocals(collectionInfo);
            DeclareCollectionMapperMapLocal(collectionInfo);
        }

        private void DeclareCollectionLocals(DestinationNode destinationNode)
        {
            if (destinationNode.UseMapper)
                return;

            foreach (DestinationNodeMember destinationNodeMember in destinationNode.Members)
            {
                if (destinationNodeMember.Map &&
                    destinationNodeMember.IsCollection)
                    DeclareCollectionLocals(new CollectionInfo(destinationNode, destinationNodeMember));
            }
        }

        private void SetCollectionSourceLocals(CollectionInfo collectionInfo)
        {
            if (collectionInfo.UseArrayCopyTo)
            {
                collectionInfo.LoopLength = GetOrAddLoopIndexLocal();

                if (collectionInfo.DestinationNodeMember != null)
                    Load(
                        collectionInfo.DestinationNodeMember.SourceNode,
                        collectionInfo.DestinationNodeMember.SourceNodeMember);
                else
                    IL.EmitLdarg(0);

                if (collectionInfo.SourceType.GetArrayRank() == 1)
                {
                    IL.Emit(OpCodes.Ldlen);
                    IL.Emit(OpCodes.Conv_I4);
                }
                else
                {
                    IL.Emit(OpCodes.Callvirt, collectionInfo.SourceType.GetProperty(nameof(Array.Length)).GetGetMethod());
                }

                IL.EmitStoreLocal(collectionInfo.LoopLength);
            }
            else if (collectionInfo.SourceType.IsArray ||
                IsAssignableFrom(collectionInfo.SourceType, typeof(IList<>)) ||
                IsAssignableFrom(collectionInfo.SourceType, typeof(IReadOnlyList<>)))
            {
                if (collectionInfo.DestinationNodeMember != null)
                    collectionInfo.SourceLocal = GetOrAddCollectionSourceLocal(collectionInfo.SourceType);

                collectionInfo.LoopIndex = GetOrAddLoopIndexLocal();
                collectionInfo.LoopLength = GetOrAddLoopLengthLocal();

                IL.EmitLdc_I4(0);
                IL.EmitStoreLocal(collectionInfo.LoopIndex);

                if (collectionInfo.DestinationNodeMember != null)
                {
                    Load(
                        collectionInfo.DestinationNodeMember.SourceNode,
                        collectionInfo.DestinationNodeMember.SourceNodeMember);
                    IL.EmitStoreLocal(collectionInfo.SourceLocal);
                    IL.EmitLoadLocal(collectionInfo.SourceLocal, false);
                }
                else
                {
                    IL.EmitLdarg(0);
                }

                if (collectionInfo.SourceType.IsArray)
                {
                    if (collectionInfo.SourceType.GetArrayRank() == 1)
                    {
                        IL.Emit(OpCodes.Ldlen);
                        IL.Emit(OpCodes.Conv_I4);
                    }
                    else
                    {
                        IL.Emit(OpCodes.Callvirt, collectionInfo.SourceType.GetProperty(nameof(Array.Length)).GetGetMethod());
                    }
                }
                else
                {
                    PropertyInfo propertyInfo = collectionInfo.SourceType.GetProperty(nameof(ICollection<Type>.Count));
                    if (propertyInfo != null)
                    {
                        IL.Emit(OpCodes.Callvirt, propertyInfo.GetGetMethod());
                    }
                    else if (IsAssignableFrom(collectionInfo.SourceType, typeof(IList<>)))
                    {
                        propertyInfo = typeof(ICollection<>)
                           .MakeGenericType(collectionInfo.SourceArgument)
                           .GetProperty(nameof(ICollection<Type>.Count));

                        IL.Emit(OpCodes.Box, collectionInfo.SourceType);
                        IL.Emit(OpCodes.Callvirt, propertyInfo.GetGetMethod());
                    }
                    else if (IsAssignableFrom(collectionInfo.SourceType, typeof(IReadOnlyList<>)))
                    {
                        propertyInfo = typeof(IReadOnlyCollection<>)
                            .MakeGenericType(collectionInfo.SourceArgument)
                            .GetProperty(nameof(IReadOnlyCollection<Type>.Count));

                        IL.Emit(OpCodes.Box, collectionInfo.SourceType);
                        IL.Emit(OpCodes.Callvirt, propertyInfo.GetGetMethod());
                    }
                }

                IL.EmitStoreLocal(collectionInfo.LoopLength);
            }
            else if (
                IsAssignableFrom(collectionInfo.SourceType, typeof(ICollection<>)) ||
                IsAssignableFrom(collectionInfo.SourceType, typeof(IProducerConsumerCollection<>)) ||
                collectionInfo.SourceType.GetMethods().Any(m =>
                    m.Name == ToArray &&
                    m.GetParameters().Length == 0 &&
                    m.ReturnType == collectionInfo.SourceArgument.MakeArrayType()))
            {
                collectionInfo.SourceLocal = GetOrAddCollectionSourceLocal(collectionInfo.SourceArgument.MakeArrayType());

                collectionInfo.LoopIndex = GetOrAddLoopIndexLocal();
                collectionInfo.LoopLength = GetOrAddLoopLengthLocal();

                IL.EmitLdc_I4(0);
                IL.EmitStoreLocal(collectionInfo.LoopIndex);

                if (collectionInfo.DestinationNodeMember != null)
                    Load(
                        collectionInfo.DestinationNodeMember.SourceNode,
                        collectionInfo.DestinationNodeMember.SourceNodeMember);
                else
                    IL.EmitLoadArgument(collectionInfo.SourceType, 0);

                MethodInfo methodInfo =
                    collectionInfo.SourceType.GetMethods().FirstOrDefault(m =>
                        m.Name == ToArray &&
                        m.GetParameters().Length == 0 &&
                        m.ReturnType == collectionInfo.SourceArgument.MakeArrayType());

                if (methodInfo != null)
                {
                    IL.EmitCallMethod(methodInfo);
                }
                else
                {
                    IL.EmitCallMethod(typeof(Reflection.Emit.ILGenerator.Converters)
                        .GetMethods()
                        .First(m =>
                            m.Name == ToArray &&
                            m.GetParameters().Length == 1 &&
                            m.GetParameters().Any(p =>
                                p.ParameterType.IsGenericType &&
                                p.ParameterType.GetGenericTypeDefinition() ==
                                    (IsAssignableFrom(collectionInfo.SourceType, typeof(ICollection<>)) ?
                                        typeof(ICollection<>) :
                                        typeof(IProducerConsumerCollection<>))))
                        .MakeGenericMethod(new Type[] { collectionInfo.SourceArgument }));
                }

                IL.EmitStoreLocal(collectionInfo.SourceLocal);

                IL.EmitLoadLocal(collectionInfo.SourceLocal, false);
                if (collectionInfo.SourceLocal.LocalType.GetArrayRank() == 1)
                {
                    IL.Emit(OpCodes.Ldlen);
                    IL.Emit(OpCodes.Conv_I4);
                }
                else
                {
                    IL.Emit(OpCodes.Callvirt, collectionInfo.SourceLocal.LocalType.GetProperty(nameof(Array.Length)).GetGetMethod());
                }
                IL.EmitStoreLocal(collectionInfo.LoopLength);
            }
            else
            {
                collectionInfo.SourceLocalEnumerator = GetOrAddCollectionSourceLocalEnumerator(collectionInfo.SourceType);

                if (collectionInfo.DestinationNodeMember != null)
                    Load(
                        collectionInfo.DestinationNodeMember.SourceNode,
                        collectionInfo.DestinationNodeMember.SourceNodeMember);
                else
                    IL.EmitLoadArgument(collectionInfo.SourceType, 0);

                IL.EmitCallMethod(GetEnumerator(collectionInfo.SourceType));
                IL.EmitStoreLocal(collectionInfo.SourceLocalEnumerator);
            }
        }

        private void SetCollectionDestinationLocals(CollectionInfo collectionInfo)
        {
            TryGetCollectionLocalType(
                collectionInfo.SourceType,
                collectionInfo.DestinationType,
                out Type destinationLocalType);

            collectionInfo.DestinationLocal = GetOrAddCollectionDestinationLocal(destinationLocalType);

            if (destinationLocalType.IsArray)
            {
                IL.EmitLoadLocal(collectionInfo.LoopLength, false);
                IL.Emit(OpCodes.Newarr, collectionInfo.DestinationArgument);
            }
            else
            {
                if (collectionInfo.LoopLength != null && destinationLocalType.GetConstructor(new[] { typeof(int) }) != null)
                {
                    IL.EmitLoadLocal(collectionInfo.LoopLength, false);
                    IL.Emit(OpCodes.Newobj, destinationLocalType.GetConstructor(new[] { typeof(int) }));
                }
                else
                {
                    IL.Emit(OpCodes.Newobj, destinationLocalType.GetConstructor(Type.EmptyTypes));
                }
            }

            IL.EmitStoreLocal(collectionInfo.DestinationLocal);
        }

        private void SetCollectionMapperMapLocal(CollectionInfo collectionInfo)
        {
            if (Reflection.TypeInfo.IsBuiltIn(collectionInfo.SourceArgument))
                return;

            collectionInfo.MapperMapMethodLocal = GetOrAddCollectionMapperMapLocal(collectionInfo.SourceArgument, collectionInfo.DestinationArgument);

            MethodInfo compiledMapperMethod = typeof(Mapper<,>)
                .MakeGenericType(new[] { collectionInfo.SourceArgument, collectionInfo.DestinationArgument })
                .GetProperty(nameof(Mapper<Type, Type>.CompiledFunc))
                .GetGetMethod();

            IL.Emit(OpCodes.Call, compiledMapperMethod);
            IL.EmitStoreLocal(collectionInfo.MapperMapMethodLocal);
        }

        private void SetCollectionLocals(CollectionInfo collectionInfo)
        {
            SetCollectionSourceLocals(collectionInfo);
            SetCollectionDestinationLocals(collectionInfo);
            SetCollectionMapperMapLocal(collectionInfo);
        }

        private void LoadCollectionElement(CollectionInfo collectionInfo)
        {
            if (collectionInfo.SourceLocalEnumerator == null)
            {
                if (collectionInfo.SourceLocal != null)
                    IL.EmitLoadLocal(collectionInfo.SourceLocal, false);
                else
                    IL.EmitLoadArgument(collectionInfo.SourceType, 0);


                IL.EmitLoadLocal(collectionInfo.LoopIndex, false);

                if (collectionInfo.SourceLocal != null ? collectionInfo.SourceLocal.LocalType.IsArray : SourceType.IsArray)
                {
                    IL.EmitLdelem(collectionInfo.SourceArgument);
                }
                else
                {
                    Type source = collectionInfo.SourceLocal != null ? collectionInfo.SourceLocal.LocalType : SourceType;
                    PropertyInfo propertyInfo = source.GetProperties().FirstOrDefault(p => p.GetIndexParameters().Length > 0);

                    if (propertyInfo == null && IsAssignableFrom(source, typeof(IList<>)))
                        propertyInfo = typeof(IList<>)
                            .MakeGenericType(collectionInfo.SourceArgument)
                            .GetProperties()
                            .First(p => p.GetIndexParameters().Length > 0);

                    if (propertyInfo == null && IsAssignableFrom(source, typeof(IReadOnlyList<>)))
                        propertyInfo = typeof(IReadOnlyList<>)
                            .MakeGenericType(collectionInfo.SourceArgument)
                            .GetProperties()
                            .First(p => p.GetIndexParameters().Length > 0);

                    if (propertyInfo == null)
                        throw new NotSupportedException($"{nameof(LoadCollectionElement)} {nameof(source)} {nameof(propertyInfo)} cannot be determined");

                    if (Reflection.TypeInfo.IsStatic(source) || source.IsValueType)
                        IL.Emit(OpCodes.Call, propertyInfo.GetGetMethod());
                    else
                        IL.EmitCallMethod(propertyInfo.GetGetMethod());
                }
            }
            else
            {
                IL.EmitLoadLocal(collectionInfo.SourceLocalEnumerator, collectionInfo.SourceLocalEnumerator.LocalType.IsValueType);
                IL.EmitCallMethod(collectionInfo.SourceLocalEnumerator.LocalType.GetProperty(nameof(IEnumerator.Current)).GetGetMethod());
            }
        }

        private void MapCollectionElement(CollectionInfo collectionInfo)
        {
            if (collectionInfo.MapperMapMethodLocal != null)
                IL.Emit(OpCodes.Callvirt, collectionInfo.MapperMapMethodLocal.LocalType.GetMethod(nameof(Func<Type>.Invoke)));
            else
                IL.EmitConvert(collectionInfo.SourceArgument, collectionInfo.DestinationArgument);
        }

        private void StoreCollectionElement(CollectionInfo collectionInfo)
        {
            if (collectionInfo.DestinationLocal.LocalType.IsArray)
            {
                IL.EmitStelem(collectionInfo.DestinationArgument);
                return;
            }

            IL.EmitCallMethod(GetCollectionAddMethod(collectionInfo.DestinationLocal.LocalType));
        }

        private void ConvertCollectionToArray(CollectionInfo collectionInfo)
        {
            Type destinationLocalType = collectionInfo.DestinationLocal.LocalType;
            MethodInfo methodInfo = destinationLocalType.GetMethods().FirstOrDefault(m =>
                m.Name == ToArray &&
                m.GetParameters().Length == 0 &&
                m.ReturnType == collectionInfo.DestinationArgument.MakeArrayType());

            if (methodInfo != null)
            {
                IL.Emit(OpCodes.Callvirt, methodInfo);
            }
            else if (
                IsAssignableFrom(destinationLocalType, typeof(ICollection<>)) ||
                IsAssignableFrom(destinationLocalType, typeof(IProducerConsumerCollection<>)))
            {
                IL.Emit(OpCodes.Call,
                    typeof(Reflection.Emit.ILGenerator.Converters)
                    .GetMethods()
                    .First(m =>
                        m.Name == ToArray &&
                        m.GetParameters().Length == 1 &&
                        m.GetParameters().Any(p =>
                            p.ParameterType.IsGenericType &&
                            p.ParameterType.GetGenericTypeDefinition() ==
                                (IsAssignableFrom(destinationLocalType, typeof(ICollection<>)) ?
                                    typeof(ICollection<>) :
                                    typeof(IProducerConsumerCollection<>))))
                    .MakeGenericMethod(new Type[] { collectionInfo.SourceArgument }));
            }
            else
            {
                IL.Emit(OpCodes.Call,
                    typeof(Enumerable).GetMethod(ToArray).MakeGenericMethod(new Type[] { collectionInfo.SourceArgument }));
            }
        }

        private static MethodInfo GetImmutableCreateRangeMethod(CollectionInfo collectionInfo)
        {
            MethodInfo method = ImmutableGenericCollectionBuilders
                .First(t => t.GenericTypeDefinition == collectionInfo.DestinationType.GetGenericTypeDefinition())
                .BuilderGenericTypeDefinition
                .GetMethods()
                .First(m =>
                    m.Name == CreateRange &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters().Any(p =>
                        p.ParameterType.IsGenericType &&
                        p.ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>)));

            if (collectionInfo.DestinationType.GenericTypeArguments.Length == collectionInfo.DestinationLocal.LocalType.GenericTypeArguments.Length)
                return method.MakeGenericMethod(collectionInfo.DestinationType.GenericTypeArguments);
            else
                return method.MakeGenericMethod(new Type[]
                {
                    collectionInfo.DestinationArgument.GenericTypeArguments[0],
                    collectionInfo.DestinationArgument.GenericTypeArguments[1]
                });
        }

        private void LoadImmutableEmpty(CollectionInfo collectionInfo) =>
            IL.Emit(OpCodes.Ldsfld, GetImmutableCreateRangeMethod(collectionInfo).ReturnType.GetField(nameof(ImmutableArray<Type>.Empty)));

        private void CallImmutableCreateRange(CollectionInfo collectionInfo) =>
            IL.Emit(OpCodes.Call, GetImmutableCreateRangeMethod(collectionInfo));

        private void StoreCollection(CollectionInfo collectionInfo)
        {
            if (collectionInfo.DestinationNodeMember != null)
            {
                if (!collectionInfo.DestinationNodeMember.Info.IsStatic)
                    Load(collectionInfo.DestinationNode);
            }
            else if (MethodType == MethodType.ActionRef)
            {
                IL.EmitLoadArgument(collectionInfo.DestinationType, 1, true);
            }

            IL.EmitLoadLocal(collectionInfo.DestinationLocal, false);

            if (collectionInfo.DestinationType == collectionInfo.DestinationLocal.LocalType)
            {
                if (collectionInfo.DestinationNodeMember != null)
                {
                    IL.EmitSetMemberValue(collectionInfo.DestinationNodeMember.Info);
                    return;
                }

                if (MethodType == MethodType.ActionRef)
                    IL.EmitStore(DestinationType);

                IL.Emit(OpCodes.Ret);

                return;
            }

            Type destinationGenericTypeDefinition = collectionInfo.DestinationType.IsGenericType ?
                collectionInfo.DestinationType.GetGenericTypeDefinition() :
                null;

            if (collectionInfo.DestinationType.IsInterface)
            {
                if (MaintainableGenericCollections
                    .Where(c => c.IsImmutable)
                    .Select(c => c.GenericTypeDefinition)
                    .Contains(destinationGenericTypeDefinition))
                {
                    CallImmutableCreateRange(collectionInfo);

                    if (collectionInfo.DestinationNodeMember != null)
                    {
                        IL.EmitSetMemberValue(collectionInfo.DestinationNodeMember.Info);
                        return;
                    }

                    if (MethodType == MethodType.ActionRef)
                        IL.EmitStore(DestinationType);

                    IL.Emit(OpCodes.Ret);

                }
                else
                {
                    if (collectionInfo.DestinationNodeMember != null)
                    {
                        IL.EmitSetMemberValue(collectionInfo.DestinationNodeMember.Info);
                        return;
                    }

                    if (MethodType == MethodType.ActionRef)
                        IL.EmitStore(DestinationType);

                    IL.Emit(OpCodes.Ret);

                }
            }
            else if (collectionInfo.DestinationType.IsArray)
            {
                ConvertCollectionToArray(collectionInfo);

                if (collectionInfo.DestinationNodeMember != null)
                {
                    IL.EmitSetMemberValue(collectionInfo.DestinationNodeMember.Info);
                    return;
                }

                if (MethodType == MethodType.ActionRef)
                    IL.EmitStore(DestinationType);

                IL.Emit(OpCodes.Ret);

            }
            else if (MaintainableGenericCollections
                    .Where(c => c.IsImmutable)
                    .Select(c => c.GenericTypeDefinition)
                    .Contains(destinationGenericTypeDefinition))
            {
                CallImmutableCreateRange(collectionInfo);

                if (collectionInfo.DestinationNodeMember != null)
                {
                    IL.EmitSetMemberValue(collectionInfo.DestinationNodeMember.Info);
                    return;
                }

                if (MethodType == MethodType.ActionRef)
                    IL.EmitStore(DestinationType);

                IL.Emit(OpCodes.Ret);

            }
            else if (collectionInfo.DestinationType.GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(new Type[] { collectionInfo.DestinationArgument }) }) != null)
            {
                if (collectionInfo.DestinationNodeMember != null)
                {
                    IL.Emit(OpCodes.Newobj, collectionInfo.DestinationType.GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(new Type[] { collectionInfo.DestinationArgument }) }));
                    IL.EmitSetMemberValue(collectionInfo.DestinationNodeMember.Info);

                    return;
                }

                if (MethodType == MethodType.ActionRef)
                    IL.EmitStore(DestinationType);

                IL.Emit(OpCodes.Ret);
            }
        }

        private void MapCollectionArrayCopyTo(CollectionInfo collectionInfo)
        {
            if (collectionInfo.DestinationNodeMember != null)
                Load(
                    collectionInfo.DestinationNodeMember.SourceNode,
                    collectionInfo.DestinationNodeMember.SourceNodeMember);
            else
                IL.EmitLdarg(0);

            IL.EmitLdc_I4(0);
            IL.EmitLoadLocal(collectionInfo.DestinationLocal, false);
            IL.EmitLdc_I4(0);
            IL.EmitLoadLocal(collectionInfo.LoopLength, false);
            IL.EmitCallMethod(typeof(Array).GetMethod(nameof(Array.Copy), new Type[]
                {
                    typeof(Array),
                    typeof(int),
                    typeof(Array),
                    typeof(int),
                    typeof(int)
                }));

            StoreCollection(collectionInfo);
        }

        private void MapCollectionLoopForEach(CollectionInfo collectionInfo)
        {
            Label loopMoveNext = IL.DefineLabel();
            Label loopBody = IL.DefineLabel();

            IL.Emit(OpCodes.Br_S, loopMoveNext);
            IL.MarkLabel(loopBody);

            IL.EmitLoadLocal(collectionInfo.DestinationLocal, false);
            if (collectionInfo.MapperMapMethodLocal != null)
                IL.EmitLoadLocal(collectionInfo.MapperMapMethodLocal, false);

            LoadCollectionElement(collectionInfo);
            MapCollectionElement(collectionInfo);
            StoreCollectionElement(collectionInfo);

            IL.MarkLabel(loopMoveNext);

            IL.EmitLoadLocal(collectionInfo.SourceLocalEnumerator, collectionInfo.SourceLocalEnumerator.LocalType.IsValueType);

            IL.EmitCallMethod(
                collectionInfo.SourceLocalEnumerator.LocalType.GetMethod(nameof(IEnumerator.MoveNext)) ??
                typeof(IEnumerator).GetMethod(nameof(IEnumerator.MoveNext)));

            IL.Emit(OpCodes.Brtrue_S, loopBody);
            StoreCollection(collectionInfo);
        }

        private void MapCollectionLoopIndex(CollectionInfo collectionInfo)
        {
            Label loopCompareIndex = IL.DefineLabel();
            Label loopBody = IL.DefineLabel();

            IL.Emit(OpCodes.Br_S, loopCompareIndex);
            IL.MarkLabel(loopBody);

            IL.EmitLoadLocal(collectionInfo.DestinationLocal, false);
            if (collectionInfo.DestinationLocal.LocalType.IsArray)
                IL.EmitLoadLocal(collectionInfo.LoopIndex, false);

            if (collectionInfo.MapperMapMethodLocal != null)
                IL.EmitLoadLocal(collectionInfo.MapperMapMethodLocal, false);

            LoadCollectionElement(collectionInfo);
            MapCollectionElement(collectionInfo);
            StoreCollectionElement(collectionInfo);

            IL.EmitLoadLocal(collectionInfo.LoopIndex, false);
            IL.EmitLdc_I4(1);
            IL.Emit(OpCodes.Add);
            IL.EmitStoreLocal(collectionInfo.LoopIndex);

            IL.MarkLabel(loopCompareIndex);

            IL.EmitLoadLocal(collectionInfo.LoopIndex, false);
            IL.EmitLoadLocal(collectionInfo.LoopLength, false);

            IL.Emit(OpCodes.Blt_S, loopBody);
            StoreCollection(collectionInfo);
        }

        private void MapCollection(
            DestinationNode destinationNode,
            DestinationNodeMember destinationNodeMember)
        {
            CollectionInfo collectionInfo = new(destinationNode, destinationNodeMember);

            Load(
                collectionInfo.DestinationNodeMember.SourceNode,
                collectionInfo.DestinationNodeMember.SourceNodeMember);

            if (collectionInfo.DestinationNodeMember.SourceNodeMember.Type.IsValueType)
                IL.Emit(OpCodes.Box, collectionInfo.DestinationNodeMember.SourceNodeMember.Type);

            IL.EmitBrfalse_s(
                () =>
                {
                    SetCollectionLocals(collectionInfo);

                    if (collectionInfo.UseArrayCopyTo)
                        MapCollectionArrayCopyTo(collectionInfo);
                    else if (collectionInfo.LoopLength != null)
                        MapCollectionLoopIndex(collectionInfo);
                    else
                        MapCollectionLoopForEach(collectionInfo);
                });
        }

        public void MapCollection(
            Type sourceType,
            Type destinationType)
        {
            CollectionInfo collectionInfo = new(sourceType, destinationType);

            IL.EmitLdarg(0);
            if (sourceType.IsValueType)
                IL.Emit(OpCodes.Box, sourceType);
            IL.EmitBrfalse_s(
                () =>
                {
                    SetCollectionLocals(collectionInfo);

                    if (collectionInfo.UseArrayCopyTo)
                        MapCollectionArrayCopyTo(collectionInfo);
                    else if (collectionInfo.LoopLength != null)
                        MapCollectionLoopIndex(collectionInfo);
                    else
                        MapCollectionLoopForEach(collectionInfo);
                });

            if (MethodType == MethodType.Function)
            {
                if (!DestinationType.IsValueType)
                    IL.Emit(OpCodes.Ldnull);
                else if (IsImmutable(destinationType.GetGenericTypeDefinition()))
                    LoadImmutableEmpty(collectionInfo);
                else
                    IL.EmitInit(DestinationType);
            }
            else
            {
                IL.EmitLoadArgument(DestinationType, 1, true);

                if (!DestinationType.IsValueType)
                    IL.Emit(OpCodes.Ldnull);
                else if (IsImmutable(destinationType.GetGenericTypeDefinition()))
                    LoadImmutableEmpty(collectionInfo);
                else
                    IL.EmitInit(DestinationType);

                IL.EmitStore(DestinationType);
            }

            IL.Emit(OpCodes.Ret);
        }
    }
}
