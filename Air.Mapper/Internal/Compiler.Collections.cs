using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using static Air.Mapper.Internal.Collections;


namespace Air.Mapper.Internal
{
    internal partial class Compiler
    {
        private LocalBuilder LoopIndexLocal { get; set; }
        private LocalBuilder LoopLengthLocal { get; set; }
        private List<LocalBuilder> LoopSourceLocals { get; set; } = new List<LocalBuilder>();
        private List<LocalBuilder> LoopSourceLocalEnumerators { get; set; } = new List<LocalBuilder>();
        private List<LocalBuilder> LoopDestinationLocals { get; set; } = new List<LocalBuilder>();
        private Dictionary<(Type, Type), LocalBuilder> LoopMapperMapLocals { get; set; } = new Dictionary<(Type, Type), LocalBuilder>();

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

        private LocalBuilder GetOrAddLoopSourceLocal(Type sourceType)
        {
            LocalBuilder sourceLocal = LoopSourceLocals.FirstOrDefault(l => l.LocalType == sourceType);
            if (sourceLocal == null)
            {
                sourceLocal = IL.DeclareLocal(sourceType);
                LoopSourceLocals.Add(sourceLocal);
            }

            return sourceLocal;
        }

        private LocalBuilder GetOrAddLoopSourceLocalEnumerator(Type sourceArgument)
        {
            LocalBuilder sourceLocalEnumerator =
                LoopSourceLocalEnumerators.FirstOrDefault(l => l.LocalType == typeof(IEnumerator<>).MakeGenericType(new Type[] { sourceArgument }));

            if (sourceLocalEnumerator == null)
            {
                sourceLocalEnumerator = IL.DeclareLocal(typeof(IEnumerator<>).MakeGenericType(new Type[] { sourceArgument }));
                LoopSourceLocalEnumerators.Add(sourceLocalEnumerator);
            }

            return sourceLocalEnumerator;
        }

        private LocalBuilder GetOrAddLoopDestinationLocal(Type destinationType)
        {
            LocalBuilder destinationLocal = LoopDestinationLocals.FirstOrDefault(l => l.LocalType == destinationType);
            if (destinationLocal == null)
            {
                destinationLocal = IL.DeclareLocal(destinationType);
                LoopDestinationLocals.Add(destinationLocal);
            }

            return destinationLocal;
        }

        private LocalBuilder GetOrAddLoopMapperMapLocal(Type sourceType, Type destinationType)
        {
            if (!LoopMapperMapLocals.TryGetValue((sourceType, destinationType), out LocalBuilder mapperLocal))
            {
                mapperLocal = IL.DeclareLocal(typeof(Func<,>).MakeGenericType(new[] { sourceType, destinationType }));
                LoopMapperMapLocals.Add((sourceType, destinationType), mapperLocal);
            }

            return mapperLocal;
        }

        private void DeclareCollectionSourceLocals(CollectionInfo collectionInfo)
        {
            Type sourceType = collectionInfo.SourceType;

            if (sourceType.IsArray ||
               IsAssignableFrom(sourceType, typeof(IList<>)) ||
               IsAssignableFrom(sourceType, typeof(IReadOnlyList<>)))
            {
                collectionInfo.SourceLocal = GetOrAddLoopSourceLocal(sourceType);

                collectionInfo.LoopIndex = GetOrAddLoopIndexLocal();
                collectionInfo.LoopLength = GetOrAddLoopLengthLocal();
            }
            else if (IsAssignableFrom(sourceType, typeof(ICollection<>)) ||
                    IsAssignableFrom(sourceType, typeof(IProducerConsumerCollection<>)))
            {
                collectionInfo.SourceLocal = GetOrAddLoopSourceLocal(collectionInfo.SourceArgument.MakeArrayType());

                collectionInfo.LoopIndex = GetOrAddLoopIndexLocal();
                collectionInfo.LoopLength = GetOrAddLoopLengthLocal();
            }
            else
            {
                collectionInfo.SourceLocalEnumerator = GetOrAddLoopSourceLocalEnumerator(collectionInfo.SourceArgument);
            }
        }

        private void SetCollectionInfoDestinationLocal(
            CollectionInfo collectionInfo,
            Type destinationType,
            Collection maintainableGenericCollection)
        {
            collectionInfo.DestinationLocal =
                maintainableGenericCollection.LocalGenericTypeDefinition.GetGenericArguments().Length == destinationType.GetGenericArguments().Length ?
                GetOrAddLoopDestinationLocal(maintainableGenericCollection.MakeLocalGenericType(destinationType.GenericTypeArguments)) :
                GetOrAddLoopDestinationLocal(maintainableGenericCollection.MakeLocalGenericType(collectionInfo.DestinationArgument));
        }

