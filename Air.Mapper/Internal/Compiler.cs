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
    internal class Compiler
    {
        public Compiler(
            Type sourceType,
            Type destinationType,
            MethodType methodType,
            List<IMapOption> mapOptions)
        {
            SourceType = sourceType;
            DestinationType = destinationType;
            MethodType = methodType;
            MapOptions = mapOptions;
        }

        #region Locals
        private protected Type SourceType { get; set; }
        private protected Type DestinationType { get; set; }
        private protected MethodType MethodType { get; set; }
        private protected List<IMapOption> MapOptions { get; set; }
        private protected Reflection.Emit.ILGenerator IL { get; set; }
        private protected Schema Schema { get; set; }

        private const char DOT = '.';
        private const string Map = nameof(Map);
        private const string To = nameof(To);
        private const string ToArray = nameof(ToArray);

        private protected const string HasValue = nameof(HasValue);
        private protected const string Value = nameof(Value);
        #endregion

        private protected void CheckArguments()
        {
            if (!Schema.CanMapTypes(SourceType, DestinationType))
                throw new NotSupportedException($"Mapping from {SourceType} to {DestinationType} not supported.");
        }

        #region Schema

        private protected void CreateSchema()
        {
            if (Reflection.TypeInfo.IsBuiltIn(SourceType) && Reflection.TypeInfo.IsBuiltIn(DestinationType))
                return;

            if (DestinationType.IsAbstract || DestinationType.IsInterface)
                return;

            Schema = new Schema(SourceType, DestinationType, MapOptions);
        }

        private List<SourceNode> GetDestinationNodeSources(DestinationNode destinationNode)
        {
            List<SourceNode> returnValue = new List<SourceNode>();

            for (int i = 0; i < destinationNode.Members.Count; i++)
            {
                if (destinationNode.Members[i].Map &&
                    !returnValue.Exists(node => node.Name == destinationNode.Members[i].SourceNode.Name))
                    returnValue.Add(destinationNode.Members[i].SourceNode);
            }

            for (int i = 0; i < returnValue.Count; i++)
            {
                SourceNode parentNode = returnValue[i].ParentNode;

                while (parentNode != null)
                {
                    if (!returnValue.Exists(node => node.Name == parentNode.Name))
                        returnValue.Add(parentNode);

                    parentNode = parentNode.ParentNode;
                }
            }

            return returnValue;
        }

        private List<DestinationNode> GetDistinctNodes(List<DestinationNode> destinationNodes)
        {
            List<DestinationNode> returnValue = new List<DestinationNode>();

            for (int i = 0; i < destinationNodes.Count; i++)
                if (!returnValue.Exists(n => n.Name == destinationNodes[i].Name))
                    returnValue.Add(destinationNodes[i]);

            return returnValue;
        }

        #endregion

        #region Declare Locals

        private void DeclareSourceNodeLocals(SourceNode sourceNode)
        {
            if (sourceNode.Local != null ||
                sourceNode.NullableLocal != null)
                return;

            if (sourceNode.NullableUnderlyingType != null)
            {
                if (StructRequired(sourceNode))
                    sourceNode.Local = IL.DeclareLocal(sourceNode.NullableUnderlyingType);

                if (sourceNode.Depth != 0)
                    sourceNode.NullableLocal = IL.DeclareLocal(sourceNode.Type);

            }
            else if (sourceNode.Type.IsValueType && sourceNode.Depth != 0)
            {
                if (StructRequired(sourceNode))
                    sourceNode.Local = IL.DeclareLocal(sourceNode.Type);
            }
        }

        private void DeclareSourceNodesLocals(List<SourceNode> sourceNodes)
        {
            for (int i = 0; i < sourceNodes.Count; i++)
                DeclareSourceNodeLocals(sourceNodes[i]);
        }

        private void DeclareDestinationNodeLocals(DestinationNode destinationNode)
        {
            if (destinationNode.Local != null ||
                destinationNode.NullableLocal != null)
                return;

            if (destinationNode.Depth != 0)
            {
                if (destinationNode.NullableUnderlyingType != null)
                {
                    destinationNode.NullableLocal = IL.DeclareLocal(destinationNode.Type);
                    destinationNode.Local = IL.DeclareLocal(destinationNode.NullableUnderlyingType);
                }
                else if (destinationNode.Type.IsValueType)
                {
                    destinationNode.Local = IL.DeclareLocal(destinationNode.Type);
                }
            }
            else
            {
                if (MethodType == MethodType.ActionRef)
                {
                    if (destinationNode.NullableUnderlyingType != null)
                        destinationNode.Local = IL.DeclareLocal(destinationNode.NullableUnderlyingType);
                }
                else
                {
                    if (destinationNode.NullableUnderlyingType != null)
                    {
                        destinationNode.Local = IL.DeclareLocal(destinationNode.NullableUnderlyingType);
                        destinationNode.NullableLocal = IL.DeclareLocal(destinationNode.Type);
                    }
                    else
                    {
                        destinationNode.Local = IL.DeclareLocal(destinationNode.Type);
                    }
                }
            }
        }

        private bool UseDestinationLocals() =>
            !(Reflection.TypeInfo.IsBuiltIn(DestinationType) || DestinationType.IsAbstract || DestinationType.IsInterface);

        private protected void DeclareLocals()
        {
            if (!UseDestinationLocals())
                return;

            Schema.ForEachDestinationNode(
                n => n.Load,
                n =>
                {
                    DeclareDestinationNodeLocals(n);
                    DeclareSourceNodesLocals(
                        GetDestinationNodeSources(n));
                    DeclareCollectionLocals(n);
                });
        }

        private protected void InitDestinationLocals()
        {
            if (!UseDestinationLocals())
                return;

            Schema.ForEachDestinationNode(n =>
                n.Load &&
                n.Type.IsValueType &&
                n.Local != null &&
                n.Depth != 0,
                n =>
                {
                    IL.EmitInit(n.Local);
                    //IL.EmitLdloca(n.Local.LocalIndex);
                    //IL.EmitInit(n.Local.LocalType);
                });
        }

        #endregion

        #region Load

        private void Load(SourceNode sourceNode)
        {
            if (sourceNode.Depth == 0)
            {
                if (sourceNode.NullableUnderlyingType != null)
                    IL.EmitLdloca(sourceNode.Local.LocalIndex);
                else if (sourceNode.Type.IsValueType)
                    IL.EmitLdarga(0);
                else
                    IL.EmitLdarg(0);

                return;
            }

            if (sourceNode.Local != null)
            {
                LoadLocal(sourceNode.Local, true);
                return;
            }

            Reflection.MemberInfo memberInfo =
                sourceNode.ParentNode.Members.First(w => w.Name == Reflection.TypeInfo.GetName(sourceNode.Name));

            if (memberInfo.IsStatic)
            {
                IL.EmitLoadMemberValue(memberInfo);
                return;
            }

            string[] segments = sourceNode.Name.Split(DOT, StringSplitOptions.RemoveEmptyEntries).ToArray();

            SourceNode parentNode = sourceNode.ParentNode;
            while (parentNode != null)
            {
                if (parentNode.Local != null)
                    LoadLocal(parentNode.Local, true);
                else if (parentNode.IsStatic)
                    IL.EmitLoadMemberValue(parentNode.MemberInfo);

                if (parentNode.IsStatic || parentNode.Local != null)
                {
                    for (int i = 0; i < segments.Length; i++)
                    {
                        string nodeName = string.Join(DOT, segments, 0, i + 1);
                        if (nodeName.Length <= parentNode.Name.Length)
                            continue;

                        IL.EmitLoadMemberValue(Schema.GetMember(nodeName, Schema.SourceNodes));
                    }

                    return;
                }

                parentNode = parentNode.ParentNode;
            }

            Load(Schema.SourceRootNode);
            for (int i = 0; i < segments.Length; i++)
                IL.EmitLoadMemberValue(Schema.GetMember(string.Join(DOT, segments, 0, i + 1), Schema.SourceNodes));
        }

        private void Load(SourceNode sourceNode, Reflection.MemberInfo sourceNodeMember)
        {
            if (!sourceNodeMember.IsStatic)
                Load(sourceNode);

            IL.EmitLoadMemberValue(sourceNodeMember);
        }

        private void LoadLocal(LocalBuilder local, bool loadAddress)
        {
            if (loadAddress)
                IL.EmitLdloca(local.LocalIndex);
            else
                IL.EmitLdloc(local.LocalIndex);
        }

        private void Load(DestinationNode destinationNode)
        {
            if (destinationNode.Depth == 0)
            {
                if (MethodType == MethodType.ActionRef)
                {
                    if (destinationNode.NullableUnderlyingType != null)
                    {
                        IL.EmitLdloca(destinationNode.Local.LocalIndex);
                    }
                    else if (destinationNode.Type.IsValueType)
                    {
                        IL.EmitLdarg(1);
                    }
                    else
                    {
                        IL.EmitLdarg(1);
                        IL.Emit(OpCodes.Ldind_Ref);
                    }
                }
                else
                {
                    LoadLocal(destinationNode.Local, destinationNode.Local.LocalType.IsValueType);
                }

                return;
            }

            if (destinationNode.Local != null)
            {
                LoadLocal(destinationNode.Local, destinationNode.Local.LocalType.IsValueType);
                return;
            }

            Reflection.MemberInfo memberInfo =
                destinationNode.ParentNode.Members.First(w => w.Info.Name == Reflection.TypeInfo.GetName(destinationNode.Name)).Info;

            if (memberInfo.IsStatic)
            {
                IL.EmitLoadMemberValue(memberInfo);
                return;
            }

            string[] segments = destinationNode.Name.Split(DOT, StringSplitOptions.RemoveEmptyEntries).ToArray();

            DestinationNode parentNode = destinationNode.ParentNode;
            while (parentNode != null)
            {
                if (parentNode.Local != null)
                    LoadLocal(parentNode.Local, parentNode.Local.LocalType.IsValueType);
                else if (parentNode.IsStatic)
                    IL.EmitLoadMemberValue(parentNode.MemberInfo);

                if (parentNode.IsStatic || parentNode.Local != null)
                {
                    for (int i = 0; i < segments.Length; i++)
                    {
                        string nodeName = string.Join(DOT, segments, 0, i + 1);
                        if (nodeName.Length <= parentNode.Name.Length)
                            continue;

                        IL.EmitLoadMemberValue(Schema.GetMember(nodeName, Schema.DestinationNodes));
                    }

                    return;
                }

                parentNode = parentNode.ParentNode;
            }

            Load(Schema.DestinationRootNode);
            for (int i = 0; i < segments.Length; i++)
                IL.EmitLoadMemberValue(Schema.GetMember(string.Join(DOT, segments, 0, i + 1), Schema.DestinationNodes));
        }

        private void Load(DestinationNode destinationNode, Reflection.MemberInfo destinationNodeMember)
        {
            if (!destinationNodeMember.IsStatic)
                Load(destinationNode);

            IL.EmitLoadMemberValue(destinationNodeMember);
        }

        private void Load(DestinationNode destinationNode, DestinationNodeMember destinationNodeMember) =>
            Load(destinationNode, destinationNodeMember.Info);

        #endregion

        #region Set

        private void StoreDestinationRootNode()
        {
            if (MethodType == MethodType.ActionRef)
                return;

            if (Schema.DestinationRootNode.NullableUnderlyingType != null) { }
            else if (Schema.DestinationRootNode.Type.IsValueType) { }
            else if (Schema.DestinationRootNode.IsStatic) { }
            else
                IL.EmitStloc(Schema.DestinationRootNode.Local.LocalIndex);
        }

        private void SetDestinationNode(DestinationNode destinationNode)
        {
            if (destinationNode.ParentNode == null ||
                !destinationNode.Type.IsValueType)
                return;

            if (destinationNode.IsStatic) { }
            else if (destinationNode.ParentNode.Type.IsValueType)
            {
                if (destinationNode.ParentNode.Depth == 0 && MethodType == MethodType.ActionRef)
                    Load(destinationNode.ParentNode);
                else
                    IL.EmitLdloca(destinationNode.ParentNode.Local.LocalIndex);
            }
            else
            {
                Load(destinationNode.ParentNode);
            }

            if (destinationNode.NullableUnderlyingType != null)
            {
                LoadLocal(destinationNode.Local, false);
                IL.Emit(OpCodes.Newobj, destinationNode.Type.GetConstructor(new Type[] { destinationNode.NullableUnderlyingType }));
            }
            else
            {
                LoadLocal(destinationNode.Local, false);
            }

            IL.EmitSetMemberValue(destinationNode.MemberInfo);
        }

        private void SetDestinationNodes(List<DestinationNode> destinationNodes)
        {
            List<DestinationNode> nodes = GetDistinctNodes(destinationNodes);

            for (int i = 0; i < destinationNodes.Count; i++)
            {
                DestinationNode parentNode = destinationNodes[i].ParentNode;
                while (parentNode != null)
                {
                    if (!nodes.Exists(n => n.Name == parentNode.Name))
                        nodes.Add(parentNode);

                    parentNode = parentNode.ParentNode;
                }
            }

            nodes.Sort((l, r) => r.Depth.CompareTo(l.Depth));
            for (int i = 0; i < nodes.Count; i++)
                SetDestinationNode(nodes[i]);
        }

        private void SetDestinationNodeMembers(
            DestinationNode destinationNode,
            List<DestinationNodeMember> destinationNodeMembers)
        {
            if (MethodType == MethodType.ActionRef)
            {
                EnsureDestinationNode(destinationNode);
                LoadAndSetDestinationNodeMembers(destinationNode, destinationNodeMembers);

                return;
            }

            if (destinationNode.Type.IsValueType)
            {
                LoadAndSetDestinationNodeMembers(destinationNode, destinationNodeMembers);
            }
            else if (destinationNode.Loaded)
            {
                Load(destinationNode);
                IL.EmitBrtrue(
                    () => { InitAndSetDestinationNodeMembers(destinationNode, destinationNodeMembers); },
                    () => { LoadAndSetDestinationNodeMembers(destinationNode, destinationNodeMembers); });
            }
            else
            {
                InitAndSetDestinationNodeMembers(destinationNode, destinationNodeMembers);
            }
        }

        private void LoadAndSetDestinationNodeMembers(
            DestinationNode destinationNode,
            List<DestinationNodeMember> destinationNodeMembers)
        {
            destinationNodeMembers.Sort((l, r) => l.IsCollection.CompareTo(r.IsCollection));
            foreach (DestinationNodeMember destinationNodeMember in destinationNodeMembers)
            {
                if (!destinationNodeMember.IsCollection)
                {
                    IL.EmitLoadAndSetValue(
                        () =>
                        {
                            if (!destinationNodeMember.Info.IsStatic)
                                Load(destinationNode);

                            Load(destinationNodeMember.SourceNode, destinationNodeMember.SourceNodeMember);
                        },
                        destinationNodeMember.SourceNodeMember,
                        destinationNodeMember.Info);
                }
                else
                {
                    MapCollection(destinationNode, destinationNodeMember);
                }
            }
        }

        #endregion

        #region Map.Collection
        private LocalBuilder LoopIndexLocal { get; set; }
        private LocalBuilder LoopLengthLocal { get; set; }
        private List<LocalBuilder> LoopSourceLocals { get; set; } = new List<LocalBuilder>();
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

        private void test()
        {
            var _source = new string[] { };

            var _1 = new HashSet<string>(_source); // ICollection
            var _2 = new LinkedList<string>(_source); // ICollection
            var _3 = new List<string>(_source); // ICollection
            var _4 = new Queue<string>(_source); // IEnumerable
            var _5 = new SortedSet<string>(_source); // ICollection
            var _6 = new Stack<string>(_source); // IEnumerable
            var _7 = new ConcurrentBag<string>(_source); // IProducerConsumerCollection
            var _8 = new ConcurrentQueue<string>(_source); // IProducerConsumerCollection
            var _9 = new ConcurrentStack<string>(_source); // IProducerConsumerCollection

            var __source = new Dictionary<string, string>();

            var __1 = new Dictionary<string, string>(__source); // ICollection
            var __2 = new SortedDictionary<string, string>(__source); // ICollection
            var __3 = new SortedList<string, string>(__source); // ICollection
            var __4 = new ConcurrentDictionary<string, string>(__source);  // ICollection


            //for (int i = 0; i < _1.Count; i++)


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
            if (collectionInfo.SourceType.IsArray ||
               IsAssignableFrom(collectionInfo.SourceType, typeof(IList<>)) ||
               IsAssignableFrom(collectionInfo.SourceType, typeof(IReadOnlyList<>)))
            {
                collectionInfo.SourceLocal = GetOrAddLoopSourceLocal(collectionInfo.SourceType);

                collectionInfo.LoopIndex = GetOrAddLoopIndexLocal();
                collectionInfo.LoopLength = GetOrAddLoopLengthLocal();
            }
            else if (IsAssignableFrom(collectionInfo.SourceType, typeof(ICollection<>)) ||
                    IsAssignableFrom(collectionInfo.SourceType, typeof(IProducerConsumerCollection<>)))
            {
                collectionInfo.SourceLocal = GetOrAddLoopSourceLocal(collectionInfo.SourceArgument.MakeArrayType());

                collectionInfo.LoopIndex = GetOrAddLoopIndexLocal();
                collectionInfo.LoopLength = GetOrAddLoopLengthLocal();
            }
            else
            {
                collectionInfo.SourceLocal = GetOrAddLoopSourceLocal(collectionInfo.SourceType);
            }
        }

        private void DeclareCollectionDestinationLocals(CollectionInfo collectionInfo)
        {
            Type destinationType = collectionInfo.DestinationType;

            if (collectionInfo.DestinationType.IsInterface)
            {
                if (destinationType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    if (collectionInfo.LoopLength != null)
                        collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(GetIEnumerableArgument(destinationType).MakeArrayType());
                    else
                        collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(typeof(List<>).MakeGenericType(new[] { GetIEnumerableArgument(destinationType) }));
                }
                else
                {
                    collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(
                        MaintainableGenericCollections.First(t => t.Type == destinationType.GetGenericTypeDefinition()).LocalType);
                }

                return;
            }

            if (destinationType.IsArray)
            {
                if (collectionInfo.LoopLength != null)
                    collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(destinationType);
                else
                    collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(typeof(List<>).MakeGenericType(new[] { GetIEnumerableArgument(destinationType) }));
            }
            else
            {
                collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(destinationType);
            }
        }

        private void DeclareCollectionMapperMapLocal(CollectionInfo collectionInfo) =>
            collectionInfo.MapperMapMethodLocal = GetOrAddLoopMapperMapLocal(collectionInfo.SourceArgument, collectionInfo.DestinationArgument);

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
            if (collectionInfo.SourceType.IsArray ||
                IsAssignableFrom(collectionInfo.SourceType, typeof(IList<>)) ||
                IsAssignableFrom(collectionInfo.SourceType, typeof(IReadOnlyList<>)))
            {
                collectionInfo.SourceLocal = GetOrAddLoopSourceLocal(collectionInfo.SourceType);

                collectionInfo.LoopIndex = GetOrAddLoopIndexLocal();
                collectionInfo.LoopLength = GetOrAddLoopLengthLocal();

                IL.EmitLdc_I4(0);
                IL.EmitStoreLocal(collectionInfo.LoopIndex);

                Load(
                    collectionInfo.DestinationNodeMember.SourceNode,
                    collectionInfo.DestinationNodeMember.SourceNodeMember);
                IL.EmitStoreLocal(collectionInfo.SourceLocal);

                IL.EmitLoadLocal(collectionInfo.SourceLocal, false);
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
                    IL.Emit(OpCodes.Callvirt, collectionInfo.SourceType.GetProperty(nameof(ICollection<Type>.Count)).GetGetMethod());
                }

                IL.EmitStoreLocal(collectionInfo.LoopLength);
            }
            else if (IsAssignableFrom(collectionInfo.SourceType, typeof(ICollection<>)) ||
                    IsAssignableFrom(collectionInfo.SourceType, typeof(IProducerConsumerCollection<>)))
            {
                collectionInfo.SourceLocal = GetOrAddLoopSourceLocal(collectionInfo.SourceArgument.MakeArrayType());

                collectionInfo.LoopIndex = GetOrAddLoopIndexLocal();
                collectionInfo.LoopLength = GetOrAddLoopLengthLocal();

                IL.EmitLdc_I4(0);
                IL.EmitStoreLocal(collectionInfo.LoopIndex);

                IL.EmitLoadLocal(collectionInfo.SourceLocal, false);
                IL.Emit(OpCodes.Callvirt, collectionInfo.SourceType.GetProperty(nameof(ICollection.Count)).GetGetMethod());
                IL.EmitStoreLocal(collectionInfo.LoopLength);

                IL.EmitLoadLocal(collectionInfo.LoopLength, false);
                IL.Emit(OpCodes.Newarr, collectionInfo.SourceLocal.LocalType);
                IL.EmitStoreLocal(collectionInfo.SourceLocal);

                Load(
                    collectionInfo.DestinationNodeMember.SourceNode,
                    collectionInfo.DestinationNodeMember.SourceNodeMember);
                IL.EmitLdc_I4(0);

                if (IsAssignableFrom(collectionInfo.SourceType, typeof(ICollection<>)))
                    IL.Emit(OpCodes.Callvirt,
                        typeof(ICollection<>)
                        .MakeGenericType(new[] { collectionInfo.SourceArgument })
                        .GetMethods()
                        .First(m => m.Name == nameof(ICollection<Type>.CopyTo) && m.GetParameters().Length == 2));
                else
                    IL.Emit(OpCodes.Callvirt,
                        typeof(IProducerConsumerCollection<>)
                        .MakeGenericType(new[] { collectionInfo.SourceArgument })
                        .GetMethods()
                        .First(m => m.Name == nameof(IProducerConsumerCollection<Type>.CopyTo) && m.GetParameters().Length == 2));

                IL.EmitLoadLocal(collectionInfo.SourceLocal, false);
                IL.Emit(OpCodes.Ldlen);
                IL.EmitStoreLocal(collectionInfo.LoopLength);
            }
            else
            {
                collectionInfo.SourceLocal = GetOrAddLoopSourceLocal(collectionInfo.SourceType);
                Load(
                    collectionInfo.DestinationNodeMember.SourceNode,
                    collectionInfo.DestinationNodeMember.SourceNodeMember);
                IL.EmitStoreLocal(collectionInfo.SourceLocal);
            }
        }

        private void SetCollectionDestinationLocals(CollectionInfo collectionInfo)
        {
            Type destinationType = collectionInfo.DestinationType;

            if (destinationType.IsInterface)
            {
                if (destinationType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    if (collectionInfo.LoopLength != null)
                    {
                        collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(GetIEnumerableArgument(destinationType).MakeArrayType());
                        IL.EmitLoadLocal(collectionInfo.LoopLength, false);
                        IL.Emit(OpCodes.Newarr, typeof(int));
                    }
                    else
                    {
                        collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(typeof(List<>).MakeGenericType(new[] { GetIEnumerableArgument(destinationType) }));
                        IL.Emit(OpCodes.Newobj, collectionInfo.DestinationLocal);
                    }
                }
                else
                {
                    collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(
                        MaintainableGenericCollections.First(t => t.Type == destinationType.GetGenericTypeDefinition()).LocalType);

                    IL.Emit(OpCodes.Newobj, collectionInfo.DestinationLocal);
                }

                IL.EmitStoreLocal(collectionInfo.DestinationLocal);

                return;
            }


            if (destinationType.IsArray)
            {
                if (collectionInfo.LoopLength != null)
                {
                    collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(destinationType);
                    IL.EmitLoadLocal(collectionInfo.LoopLength, false);
                    IL.Emit(OpCodes.Newarr, typeof(int));
                }
                else
                {
                    collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(typeof(List<>).MakeGenericType(new[] { GetIEnumerableArgument(destinationType) }));
                    IL.Emit(OpCodes.Newobj, collectionInfo.DestinationLocal);
                }

                IL.EmitStoreLocal(collectionInfo.DestinationLocal);
            }
            else if (destinationType.GetInterfaces().Where(ci => ci.IsGenericType).Select(ci => ci.GetGenericTypeDefinition())
                    .Intersect(MaintainableGenericCollections.Select(s => s.Type))
                    .Any())
            {
                collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(destinationType);

                if (collectionInfo.LoopLength != null && destinationType.GetConstructor(new[] { typeof(int) }) != null)
                {
                    IL.EmitLoadLocal(collectionInfo.LoopLength, false);
                    IL.Emit(OpCodes.Newobj, destinationType.GetConstructor(new[] { typeof(int) }));
                }
                else
                {
                    IL.Emit(OpCodes.Newobj, destinationType.GetConstructor(Type.EmptyTypes));
                }

                IL.EmitStoreLocal(collectionInfo.DestinationLocal);
            }
            else
            {
                collectionInfo.DestinationLocal = GetOrAddLoopDestinationLocal(destinationType);
            }
        }

        private void SetCollectionMapperMapLocal(CollectionInfo collectionInfo)
        {
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

        private void MapCollectionLoopForEach(CollectionInfo collectionInfo)
        {

        }

        private void MapCollectionLoopIndex(CollectionInfo collectionInfo)
        {
            Label loopCompareIndex = IL.DefineLabel();
            Label loopBody = IL.DefineLabel();

            IL.Emit(OpCodes.Br_S, loopCompareIndex);

            IL.MarkLabel(loopBody);

            IL.EmitLoadLocal(collectionInfo.DestinationLocal, false);
            IL.EmitLoadLocal(collectionInfo.MapperMapMethodLocal, false);

            IL.EmitLoadLocal(collectionInfo.SourceLocal, false);
            IL.EmitLoadLocal(collectionInfo.LoopIndex, false);


            if (collectionInfo.SourceLocal.LocalType.IsArray)
                IL.Emit(OpCodes.Ldelem_I4);
            else
                IL.Emit(OpCodes.Callvirt, collectionInfo.SourceLocal.LocalType.GetProperties().First(p => p.GetIndexParameters().Length > 0).GetGetMethod());

            IL.Emit(OpCodes.Callvirt, typeof(Func<,>).MakeGenericType(new[] { collectionInfo.SourceArgument, collectionInfo.DestinationArgument }).GetMethod(nameof(Func<Type>.Invoke)));

            if (collectionInfo.DestinationLocal.LocalType.IsArray)
                IL.Emit(OpCodes.Stelem_Ref);
            else
                IL.Emit(OpCodes.Callvirt, collectionInfo.DestinationLocal.LocalType.GetMethods().First(m => m.Name == nameof(IList.Add) && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == collectionInfo.DestinationArgument));

            IL.EmitLoadLocal(collectionInfo.LoopIndex, false);
            IL.EmitLdc_I4(1);
            IL.Emit(OpCodes.Add);
            IL.EmitStoreLocal(collectionInfo.LoopIndex);

            IL.MarkLabel(loopCompareIndex);

            IL.EmitLoadLocal(collectionInfo.LoopIndex, false);
            IL.EmitLoadLocal(collectionInfo.LoopLength, false);

            IL.Emit(OpCodes.Blt_S, loopBody);

            if (!collectionInfo.DestinationNodeMember.Info.IsStatic)
                Load(collectionInfo.DestinationNode);

            IL.EmitLoadLocal(collectionInfo.DestinationLocal, false);
            IL.EmitSetMemberValue(collectionInfo.DestinationNodeMember.Info);
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

        //private void MapFromArray(CollectionInfo collectionInfo)
        //{
        //    SetCollectionLocals(collectionInfo);

        //    GetOrAddLoopMapperMapLocal(
        //        collectionInfo.SourceArgument,
        //        collectionInfo.DestinationArgument,
        //        out LocalBuilder mapperLocal,
        //        out MethodInfo compiledMapperMethod);

        //    IL.Emit(OpCodes.Call, compiledMapperMethod);
        //    IL.EmitStoreLocal(mapperLocal);

        //    LocalBuilder loopIndex = GetOrAddLoopIndexLocal();
        //    LocalBuilder loopLength = GetOrAddLoopLengthLocal();

        //    Load(
        //        collectionInfo.DestinationNodeMember.SourceNode,
        //        collectionInfo.DestinationNodeMember.SourceNodeMember);

        //    IL.EmitBrfalse_s(
        //        () =>
        //        {
        //            Load(
        //                collectionInfo.DestinationNodeMember.SourceNode,
        //                collectionInfo.DestinationNodeMember.SourceNodeMember);
        //            IL.EmitStoreLocal(collectionInfo.SourceLocal);

        //            IL.EmitLdc_I4(0);
        //            IL.EmitStoreLocal(loopIndex);

        //            IL.EmitLoadLocal(collectionInfo.SourceLocal, false);
        //            IL.Emit(OpCodes.Ldlen);
        //            IL.Emit(OpCodes.Conv_I4);

        //            IL.EmitStoreLocal(loopLength);
        //            IL.EmitLoadLocal(loopLength, false);

        //            IL.Emit(OpCodes.Newarr, typeof(int));

        //            IL.EmitStoreLocal(collectionInfo.DestinationLocal);

        //            Label loopCompareIndex = IL.DefineLabel();
        //            Label loopBody = IL.DefineLabel();

        //            IL.Emit(OpCodes.Br_S, loopCompareIndex);

        //            IL.MarkLabel(loopBody);

        //            IL.EmitLoadLocal(collectionInfo.DestinationLocal, false);
        //            IL.EmitLoadLocal(mapperLocal, false);

        //            IL.EmitLoadLocal(collectionInfo.SourceLocal, false);
        //            IL.EmitLoadLocal(loopIndex, false);

        //            IL.Emit(OpCodes.Ldelem_I4);

        //            IL.Emit(OpCodes.Callvirt, typeof(Func<,>).MakeGenericType(new[] { collectionInfo.SourceArgument, collectionInfo.DestinationArgument }).GetMethod(nameof(Func<Type>.Invoke)));

        //            // todo review
        //            if (collectionInfo.DestinationType.IsArray)
        //                IL.Emit(OpCodes.Stelem_Ref);
        //            //else
        //            //    IL.Emit(OpCodes.Callvirt, destinationAddItem);

        //            IL.EmitLoadLocal(loopIndex, false);
        //            IL.EmitLdc_I4(1);
        //            IL.Emit(OpCodes.Add);
        //            IL.EmitStoreLocal(loopIndex);

        //            IL.MarkLabel(loopCompareIndex);

        //            IL.EmitLoadLocal(loopIndex, false);
        //            IL.EmitLoadLocal(loopLength, false);

        //            IL.Emit(OpCodes.Blt_S, loopBody);

        //            if (!collectionInfo.DestinationNodeMember.Info.IsStatic)
        //                Load(collectionInfo.DestinationNode);

        //            IL.EmitLoadLocal(collectionInfo.DestinationLocal, false);
        //            IL.EmitSetMemberValue(collectionInfo.DestinationNodeMember.Info);
        //        });
        //}



        #endregion



        #region todo removed

        //private string GetMapperToCollectionMethodName(Type collectionType) =>
        //    collectionType.IsArray ? ToArray : $"{To}{collectionType.GetGenericTypeDefinition().Name.Split('`')[0]}";

        //private void ConvertToCollection(Type collectionType)
        //{
        //    IL.Emit(OpCodes.Dup);
        //    IL.EmitBrfalse_s(() => IL.Emit(OpCodes.Newobj, GetGenericIEnumerableParameterTypeConstructor(collectionType)));
        //}

        //private MethodInfo GetMapperToKeyValuePairCollectionTypeMethod(
        //    DestinationNodeMember destinationNodeMember,
        //    Type sourceElementType,
        //    Type destinationElementType)
        //{
        //    Type destinationType = destinationNodeMember.Info.Type;

        //    MethodInfo[] methods = GetGenericIEnumerableParameterTypeMethods(
        //        MakeGenericMapper(sourceElementType.GenericTypeArguments[1], destinationElementType.GenericTypeArguments[1]),
        //        ToKeyValuePairCollectionArgumentPredicate);

        //    MethodInfo methodInfo = methods.FirstOrDefault(m =>
        //            m.Name == GetMapperToCollectionMethodName(destinationType) &&
        //            m.IsGenericMethod &&
        //            m.ReturnType.IsGenericType &&
        //            m.ReturnType.GetGenericTypeDefinition() == destinationType.GetGenericTypeDefinition()) ??
        //        methods.Single(m =>
        //            m.Name == ToArray &&
        //            m.IsGenericMethod &&
        //            m.ReturnType.IsArray);

        //    return methodInfo.MakeGenericMethod(new[]
        //    {
        //        sourceElementType.GenericTypeArguments[0],
        //        destinationElementType.GenericTypeArguments[0]
        //    });
        //}

        //private MethodInfo GetMapperToCollectionTypeMethod(
        //    DestinationNodeMember destinationNodeMember,
        //    Type sourceElementType,
        //    Type destinationElementType)
        //{
        //    Type sourceType = destinationNodeMember.SourceNodeMember.Type;
        //    Type destinationType = destinationNodeMember.Info.Type;

        //    MethodInfo[] methods = MakeGenericMapper(sourceElementType, destinationElementType)
        //       .GetMethods(BindingFlags.Public | BindingFlags.Static);

        //    MethodInfo methodInfo =
        //        methods.FirstOrDefault(m =>
        //            m.Name == GetMapperToCollectionMethodName(destinationType) &&
        //            !m.IsGenericMethod &&
        //            m.ReturnType == destinationType &&
        //            m.GetParameters().Length == 1 &&
        //            m.GetParameters()[0].ParameterType == sourceType) ??
        //        methods.FirstOrDefault(m =>
        //            m.Name == GetMapperToCollectionMethodName(destinationType) &&
        //            !m.IsGenericMethod &&
        //            m.ReturnType == destinationType &&
        //            m.GetParameters().Length == 1 &&
        //            m.GetParameters()[0].ParameterType == MakeGenericIEnumerableType(sourceElementType));

        //    if (methodInfo != null)
        //        return methodInfo;

        //    methodInfo = methods.FirstOrDefault(m =>
        //            m.Name == ToArray &&
        //            !m.IsGenericMethod &&
        //            m.ReturnType == destinationElementType.MakeArrayType() &&
        //            m.GetParameters().Length == 1 &&
        //            m.GetParameters()[0].ParameterType == sourceType) ??
        //        methods.Single(m =>
        //            m.Name == ToArray &&
        //            !m.IsGenericMethod &&
        //            m.ReturnType == destinationElementType.MakeArrayType() &&
        //            m.GetParameters().Length == 1 &&
        //            m.GetParameters()[0].ParameterType == MakeGenericIEnumerableType(sourceElementType));

        //    return methodInfo;
        //}

        //private MethodInfo GetMapperToCollectionMethod(DestinationNodeMember destinationNodeMember)
        //{
        //    Type sourceElementType = GetGenericIEnumerableElementType(destinationNodeMember.SourceNodeMember.Type);
        //    Type destinationElementType = GetGenericIEnumerableElementType(destinationNodeMember.Info.Type);

        //    if (sourceElementType.IsGenericType && sourceElementType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
        //        return GetMapperToKeyValuePairCollectionTypeMethod(destinationNodeMember, sourceElementType, destinationElementType);
        //    else
        //        return GetMapperToCollectionTypeMethod(destinationNodeMember, sourceElementType, destinationElementType);
        //}

        //private void MapperToCollection(
        //   DestinationNode destinationNode,
        //   DestinationNodeMember destinationNodeMember)
        //{
        //    if (MethodType == MethodType.ActionRef)
        //        return;

        //    MethodInfo mapperToCollection = GetMapperToCollectionMethod(destinationNodeMember);

        //    if (!destinationNodeMember.Info.IsStatic)
        //        Load(destinationNode);

        //    Load(destinationNodeMember.SourceNode, destinationNodeMember.SourceNodeMember);
        //    IL.Emit(OpCodes.Call, mapperToCollection);

        //    if (mapperToCollection.ReturnType != destinationNodeMember.Info.Type)
        //        ConvertToCollection(destinationNodeMember.Info.Type);

        //    IL.EmitSetMemberValue(destinationNodeMember.Info);
        //}

        //private void DupMapperToCollection(DestinationNodeMember destinationNodeMember)
        //{
        //    if (MethodType == MethodType.ActionRef)
        //        return;

        //    IL.Emit(OpCodes.Dup);
        //    Load(destinationNodeMember.SourceNode, destinationNodeMember.SourceNodeMember);
        //    MethodInfo mapperToCollection = GetMapperToCollectionMethod(destinationNodeMember);
        //    IL.Emit(OpCodes.Call, mapperToCollection);

        //    if (mapperToCollection.ReturnType != destinationNodeMember.Info.Type)
        //        ConvertToCollection(destinationNodeMember.Info.Type);

        //    IL.EmitSetMemberValue(destinationNodeMember.Info);
        //}



        #endregion




        private protected bool MapsNodeMembers(SourceNode sourceNode)
        {
            if (!sourceNode.Load)
                return false;

            List<SourceNode> childNodes = Schema.GetChildNodes(sourceNode, 1, n => n.Load);

            return Schema.DestinationNodes.Exists(n =>
                n.Load &&
                n.MembersMapCount != 0 &&
                n.Members.Exists(m => m.Map && m.SourceNode.Name == sourceNode.Name));
        }

        private protected bool MapsNodesMembers(SourceNode sourceNode)
        {
            if (!sourceNode.Load)
                return false;

            List<SourceNode> childNodes = Schema.GetChildNodes(sourceNode, 0, n => n.Load);

            return Schema.DestinationNodes.Exists(n =>
                n.Load &&
                n.MembersMapCount != 0 && n.Members.Exists(m => m.Map &&
                    (
                        m.SourceNode.Name == sourceNode.Name ||
                        childNodes.Exists(c => c.Name == m.SourceNode.Name)
                    ))
                );
        }

        private protected bool StructRequired(SourceNode sourceNode)
        {
            List<SourceNode> childNodes = Schema.GetChildNodes(sourceNode, 1, (n) => n.Load);

            return childNodes.Exists(n => !n.IsStatic) ||
                Schema.DestinationNodes.Exists(n =>
                    n.Load &&
                    n.MembersMapCount != 0 && n.Members.Exists(m =>
                        m.Map &&
                        m.SourceNode.Name == sourceNode.Name &&
                        !m.SourceNodeMember.IsStatic));
        }

        private void MapDestinationNodeMembers(
            SourceNode sourceNode,
            DestinationNode destinationNode,
            List<DestinationNodeMember> destinationNodeMembers)
        {
            EnsureDestinationNodePath(destinationNode, sourceNode);
            SetDestinationNodeMembers(destinationNode, destinationNodeMembers);
        }

        private protected void MapSourceNodes(SourceNode sourceNode)
        {
            if (!MapsNodesMembers(sourceNode))
                return;

            if (sourceNode.Depth == 0)
            {
                MapSourceNode(sourceNode);
            }
            else if (sourceNode.NullableUnderlyingType != null)
            {
                Load(sourceNode.ParentNode, sourceNode.MemberInfo);
                IL.EmitStloc(sourceNode.NullableLocal.LocalIndex);
                LoadLocal(sourceNode.NullableLocal, true);
                IL.Emit(OpCodes.Call, sourceNode.NullableLocal.LocalType.GetProperty(HasValue).GetGetMethod());
                IL.EmitBrfalse(
                    () =>
                    {
                        if (StructRequired(sourceNode))
                        {
                            LoadLocal(sourceNode.NullableLocal, true);
                            IL.Emit(OpCodes.Call, sourceNode.NullableLocal.LocalType.GetProperty(Value).GetGetMethod());
                            IL.EmitStloc(sourceNode.Local.LocalIndex);
                        }

                        MapSourceNode(sourceNode);
                    });
            }
            else if (sourceNode.Type.IsValueType)
            {
                if (StructRequired(sourceNode))
                {
                    Load(sourceNode.ParentNode, sourceNode.MemberInfo);
                    IL.EmitStloc(sourceNode.Local.LocalIndex);
                }

                MapSourceNode(sourceNode);
            }
            else
            {
                Load(sourceNode);
                IL.EmitBrfalse(() => MapSourceNode(sourceNode));
            }
        }

        private void MapSourceChildNodes(SourceNode sourceNode)
        {
            List<SourceNode> childNodes = Schema.GetChildNodes(sourceNode, 1, n => n.Load);
            for (int s = 0; s < childNodes.Count; s++)
                MapSourceNodes(childNodes[s]);
        }

        private void MapSourceNode(SourceNode sourceNode)
        {
            List<DestinationNode> destinationNodes = new List<DestinationNode>();

            Schema.ForEachDestinationNode(
                n => n.Load,
                n =>
                {
                    List<DestinationNodeMember> members = Schema.GetDestinationNodeMembers(n, m =>
                        m.SourceNode != null &&
                        m.SourceNode.Name == sourceNode.Name &&
                        m.Map &&
                        m.Status == Status.Successful);

                    if (members.Count != 0)
                    {
                        MapDestinationNodeMembers(sourceNode, n, members);
                        destinationNodes.Add(n);
                    }
                });

            MapSourceChildNodes(sourceNode);
            SetDestinationNodes(destinationNodes);
        }

        private void Init(DestinationNode destinationNode)
        {
            if (destinationNode.Depth != 0)
            {
                if (destinationNode.NullableUnderlyingType != null) { }
                else if (destinationNode.Type.IsValueType) { }
                else if (destinationNode.IsStatic)
                {
                    IL.EmitInit(destinationNode.Type);
                    IL.EmitSetMemberValue(destinationNode.MemberInfo);
                }
                else
                {
                    Load(destinationNode.ParentNode);
                    IL.EmitInit(destinationNode.Type);
                    IL.EmitSetMemberValue(destinationNode.MemberInfo);
                }

                return;
            }

            if (destinationNode.NullableUnderlyingType != null)
            {
                if (MethodType == MethodType.ActionRef)
                {
                    IL.EmitLdarg(1);
                    IL.EmitInit(destinationNode.Type);

                    IL.EmitLdloca(destinationNode.Local.LocalIndex);
                    IL.EmitInit(destinationNode.Local.LocalType);
                }
                else
                {
                    LoadLocal(destinationNode.NullableLocal, true);
                    IL.EmitInit(destinationNode.NullableLocal.LocalType);
                }
            }
            else if (destinationNode.Type.IsValueType)
            {
                if (MethodType == MethodType.ActionRef)
                {
                    IL.EmitLdarg(1);
                    IL.EmitInit(destinationNode.Type);
                }
            }
            else if (destinationNode.IsStatic) { }
            else
            {
                if (MethodType == MethodType.ActionRef)
                {
                    IL.EmitLdarg(1);
                    IL.EmitInit(destinationNode.Type);
                    IL.Emit(OpCodes.Stind_Ref);
                }
                else
                {
                    IL.EmitInit(destinationNode.Local.LocalType);
                    IL.EmitStloc(destinationNode.Local.LocalIndex);
                }
            }
        }

        private void InitIfNull(DestinationNode destinationNode)
        {
            if (destinationNode.Depth != 0)
            {
                if (destinationNode.NullableUnderlyingType != null) { }
                else if (destinationNode.Type.IsValueType) { }
                else if (destinationNode.IsStatic)
                {
                    IL.EmitLoadMemberValue(destinationNode.MemberInfo);
                    IL.EmitBrtrue_s(() => Init(destinationNode));
                }
                else
                {
                    Load(destinationNode);
                    IL.EmitBrtrue_s(() => Init(destinationNode));
                }

                return;
            }

            if (destinationNode.NullableUnderlyingType != null)
            {
                Load(destinationNode);
                IL.EmitBrtrue_s(() => Init(destinationNode));
            }
            else if (destinationNode.Type.IsValueType) { }
            else if (destinationNode.IsStatic) { }
            else
            {
                Load(destinationNode);
                IL.EmitBrtrue_s(() => Init(destinationNode));
            }
        }

        /// <summary>
        /// Avoid using valueType destinationNode
        /// </summary>
        /// <param name="destinationNode"></param>
        /// <param name="destinationNodeMembers"></param>
        private void InitAndSetDestinationNodeMembers(
            DestinationNode destinationNode,
            List<DestinationNodeMember> destinationNodeMembers)
        {
            if (destinationNode.Depth == 0 && MethodType == MethodType.ActionRef)
            {
                InitIfNull(destinationNode);

                LoadAndSetDestinationNodeMembers(destinationNode, destinationNodeMembers);

                return;
            }

            List<DestinationNodeMember> loadAndSetQueue = new List<DestinationNodeMember>();

            if (destinationNode.Depth != 0 && !destinationNode.IsStatic)
                Load(destinationNode.ParentNode);

            IL.EmitInit(destinationNode.Type);
            destinationNode.Loaded = true;

            destinationNodeMembers.Sort((l, r) => l.IsCollection.CompareTo(r.IsCollection));
            foreach (DestinationNodeMember destinationNodeMember in destinationNodeMembers)
            {
                if (destinationNodeMember.Info.IsStatic)
                {
                    loadAndSetQueue.Add(destinationNodeMember);
                    continue;
                }
                else if (destinationNodeMember.IsCollection)
                {
                    loadAndSetQueue.Add(destinationNodeMember);
                    continue;
                }

                IL.Emit(OpCodes.Dup);

                IL.EmitLoadAndSetValue(
                    () => Load(destinationNodeMember.SourceNode, destinationNodeMember.SourceNodeMember),
                    destinationNodeMember.SourceNodeMember,
                    destinationNodeMember.Info);
            }

            if (destinationNode.Depth != 0)
                IL.EmitSetMemberValue(destinationNode.MemberInfo);
            else
                StoreDestinationRootNode();

            if (loadAndSetQueue.Count != 0)
                LoadAndSetDestinationNodeMembers(destinationNode, loadAndSetQueue);
        }

        private void EnsureDestinationNodePath(DestinationNode destinationNode, SourceNode sourceNode)
        {
            if (destinationNode.Depth == 0)
                return;

            List<SourceNode> sourceNodes = sourceNode.ParentNodes;
            sourceNodes.Sort((l, r) => l.Depth.CompareTo(r.Depth));
            sourceNodes.Add(sourceNode);

            List<DestinationNode> destinationNodes = destinationNode.ParentNodes;
            destinationNodes.Sort((l, r) => l.Depth.CompareTo(r.Depth));

            for (int n = 0; n < destinationNodes.Count; n++)
            {
                if (sourceNodes
                    .SelectMany(s => s.DestinationNodes)
                    .Any(w => w.Name == destinationNodes[n].Name))
                    continue;

                bool loaded = destinationNodes[n].Loaded;

                sourceNode.DestinationNodes.Add(destinationNodes[n]);
                destinationNodes[n].Loaded = true;

                if (loaded)
                    continue;

                if (MethodType == MethodType.ActionRef)
                    EnsureDestinationNode(destinationNodes[n]);
                else
                    Init(destinationNodes[n]);
            }

            sourceNode.DestinationNodes.Add(destinationNode);
        }

        private void EnsureDestinationNode(DestinationNode destinationNode)
        {
            if (destinationNode.Depth == 0)
            {
                InitIfNull(destinationNode);
                return;
            }

            if (destinationNode.NullableUnderlyingType != null)
            {
                Load(destinationNode.ParentNode, destinationNode.MemberInfo);
                IL.EmitStloc(destinationNode.NullableLocal.LocalIndex);
                LoadLocal(destinationNode.NullableLocal, true);
                IL.Emit(OpCodes.Call, destinationNode.Type.GetProperty(HasValue).GetGetMethod());
                IL.EmitBrfalse_s(
                    () =>
                    {
                        LoadLocal(destinationNode.NullableLocal, true);
                        IL.Emit(OpCodes.Call, destinationNode.Type.GetProperty(Value).GetGetMethod());
                        IL.EmitStloc(destinationNode.Local.LocalIndex);
                    },
                    () =>
                    {
                        LoadLocal(destinationNode.Local, true);
                        IL.EmitInit(destinationNode.Local.LocalType);
                    },
                    true);
            }
            else if (destinationNode.Type.IsValueType)
            {
                Load(destinationNode.ParentNode, destinationNode.MemberInfo);
                IL.EmitStloc(destinationNode.Local.LocalIndex);
            }
            else
            {
                InitIfNull(destinationNode);
            }
        }
    }
}
