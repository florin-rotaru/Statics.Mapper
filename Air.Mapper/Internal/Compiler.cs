using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Air.Mapper.Internal
{
    internal abstract class Compiler<S, D>
    {
        private protected List<IMapOption> MapOptions { get; private set; }
        private protected DynamicMethod Method { get; set; }
        private protected Reflection.Emit.ILGenerator IL { get; set; }
        private protected Schema Schema { get; set; }

        private protected const string Value = nameof(Value);
        private protected const string HasValue = nameof(HasValue);
        private protected const string ToArray = nameof(ToArray);
        private protected const char DOT = '.';

        private protected Argument Source = new Argument(typeof(S));
        private protected Argument Destination = new Argument(typeof(D));

        private protected void SetMapOptions(Action<MapOptions<S, D>> mapOptions = null)
        {
            MapOptions<S, D> options = new MapOptions<S, D>();
            mapOptions?.Invoke(options);

            MapOptions = options.Get();
        }

        private protected void CheckArguments()
        {
            if (!Source.IsBuiltIn && typeof(IEnumerable).IsAssignableFrom(Source.Type))
                throw new NotSupportedException($"Mapping from {Source.Type} {nameof(IEnumerable)} not supported.");

            if (!Destination.IsBuiltIn && typeof(IEnumerable).IsAssignableFrom(Destination.Type))
                throw new NotSupportedException($"Mapping to {Destination.Type} {nameof(IEnumerable)} not supported.");

            if (Source.IsBuiltIn != Destination.IsBuiltIn)
                throw new NotSupportedException($"Mapping from {Source.Type} to {Destination.Type} not supported.");

            if (Source.IsBuiltIn && Destination.IsBuiltIn && !Reflection.Emit.ILGenerator.CanEmitSetOrConvert(Source.Type, Destination.Type))
                throw new NotSupportedException($"Cannot convert from {Source.Type} to {Destination.Type}.");

            if (Destination.Type.IsInterface && Source.Type != Destination.Type)
                throw new NotSupportedException($"Mapping from {Source.Type} to Interface {Destination.Type} not supported.");

            if (Destination.Type.IsAbstract && Source.Type != Destination.Type)
                throw new NotSupportedException($"Mapping from {Source.Type} to Abstract {Destination.Type} not supported.");
        }

        #region Schema

        private protected void CreateSchema()
        {
            if (Source.IsBuiltIn && Destination.IsBuiltIn)
                return;

            if (Destination.Type.IsAbstract || Destination.Type.IsInterface)
                return;

            Schema = new Schema(Source.Type, Destination.Type, MapOptions);
        }

        private List<SourceNode> GetDestinationNodeSources(DestinationNode destinationNode)
        {
            List<SourceNode> returnValue = new List<SourceNode>();

            if (destinationNode.UseMapper)
            {
                returnValue.Add(destinationNode.SourceNode.ParentNode);
            }
            else
            {
                for (int i = 0; i < destinationNode.Members.Count; i++)
                {
                    if (destinationNode.Members[i].Map &&
                        !returnValue.Exists(node => node.Name == destinationNode.Members[i].SourceNode.Name))
                        returnValue.Add(destinationNode.Members[i].SourceNode);
                }
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

        #region Locals

        private void DeclareSourceNodesLocals(List<SourceNode> sourceNodes)
        {
            for (int i = 0; i < sourceNodes.Count; i++)
            {
                if (sourceNodes[i].Local != null ||
                    sourceNodes[i].NullableLocal != null)
                    continue;

                if (sourceNodes[i].NullableUnderlyingType != null)
                {
                    if (StructRequired(sourceNodes[i]))
                        sourceNodes[i].Local = IL.DeclareLocal(sourceNodes[i].NullableUnderlyingType);

                    if (sourceNodes[i].Depth != 0)
                        sourceNodes[i].NullableLocal = IL.DeclareLocal(sourceNodes[i].Type);

                }
                else if (sourceNodes[i].Type.IsValueType && sourceNodes[i].Depth != 0)
                {
                    if (StructRequired(sourceNodes[i]))
                        sourceNodes[i].Local = IL.DeclareLocal(sourceNodes[i].Type);
                }
            }
        }

        private void DeclareDestinationNodeLocals(DestinationNode destinationNode)
        {
            if (destinationNode.Local != null ||
                destinationNode.NullableLocal != null)
                return;

            if (destinationNode.Depth != 0)
            {
                if (Method.ReturnType != typeof(void) && destinationNode.UseMapper) { }
                else if (destinationNode.NullableUnderlyingType != null)
                {
                    destinationNode.NullableLocal = IL.DeclareLocal(destinationNode.Type);

                    if (!destinationNode.UseMapper)
                        destinationNode.Local = IL.DeclareLocal(destinationNode.NullableUnderlyingType);
                }
                else if (destinationNode.Type.IsValueType)
                {
                    destinationNode.Local = IL.DeclareLocal(destinationNode.Type);
                }
                else if (destinationNode.UseMapper)
                {
                    destinationNode.Local = IL.DeclareLocal(destinationNode.Type);
                }
            }
            else
            {
                if (Method.ReturnType == typeof(void))
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

            if (!destinationNode.UseMapper)
                DeclareDestinationNodeMembersLocals(destinationNode);
        }

        private protected void DeclareDestinationNodeMembersLocals(DestinationNode destinationNode)
        {
            if (Method.ReturnType != typeof(void))
                return;

            for (int i = 0; i < destinationNode.Members.Count; i++)
                if (destinationNode.Members[i].IsCollection && destinationNode.Members[i].Map)
                    destinationNode.Members[i].Local = IL.DeclareLocal(CollectionDestinationType(destinationNode.Members[i].Info.Type));
        }

        private bool UseDestinationLocals() =>
            !(Destination.IsBuiltIn || Destination.Type.IsAbstract || Destination.Type.IsInterface);

        private protected void DeclareLocals()
        {
            if (!UseDestinationLocals())
                return;

            for (int i = 0; i < Schema.DestinationNodes.Count; i++)
            {
                if (!Schema.DestinationNodes[i].Load)
                    continue;

                DeclareDestinationNodeLocals(Schema.DestinationNodes[i]);
                DeclareSourceNodesLocals(
                    GetDestinationNodeSources(Schema.DestinationNodes[i]));
            }
        }

        private protected void InitDestinationLocals()
        {
            if (!UseDestinationLocals())
                return;

            foreach (DestinationNode destinationNode in Schema.DestinationNodes)
            {
                if (!destinationNode.Load ||
                    !destinationNode.Type.IsValueType ||
                    destinationNode.UseMapper ||
                    destinationNode.Local == null ||
                    destinationNode.Depth == 0)
                    continue;

                IL.EmitLdloca(destinationNode.Local.LocalIndex);
                IL.EmitInit(destinationNode.Local.LocalType);
            }
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
                if (Method.ReturnType == typeof(void))
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

        private void Load(DestinationNode destinationNode, DestinationNodeMember destinationNodeMember)
            => Load(destinationNode, destinationNodeMember.Info);

        private void Load(DestinationNode destinationNode, Reflection.MemberInfo destinationNodeMember)
        {
            if (!destinationNodeMember.IsStatic)
                Load(destinationNode);

            IL.EmitLoadMemberValue(destinationNodeMember);
        }

        #endregion

        #region Set

        private void StoreDestinationRootNode()
        {
            if (Method.ReturnType == typeof(void))
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

            if (destinationNode.UseMapper && Method.ReturnType != typeof(void))
            {
                IL.EmitSetMemberValue(destinationNode.MemberInfo);
                return;
            }

            if (destinationNode.IsStatic) { }
            else if (destinationNode.ParentNode.Type.IsValueType)
            {
                if (destinationNode.ParentNode.Depth == 0 && Method.ReturnType == typeof(void))
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
                if (destinationNode.UseMapper)
                {
                    LoadLocal(destinationNode.NullableLocal, false);
                }
                else
                {
                    LoadLocal(destinationNode.Local, false);
                    IL.Emit(OpCodes.Newobj, destinationNode.Type.GetConstructor(new Type[] { destinationNode.NullableUnderlyingType }));
                }
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

        private void SetDestinationNodeMembers(DestinationNode destinationNode, List<DestinationNodeMember> destinationNodeMembers)
        {
            if (Method.ReturnType == typeof(void))
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

        private void LoadAndSetCollection(DestinationNode destinationNode, DestinationNodeMember destinationNodeMember)
        {
            if (Schema.CanLoadAndSetCollectionValue(destinationNodeMember.SourceNodeMember, destinationNodeMember.Info))
            {
                MapperMapCollection(destinationNode, destinationNodeMember);
                return;
            }

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

        private void LoadAndSetDestinationNodeMembers(DestinationNode destinationNode, List<DestinationNodeMember> destinationNodeMembers)
        {
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
                    LoadAndSetCollection(destinationNode, destinationNodeMember);
                }
            }
        }

        #endregion

        #region MapperMap

        private bool TryConvertToArray(DestinationNodeMember destinationNodeMember)
        {
            MethodInfo methodInfo = destinationNodeMember.Info.Type
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(m =>
                    m.Name == ToArray &&
                    m.GetParameters().Length == 0 &&
                    m.ReturnType == destinationNodeMember.Local.LocalType);

            if (methodInfo == null)
                return false;

            IL.Emit(OpCodes.Callvirt, methodInfo);

            return true;
        }

        private void CallLinqEnumerableToArray(DestinationNodeMember destinationNodeMember)
        {
            MethodInfo methodInfo = typeof(Enumerable)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(m =>
                    m.Name == ToArray &&
                    m.GetParameters().Length == 1 &&
                    m.IsDefined(typeof(ExtensionAttribute), false));

            methodInfo = methodInfo.MakeGenericMethod(destinationNodeMember.Local.LocalType.GetElementType());

            IL.Emit(OpCodes.Call, methodInfo);
        }

        private bool IsDictionary(Type type)
        {
            return
                type.GenericTypeArguments.Length == 2 &&
                type.GetInterfaces().Where(i => i.IsGenericType).Select(i => i.GetGenericTypeDefinition()).Contains(typeof(IDictionary<,>));
        }

        private void ConvertDestinationNodeMemberTypeToCollection(
            DestinationNode destinationNode,
            DestinationNodeMember destinationNodeMember)
        {
            if (IsDictionary(destinationNodeMember.Info.Type))
                return;

            IL.Emit(OpCodes.Ldnull);
            IL.EmitStloc(destinationNodeMember.Local.LocalIndex);

            Load(destinationNode, destinationNodeMember);
            IL.EmitBrfalse_s(() =>
            {
                Load(destinationNode, destinationNodeMember);
                if (!TryConvertToArray(destinationNodeMember))
                    CallLinqEnumerableToArray(destinationNodeMember);
                IL.EmitStloc(destinationNodeMember.Local.LocalIndex);
            });
        }

        private void ConvertCollectionToDestinationNodeMemberType(DestinationNodeMember destinationNodeMember) =>
            IL.Emit(OpCodes.Newobj, destinationNodeMember.Info.Type.GetConstructor(new[]
            {
                CollectionDestinationType(destinationNodeMember.Info.Type)
            }));

        private Type CollectionDestinationType(Type type)
        {
            if (type.IsArray)
                return type;

            if (type.GenericTypeArguments.Length == 1)
                return type.GenericTypeArguments[0].MakeArrayType();

            return typeof(Dictionary<,>).MakeGenericType(new[]
            {
                type.GenericTypeArguments[0],
                type.GenericTypeArguments[1]
            });
        }

        private Type MapperMapToArrayGenericTypeArgument(Type type)
        {
            if (type.IsArray)
                return type.GetElementType();

            return type.GenericTypeArguments[0];
        }

        private MethodInfo MapperMapCollectionFuncMethodInfo(Type sourceType, Type destinationType)
        {
            if (destinationType.IsArray || destinationType.GenericTypeArguments.Length == 1)
                return typeof(Mapper<,>).MakeGenericType(new[]
                {
                    MapperMapToArrayGenericTypeArgument(sourceType),
                    MapperMapToArrayGenericTypeArgument(destinationType)
                })
                .GetMethods(BindingFlags.Public | BindingFlags.Static).Single(m =>
                    m.Name == nameof(Mapper<S, D>.ToArray) &&
                    !m.IsGenericMethod &&
                    m.ReturnType == CollectionDestinationType(destinationType) &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == typeof(IEnumerable<>).MakeGenericType(new[]
                    {
                        MapperMapToArrayGenericTypeArgument(sourceType)
                    }));

            return typeof(Mapper<,>).MakeGenericType(new[]
            {
                sourceType.GenericTypeArguments[1],
                destinationType.GenericTypeArguments[1]
            })
            .GetMethods(BindingFlags.Public | BindingFlags.Static).Single(m =>
                m.Name == nameof(Mapper<S, D>.ToDictionary) &&
                m.IsGenericMethod &&
                m.ReturnType.IsGenericType &&
                m.ReturnType.GetGenericTypeDefinition() == typeof(Dictionary<,>) &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType.IsGenericType &&
                m.GetParameters()[0].ParameterType.GenericTypeArguments.Length == 2 &&
                m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            .MakeGenericMethod(new[]
            {
                sourceType.GenericTypeArguments[0],
                destinationType.GenericTypeArguments[0]
            });
        }

        private MethodInfo MapperMapCollectionActionRefMethodInfo(Type sourceType, Type destinationType)
        {
            if (destinationType.IsArray || destinationType.GenericTypeArguments.Length == 1)
                return typeof(Mapper<,>).MakeGenericType(new[]
                {
                    MapperMapToArrayGenericTypeArgument(sourceType),
                    MapperMapToArrayGenericTypeArgument(destinationType)
                })
                .GetMethods(BindingFlags.Public | BindingFlags.Static).Single(m =>
                    m.Name == nameof(Mapper<S, D>.ToArray) &&
                    !m.IsGenericMethod &&
                    m.ReturnType == typeof(void) &&
                    m.GetParameters().Length == 2 &&
                    m.GetParameters()[0].ParameterType == typeof(IEnumerable<>).MakeGenericType(new[]
                    {
                        MapperMapToArrayGenericTypeArgument(sourceType)
                    }) &&
                    m.GetParameters()[1].ParameterType == CollectionDestinationType(destinationType).MakeByRefType());

            return typeof(Mapper<,>).MakeGenericType(new[]
            {
                sourceType.GenericTypeArguments[1],
                destinationType.GenericTypeArguments[1]
            })
            .GetMethods(BindingFlags.Public | BindingFlags.Static).Single(m =>
                m.Name == nameof(Mapper<S, D>.ToDictionary) &&
                m.IsGenericMethod &&
                m.ReturnType == typeof(void) &&
                m.GetParameters().Length == 2 &&
                m.GetParameters()[0].ParameterType.IsGenericType &&
                m.GetParameters()[0].ParameterType.GenericTypeArguments.Length == 2 &&
                m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IDictionary<,>) &&
                m.GetParameters()[1].ParameterType.IsGenericType &&
                m.GetParameters()[1].ParameterType.GenericTypeArguments.Length == 2 &&
                m.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            .MakeGenericMethod(new[]
            {
                sourceType.GenericTypeArguments[0],
                destinationType.GenericTypeArguments[0]
            });
        }

        private void MapperMapCollectionFunc(DestinationNode destinationNode, DestinationNodeMember destinationNodeMember)
        {
            MethodInfo methodInfo = MapperMapCollectionFuncMethodInfo(
                destinationNodeMember.SourceNodeMember.Type,
                destinationNodeMember.Info.Type);

            if (!destinationNodeMember.Info.IsStatic)
                Load(destinationNode);

            Load(destinationNodeMember.SourceNode, destinationNodeMember.SourceNodeMember);
            IL.Emit(OpCodes.Call, methodInfo);

            if (destinationNodeMember.Info.Type != CollectionDestinationType(destinationNodeMember.Info.Type))
                ConvertCollectionToDestinationNodeMemberType(destinationNodeMember);

            IL.EmitSetMemberValue(destinationNodeMember.Info);
        }

        private void MapperMapCollectionActionRef(DestinationNode destinationNode, DestinationNodeMember destinationNodeMember)
        {
            MethodInfo methodInfo = MapperMapCollectionActionRefMethodInfo(
               destinationNodeMember.SourceNodeMember.Type,
               destinationNodeMember.Info.Type);

            if (destinationNodeMember.Info.Type != destinationNodeMember.Local.LocalType)
            {
                ConvertDestinationNodeMemberTypeToCollection(destinationNode, destinationNodeMember);
            }
            else
            {
                Load(destinationNode, destinationNodeMember);
                IL.EmitStloc(destinationNodeMember.Local.LocalIndex);
            }

            Load(destinationNodeMember.SourceNode, destinationNodeMember.SourceNodeMember);
            LoadLocal(destinationNodeMember.Local, true);
            IL.Emit(OpCodes.Call, methodInfo);

            if (!destinationNodeMember.Info.IsStatic)
                Load(destinationNode);
            LoadLocal(destinationNodeMember.Local, false);
            if (destinationNodeMember.Info.Type != CollectionDestinationType(destinationNodeMember.Info.Type))
                ConvertCollectionToDestinationNodeMemberType(destinationNodeMember);

            IL.EmitSetMemberValue(destinationNodeMember.Info);
        }

        private void MapperMapCollection(DestinationNode destinationNode, DestinationNodeMember destinationNodeMember)
        {
            if (Method.ReturnType != typeof(void))
                MapperMapCollectionFunc(destinationNode, destinationNodeMember);
            else
                MapperMapCollectionActionRef(destinationNode, destinationNodeMember);
        }

        private void MapperMapFunc(DestinationNode destinationNode)
        {
            SourceNode sourceNode = destinationNode.SourceNode;
            EnsureDestinationNodePath(destinationNode, sourceNode);

            MethodInfo method = typeof(Mapper<,>).MakeGenericType(new[] { sourceNode.Type, destinationNode.Type })
                .GetMethods(BindingFlags.Public | BindingFlags.Static).First(m =>
                    m.Name == nameof(Mapper<S, D>.Map) &&
                    !m.IsGenericMethod &&
                    m.ReturnType == destinationNode.Type &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == sourceNode.Type);

            if (!destinationNode.IsStatic)
                Load(destinationNode.ParentNode);

            if (destinationNode.Type.IsValueType)
            {
                Load(sourceNode.ParentNode, sourceNode.MemberInfo);
                IL.Emit(OpCodes.Call, method);
            }
            else
            {
                Load(sourceNode.ParentNode, sourceNode.MemberInfo);
                IL.Emit(OpCodes.Call, method);
                IL.EmitSetMemberValue(destinationNode.MemberInfo);
            }

            SetDestinationNode(destinationNode);
        }

        private void MapperMapActionRef(DestinationNode destinationNode)
        {
            SourceNode sourceNode = destinationNode.SourceNode;
            EnsureDestinationNodePath(destinationNode, sourceNode);

            MethodInfo method = typeof(Mapper<,>).MakeGenericType(new[] { sourceNode.Type, destinationNode.Type })
                .GetMethods(BindingFlags.Public | BindingFlags.Static).First(m =>
                    m.Name == nameof(Mapper<S, D>.Map) &&
                    !m.IsGenericMethod &&
                    m.ReturnType == typeof(void) &&
                    m.GetParameters().Length == 2 &&
                    m.GetParameters()[0].ParameterType == sourceNode.Type &&
                    m.GetParameters()[1].ParameterType == destinationNode.Type.MakeByRefType());

            if (!destinationNode.IsStatic)
                Load(destinationNode.ParentNode);
            IL.EmitLoadMemberValue(destinationNode.MemberInfo);
            IL.EmitStloc(destinationNode.NullableLocal != null ? destinationNode.NullableLocal.LocalIndex : destinationNode.Local.LocalIndex);

            Load(sourceNode.ParentNode, sourceNode.MemberInfo);
            LoadLocal(destinationNode.NullableLocal ?? destinationNode.Local, true);
            IL.Emit(OpCodes.Call, method);

            if (!destinationNode.IsStatic)
                Load(destinationNode.ParentNode);
            LoadLocal(destinationNode.NullableLocal ?? destinationNode.Local, false);
            IL.EmitSetMemberValue(destinationNode.MemberInfo);
        }

        private void MapperMap(DestinationNode destinationNode)
        {
            if (Method.ReturnType != typeof(void))
                MapperMapFunc(destinationNode);
            else
                MapperMapActionRef(destinationNode);
        }

        #endregion

        private protected bool MapsNodeMembers(SourceNode sourceNode)
        {
            if (!sourceNode.Load)
                return false;

            List<SourceNode> childNodes = Schema.GetChildNodes(sourceNode, 1, n => n.Load);

            return Schema.DestinationNodes.Exists(n =>
                n.Load &&
                (
                    (n.MembersMapCount != 0 && n.Members.Exists(m => m.Map && m.SourceNode.Name == sourceNode.Name)) ||
                    (n.UseMapper && childNodes.Exists(c => c.Name == n.SourceNode.Name))
                ));
        }

        private protected bool MapsNodesMembers(SourceNode sourceNode)
        {
            if (!sourceNode.Load)
                return false;

            List<SourceNode> childNodes = Schema.GetChildNodes(sourceNode, 0, n => n.Load);

            return Schema.DestinationNodes.Exists(n =>
                n.Load &&
                (
                    (n.MembersMapCount != 0 && n.Members.Exists(m => m.Map &&
                        (
                            m.SourceNode.Name == sourceNode.Name ||
                            childNodes.Exists(c => c.Name == m.SourceNode.Name)
                        ))
                    ) ||
                    (n.UseMapper && childNodes.Exists(c => c.Name == n.SourceNode.Name))
                ));
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
                IL.EmitBrfalse(() =>
                {
                    MapSourceNode(sourceNode);
                });
            }
        }

        private void MapSourceNodeMembers(
           SourceNode sourceNode,
           Action<DestinationNode, List<DestinationNodeMember>> mapDestinationNodeMembers)
        {
            foreach (DestinationNode destinationNode in Schema.DestinationNodes)
            {
                if (!destinationNode.Load)
                    continue;

                List<DestinationNodeMember> destinationNodeMembers =
                    destinationNode.Members.Where(w =>
                        w.SourceNode != null &&
                        w.SourceNode.Name == sourceNode.Name &&
                        w.Map &&
                        w.Status == Status.Successful)
                    .ToList();

                if (destinationNodeMembers.Count == 0)
                    continue;

                mapDestinationNodeMembers(destinationNode, destinationNodeMembers);
            }
        }

        private void MapSourceChildNodes(SourceNode sourceNode, ref List<DestinationNode> destinationNodes)
        {
            List<SourceNode> childNodes = Schema.GetChildNodes(sourceNode, 1, n => n.Load);
            for (int s = 0; s < childNodes.Count; s++)
            {
                List<DestinationNode> mapperMapDestinationNodes =
                    Schema.DestinationNodes.Where(n => n.UseMapper && n.SourceNode.Name == childNodes[s].Name).ToList();

                for (int d = 0; d < mapperMapDestinationNodes.Count; d++)
                {
                    MapperMap(mapperMapDestinationNodes[d]);

                    if (!destinationNodes.Exists(n => n.Name == mapperMapDestinationNodes[d].ParentNode.Name))
                        destinationNodes.Add(mapperMapDestinationNodes[d].ParentNode);
                }

                MapSourceNodes(childNodes[s]);
            }
        }

        private void MapSourceNode(SourceNode sourceNode)
        {
            List<DestinationNode> destinationNodes = new List<DestinationNode>();
            MapSourceNodeMembers(sourceNode, (node, members) =>
            {
                MapDestinationNodeMembers(sourceNode, node, members);
                destinationNodes.Add(node);
            });
            MapSourceChildNodes(sourceNode, ref destinationNodes);
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
                if (Method.ReturnType == typeof(void))
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
                if (Method.ReturnType == typeof(void))
                {
                    IL.EmitLdarg(1);
                    IL.EmitInit(destinationNode.Type);
                }
            }
            else if (destinationNode.IsStatic) { }
            else
            {
                if (Method.ReturnType == typeof(void))
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
            if (destinationNode.Depth == 0 && Method.ReturnType == typeof(void))
            {
                InitIfNull(destinationNode);

                LoadAndSetDestinationNodeMembers(destinationNode, destinationNodeMembers);

                return;
            }

            List<DestinationNodeMember> queueLoadAndSet = new List<DestinationNodeMember>();

            if (destinationNode.Depth != 0 && !destinationNode.IsStatic)
                Load(destinationNode.ParentNode);

            IL.EmitInit(destinationNode.Type);
            destinationNode.Loaded = true;

            foreach (DestinationNodeMember destinationNodeMember in destinationNodeMembers)
            {
                if (destinationNodeMember.Info.IsStatic || destinationNodeMember.IsCollection)
                {
                    queueLoadAndSet.Add(destinationNodeMember);
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

            if (queueLoadAndSet.Count != 0)
                LoadAndSetDestinationNodeMembers(destinationNode, queueLoadAndSet);
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

                if (Method.ReturnType == typeof(void))
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

        public abstract string ViewIL(Action<MapOptions<S, D>> mapOptions = null);
    }
}