        private void DeclareCollectionDestinationLocals(CollectionInfo collectionInfo)
        {
            Type destinationType = collectionInfo.DestinationType;
            Type destinationGenericTypeDefinition = destinationType.IsGenericType ?
                destinationType.GetGenericTypeDefinition() :
                null;
            Type intersectType;

            if (destinationType.IsInterface)
            {
                if (destinationGenericTypeDefinition == typeof(IEnumerable<>))
                {
                    if (collectionInfo.LoopLength != null)
                        collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(collectionInfo.DestinationArgument.MakeArrayType());
                    else
                        collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(typeof(List<>).MakeGenericType(new[] { collectionInfo.DestinationArgument }));
                }
                else
                {
                    SetCollectionInfoDestinationLocal(
                        collectionInfo,
                        destinationType,
                        MaintainableGenericCollections.First(t => t.GenericTypeDefinition == destinationGenericTypeDefinition));
                }

                return;
            }

            bool isArrayOrHasDefaultConstructor = destinationType.IsArray || destinationType.GetConstructor(Type.EmptyTypes) != null;

            if (destinationType.IsArray)
            {
                if (collectionInfo.LoopLength != null)
                    collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(destinationType);
                else
                    collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(typeof(List<>).MakeGenericType(new[] { collectionInfo.DestinationArgument }));
            }
            else if (
                isArrayOrHasDefaultConstructor &&
                destinationGenericTypeDefinition != null &&
                MaintainableGenericCollections.Any(t => t.GenericTypeDefinition == destinationGenericTypeDefinition))
            {
                SetCollectionInfoDestinationLocal(
                    collectionInfo,
                    destinationType,
                    MaintainableGenericCollections.First(t => t.GenericTypeDefinition == destinationGenericTypeDefinition));
            }
            else if (
                isArrayOrHasDefaultConstructor &&
                MaintainableGenericCollections.Select(s => s.GenericTypeDefinition)
                    .Intersect(GetGenericTypeDefinitionBaseTypes(destinationType))
                    .Any())
            {
                intersectType = MaintainableGenericCollections.Select(s => s.GenericTypeDefinition)
                    .Intersect(GetGenericTypeDefinitionBaseTypes(destinationType))
                    .FirstOrDefault();

                SetCollectionInfoDestinationLocal(
                    collectionInfo,
                    intersectType,
                    MaintainableGenericCollections.First(t => t.GenericTypeDefinition == intersectType.GetGenericTypeDefinition()));
            }
            else if (
                isArrayOrHasDefaultConstructor &&
                MaintainableGenericCollections.Select(s => s.GenericTypeDefinition)
                    .Intersect(destinationType.GetInterfaces().Where(ci => ci.IsGenericType).Select(ci => ci.GetGenericTypeDefinition()))
                    .Any())
            {
                intersectType = MaintainableGenericCollections.Select(s => s.GenericTypeDefinition)
                    .Intersect(destinationType.GetInterfaces().Where(ci => ci.IsGenericType).Select(ci => ci.GetGenericTypeDefinition()))
                    .FirstOrDefault();

                SetCollectionInfoDestinationLocal(
                    collectionInfo,
                    intersectType,
                    MaintainableGenericCollections.First(t => t.GenericTypeDefinition == intersectType.GetGenericTypeDefinition()));
            }
            else if (destinationType.GetConstructors().Any(c =>
                c.GetParameters().Length == 1 &&
                c.GetParameters()[0].ParameterType == typeof(IEnumerable<>).MakeGenericType(new[] { collectionInfo.DestinationArgument })))
            {
                collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(typeof(List<>).MakeGenericType(new[] { collectionInfo.DestinationArgument }));
            }
        }

        private void DeclareCollectionMapperMapLocal(CollectionInfo collectionInfo)
        {
            if (Reflection.TypeInfo.IsBuiltIn(collectionInfo.SourceArgument))
                return;

            collectionInfo.MapperMapMethodLocal = GetOrAddLoopMapperMapLocal(collectionInfo.SourceArgument, collectionInfo.DestinationArgument);
        }

        private void DeclareCollectionLocals(DestinationNode destinationNode)
        {
            foreach (DestinationNodeMember destinationNodeMember in destinationNode.Members)
            {
                if (destinationNodeMember.Map &&
                    destinationNodeMember.IsCollection)
                {
                    CollectionInfo collectionInfo = new CollectionInfo(destinationNode, destinationNodeMember);
                    DeclareCollectionSourceLocals(collectionInfo);
                    DeclareCollectionDestinationLocals(collectionInfo);
                    DeclareCollectionMapperMapLocal(collectionInfo);
                }
            }
        }

        private void SetCollectionSourceLocals(CollectionInfo collectionInfo)
        {
            Type sourceType = collectionInfo.SourceType;

            if (sourceType.IsArray ||
                IsAssignableFrom(sourceType, typeof(IList<>)) ||
                IsAssignableFrom(sourceType, typeof(IReadOnlyList<>)))
            {
                collectionInfo.SourceLocal = GetOrAddLoopSourceLocal(sourceType);

                collectionInfo.LoopIndex = GetOrAddLoopIndexLocal();
                collectionInfo.LoopLength = GetOrAddLoopLengthLocal();

                IL.EmitLdc_I4(0);
                IL.EmitStoreLocal(collectionInfo.LoopIndex);

                Load(
                    collectionInfo.DestinationNodeMember.SourceNode,
                    collectionInfo.DestinationNodeMember.SourceNodeMember);
                IL.EmitStoreLocal(collectionInfo.SourceLocal);

                IL.EmitLoadLocal(collectionInfo.SourceLocal, false);
                if (sourceType.IsArray)
                {
                    if (sourceType.GetArrayRank() == 1)
                    {
                        IL.Emit(OpCodes.Ldlen);
                        IL.Emit(OpCodes.Conv_I4);
                    }
                    else
                    {
                        IL.Emit(OpCodes.Callvirt, sourceType.GetProperty(nameof(Array.Length)).GetGetMethod());
                    }
                }
                else
                {
                    IL.Emit(OpCodes.Callvirt, sourceType.GetProperty(nameof(ICollection<Type>.Count)).GetGetMethod());
                }

                IL.EmitStoreLocal(collectionInfo.LoopLength);
            }
            else if (IsAssignableFrom(sourceType, typeof(ICollection<>)) ||
                    IsAssignableFrom(sourceType, typeof(IProducerConsumerCollection<>)))
            {
                collectionInfo.SourceLocal = GetOrAddLoopSourceLocal(collectionInfo.SourceArgument.MakeArrayType());

                collectionInfo.LoopIndex = GetOrAddLoopIndexLocal();
                collectionInfo.LoopLength = GetOrAddLoopLengthLocal();

                IL.EmitLdc_I4(0);
                IL.EmitStoreLocal(collectionInfo.LoopIndex);

                Load(
                    collectionInfo.DestinationNodeMember.SourceNode,
                    collectionInfo.DestinationNodeMember.SourceNodeMember);

                if (IsAssignableFrom(sourceType, typeof(ICollection<>)))
                    IL.Emit(OpCodes.Callvirt,
                        typeof(ICollection<>)
                        .MakeGenericType(new[] { collectionInfo.SourceArgument })
                        .GetProperty(nameof(ICollection<Type>.Count))
                        .GetGetMethod());
                else
                    IL.Emit(OpCodes.Callvirt,
                        typeof(IProducerConsumerCollection<>)
                        .MakeGenericType(new[] { collectionInfo.SourceArgument })
                        .GetProperty(nameof(IProducerConsumerCollection<Type>.Count))
                        .GetGetMethod());

                IL.EmitStoreLocal(collectionInfo.LoopLength);

                IL.EmitLoadLocal(collectionInfo.LoopLength, false);
                IL.Emit(OpCodes.Newarr, collectionInfo.SourceLocal.LocalType);
                IL.EmitStoreLocal(collectionInfo.SourceLocal);

                Load(
                    collectionInfo.DestinationNodeMember.SourceNode,
                    collectionInfo.DestinationNodeMember.SourceNodeMember);
                IL.EmitLoadLocal(collectionInfo.SourceLocal, false);
                IL.EmitLdc_I4(0);

                MethodInfo methodInfo = IsAssignableFrom(sourceType, typeof(ICollection<>))
                    ? typeof(ICollection<>).MakeGenericType(new Type[] { collectionInfo.DestinationArgument }).GetMethods().FirstOrDefault(m =>
                        m.Name == nameof(ICollection<Type>.CopyTo) &&
                        m.GetParameters().Length == 2 &&
                        m.GetParameters()[0].ParameterType == collectionInfo.DestinationArgument.MakeArrayType())
                    : typeof(IProducerConsumerCollection<>).MakeGenericType(new Type[] { collectionInfo.DestinationArgument }).GetMethods().FirstOrDefault(m =>
                        m.Name == nameof(IProducerConsumerCollection<Type>.CopyTo) &&
                        m.GetParameters().Length == 2 &&
                        m.GetParameters()[0].ParameterType == collectionInfo.DestinationArgument.MakeArrayType());

                IL.Emit(OpCodes.Callvirt, methodInfo);
            }
            else
            {
                collectionInfo.SourceLocalEnumerator = GetOrAddLoopSourceLocalEnumerator(collectionInfo.SourceArgument);

                Load(
                    collectionInfo.DestinationNodeMember.SourceNode,
                    collectionInfo.DestinationNodeMember.SourceNodeMember);
                IL.Emit(OpCodes.Callvirt, collectionInfo.SourceType.GetMethod(nameof(IEnumerable.GetEnumerator), Type.EmptyTypes));
                IL.EmitStoreLocal(collectionInfo.SourceLocalEnumerator);
            }
        }

        private void SetCollectionDestinationLocal(
            CollectionInfo collectionInfo,
            Type destinationType,
            Collection maintainableGenericCollection)
        {
            SetCollectionInfoDestinationLocal(
                collectionInfo,
                destinationType,
                maintainableGenericCollection);

            Type localType = collectionInfo.DestinationLocal.LocalType;

            if (collectionInfo.LoopLength != null && localType.GetConstructor(new[] { typeof(int) }) != null)
            {
                IL.EmitLoadLocal(collectionInfo.LoopLength, false);
                IL.Emit(OpCodes.Newobj, localType.GetConstructor(new[] { typeof(int) }));
            }
            else
            {
                IL.Emit(OpCodes.Newobj, localType.GetConstructor(Type.EmptyTypes));
            }

            IL.EmitStoreLocal(collectionInfo.DestinationLocal);
        }

        private void SetCollectionDestinationLocals(CollectionInfo collectionInfo)
        {
            Type destinationType = collectionInfo.DestinationType;
            Type destinationGenericTypeDefinition = destinationType.IsGenericType ?
                destinationType.GetGenericTypeDefinition() :
                null;
            Type intersectType;

            if (destinationType.IsInterface)
            {
                if (destinationGenericTypeDefinition == typeof(IEnumerable<>))
                {
                    if (collectionInfo.LoopLength != null)
                    {
                        collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(collectionInfo.DestinationArgument.MakeArrayType());
                        IL.EmitLoadLocal(collectionInfo.LoopLength, false);
                        IL.Emit(OpCodes.Newarr, collectionInfo.DestinationArgument);
                    }
                    else
                    {
                        collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(typeof(List<>).MakeGenericType(new[] { collectionInfo.DestinationArgument }));
                        
                        IL.Emit(OpCodes.Newobj, collectionInfo.DestinationLocal.LocalType.GetConstructor(Type.EmptyTypes));
                    }
                }
                else
                {
                    SetCollectionInfoDestinationLocal(
                       collectionInfo,
                       destinationType,
                       MaintainableGenericCollections.First(t => t.GenericTypeDefinition == destinationGenericTypeDefinition));

                    IL.Emit(OpCodes.Newobj, collectionInfo.DestinationLocal.LocalType.GetConstructor(Type.EmptyTypes));
                }

                IL.EmitStoreLocal(collectionInfo.DestinationLocal);

                return;
            }

            bool isArrayOrHasDefaultConstructor = destinationType.IsArray || destinationType.GetConstructor(Type.EmptyTypes) != null;

            if (destinationType.IsArray)
            {
                if (collectionInfo.LoopLength != null)
                {
                    collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(destinationType);
                    IL.EmitLoadLocal(collectionInfo.LoopLength, false);
                    IL.Emit(OpCodes.Newarr, collectionInfo.DestinationArgument);
                }
                else
                {
                    collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(typeof(List<>).MakeGenericType(new[] { collectionInfo.DestinationArgument }));
                    IL.Emit(OpCodes.Newobj, collectionInfo.DestinationLocal.LocalType.GetConstructor(Type.EmptyTypes));
                }

                IL.EmitStoreLocal(collectionInfo.DestinationLocal);
            }
            else if (
                isArrayOrHasDefaultConstructor &&
                destinationGenericTypeDefinition != null &&
                MaintainableGenericCollections.Any(t => t.GenericTypeDefinition == destinationGenericTypeDefinition))
            {
                SetCollectionDestinationLocal(
                    collectionInfo,
                    destinationType,
                    MaintainableGenericCollections.First(t => t.GenericTypeDefinition == destinationGenericTypeDefinition));
            }
            else if (
                isArrayOrHasDefaultConstructor &&
                MaintainableGenericCollections.Select(s => s.GenericTypeDefinition)
                    .Intersect(GetGenericTypeDefinitionBaseTypes(destinationType))
                    .Any())
            {
                intersectType = MaintainableGenericCollections.Select(s => s.GenericTypeDefinition)
                    .Intersect(GetGenericTypeDefinitionBaseTypes(destinationType))
                    .FirstOrDefault();

                SetCollectionDestinationLocal(
                    collectionInfo,
                    intersectType,
                    MaintainableGenericCollections.First(t => t.GenericTypeDefinition == intersectType.GetGenericTypeDefinition()));

            }
            else if (
                isArrayOrHasDefaultConstructor &&
                MaintainableGenericCollections.Select(s => s.GenericTypeDefinition)
                    .Intersect(destinationType.GetInterfaces().Where(ci => ci.IsGenericType).Select(ci => ci.GetGenericTypeDefinition()))
                    .Any())
            {
                intersectType = MaintainableGenericCollections.Select(s => s.GenericTypeDefinition)
                    .Intersect(destinationType.GetInterfaces().Where(ci => ci.IsGenericType).Select(ci => ci.GetGenericTypeDefinition()))
                    .FirstOrDefault();

                SetCollectionInfoDestinationLocal(
                    collectionInfo,
                    intersectType,
                    MaintainableGenericCollections.First(t => t.GenericTypeDefinition == intersectType.GetGenericTypeDefinition()));
            }
            else if (destinationType.GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(new[] { collectionInfo.DestinationArgument }) }) != null)
            {
                collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(typeof(List<>).MakeGenericType(new[] { collectionInfo.DestinationArgument }));
                IL.Emit(OpCodes.Newobj, collectionInfo.DestinationLocal.LocalType.GetConstructor(Type.EmptyTypes));
                IL.EmitStoreLocal(collectionInfo.DestinationLocal);
            }
        }

        private void SetCollectionMapperMapLocal(CollectionInfo collectionInfo)
        {
            if (Reflection.TypeInfo.IsBuiltIn(collectionInfo.SourceArgument))
                return;

            collectionInfo.MapperMapMethodLocal = GetOrAddLoopMapperMapLocal(collectionInfo.SourceArgument, collectionInfo.DestinationArgument);

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
                IL.EmitLoadLocal(collectionInfo.SourceLocal, false);
                IL.EmitLoadLocal(collectionInfo.LoopIndex, false);

                if (collectionInfo.SourceLocal.LocalType.IsArray)
                    IL.Emit(OpCodes.Ldelem_Ref);
                else
                    IL.Emit(OpCodes.Callvirt, collectionInfo.SourceLocal.LocalType.GetProperties().First(p => p.GetIndexParameters().Length > 0).GetGetMethod());
            }
            else
            {
                IL.EmitLoadLocal(collectionInfo.SourceLocalEnumerator, false);
                IL.Emit(OpCodes.Callvirt, collectionInfo.SourceLocalEnumerator.LocalType.GetProperty(nameof(IEnumerator.Current)).GetGetMethod());
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
                IL.Emit(OpCodes.Stelem_Ref);
            else
                IL.Emit(OpCodes.Callvirt, typeof(ICollection<>).MakeGenericType(new[] { collectionInfo.DestinationArgument }).GetMethod(nameof(ICollection<Type>.Add)));
        }

        private void ConvertCollectionToArray(CollectionInfo collectionInfo)
        {
            MethodInfo methodInfo = collectionInfo.DestinationLocal.LocalType.GetMethods().FirstOrDefault(m =>
                m.Name == nameof(List<Type>.ToArray) &&
                m.GetParameters().Length == 0 &&
                m.ReturnType == collectionInfo.DestinationArgument.MakeArrayType());

            if (methodInfo != null)
            {
                if (!collectionInfo.DestinationNodeMember.Info.IsStatic)
                    Load(collectionInfo.DestinationNode);

                IL.EmitLoadLocal(collectionInfo.DestinationLocal, false);
                IL.Emit(OpCodes.Callvirt, methodInfo);
                IL.EmitSetMemberValue(collectionInfo.DestinationNodeMember.Info);
                return;
            }

            methodInfo = typeof(ICollection<>).MakeGenericType(new Type[] { collectionInfo.DestinationArgument }).GetMethods().FirstOrDefault(m =>
                m.Name == nameof(ICollection<Type>.CopyTo) &&
                m.GetParameters().Length == 2 &&
                m.GetParameters()[0].ParameterType == collectionInfo.DestinationArgument.MakeArrayType());

            if (!collectionInfo.DestinationNodeMember.Info.IsStatic)
                Load(collectionInfo.DestinationNode);
            IL.EmitLoadLocal(collectionInfo.DestinationLocal, false);
            IL.Emit(OpCodes.Callvirt, collectionInfo.DestinationLocal.LocalType.GetProperty(nameof(ICollection.Count)).GetGetMethod());
            IL.Emit(OpCodes.Newarr, collectionInfo.DestinationArgument);
            IL.EmitSetMemberValue(collectionInfo.DestinationNodeMember.Info);


            IL.EmitLoadLocal(collectionInfo.DestinationLocal, false);
            if (!collectionInfo.DestinationNodeMember.Info.IsStatic)
                Load(collectionInfo.DestinationNode);
            IL.EmitLoadMemberValue(collectionInfo.DestinationNodeMember.Info);
            IL.EmitLdc_I4(0);
            IL.Emit(OpCodes.Callvirt, methodInfo);
        }

        private void StoreCollection(CollectionInfo collectionInfo)
        {
            if (collectionInfo.DestinationType == collectionInfo.DestinationLocal.LocalType)
            {
                if (!collectionInfo.DestinationNodeMember.Info.IsStatic)
                    Load(collectionInfo.DestinationNode);

                IL.EmitLoadLocal(collectionInfo.DestinationLocal, false);
                IL.EmitSetMemberValue(collectionInfo.DestinationNodeMember.Info);

                return;
            }

            Type destinationGenericTypeDefinition = collectionInfo.DestinationType.IsGenericType ?
                collectionInfo.DestinationType.GetGenericTypeDefinition() :
                null;

            bool isImmutable =
                MaintainableGenericCollections.Where(c => c.IsImmutable).Select(s => s.GenericTypeDefinition)
                    .Intersect(GetGenericTypeDefinitionBaseTypes(collectionInfo.DestinationType))
                    .Any() ||
                MaintainableGenericCollections.Where(c => c.IsImmutable).Select(s => s.GenericTypeDefinition)
                    .Intersect(collectionInfo.DestinationType.GetInterfaces().Where(ci => ci.IsGenericType).Select(ci => ci.GetGenericTypeDefinition()))
                    .Any();


            if (collectionInfo.DestinationType.IsInterface)
            {
                if (isImmutable)
                {

                }
                else
                {

                }
            }
            else if (collectionInfo.DestinationType.IsArray)
            {
                ConvertCollectionToArray(collectionInfo);
            }
            else if (collectionInfo.DestinationType.GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(new Type[] { collectionInfo.DestinationArgument }) }) != null)
            {
                if (!collectionInfo.DestinationNodeMember.Info.IsStatic)
                    Load(collectionInfo.DestinationNode);

                IL.EmitLoadLocal(collectionInfo.DestinationLocal, false);
                IL.Emit(OpCodes.Newobj, collectionInfo.DestinationType.GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(new Type[] { collectionInfo.DestinationArgument }) }));
                IL.EmitSetMemberValue(collectionInfo.DestinationNodeMember.Info);
            }
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

            IL.EmitLoadLocal(collectionInfo.SourceLocalEnumerator, false);
            IL.Emit(OpCodes.Callvirt, typeof(IEnumerator).GetMethod(nameof(IEnumerator.MoveNext)));

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
            CollectionInfo collectionInfo = new CollectionInfo(destinationNode, destinationNodeMember);

            Load(
                collectionInfo.DestinationNodeMember.SourceNode,
                collectionInfo.DestinationNodeMember.SourceNodeMember);

            IL.EmitBrfalse_s(
                () =>
                {
                    SetCollectionLocals(collectionInfo);

                    if (collectionInfo.LoopLength != null)
                        MapCollectionLoopIndex(collectionInfo);
                    else
                        MapCollectionLoopForEach(collectionInfo);
                });
        }
    }
}
