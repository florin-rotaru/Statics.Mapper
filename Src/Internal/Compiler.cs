using Statics.Mapper.Internal.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Statics.Mapper.Internal
{
    internal partial class Compiler(
        Type sourceType,
        Type destinationType,
        MethodType methodType)
    {
        protected Type SourceType { get; set; } = sourceType;
        protected Type DestinationType { get; set; } = destinationType;
        protected MethodType MethodType { get; set; } = methodType;
        protected List<IMapperOptionArguments>? MapOptions { get; set; }
        protected IL IL { get; set; }
        protected Schema Schema { get; set; }

        const char DOT = '.';
        const string Map = nameof(Map);
        const string To = nameof(To);
        const string ToArray = nameof(ToArray);
        const string CreateRange = nameof(CreateRange);

        protected const string HasValue = nameof(HasValue);
        protected const string Value = nameof(Value);

        protected void Evaluate()
        {
            IMapperOptionArguments? mapDestinationTypeAs = MapOptions?.Where(o => o.Name == nameof(MapAsOption) && new MapAsOption(o).DestinationMemberName == string.Empty).LastOrDefault();
            if (mapDestinationTypeAs != null)
                DestinationType = new MapAsOption(mapDestinationTypeAs).Implementation;

            if (!Schema.CanMapTypes(SourceType, DestinationType, MapOptions))
                throw new NotSupportedException($"Mapping from {SourceType.Name} to {DestinationType.Name} not supported.");
        }

        #region Schema

        protected void CreateSchema()
        {
            if (Collections.IsCollection(DestinationType) ||
                MapperTypeInfo.IsBuiltIn(DestinationType) ||
                DestinationType.IsAbstract ||
                DestinationType.IsInterface)
                return;

            Schema = new Schema(SourceType, DestinationType, MapOptions);
        }

        static List<SourceNode> GetDestinationNodeSources(DestinationNode destinationNode)
        {
            List<SourceNode> returnValue = [];

            if (destinationNode.UseMapper)
            {
                if (destinationNode.SourceNode?.Depth != 0 && destinationNode.SourceNode?.ParentNode != null)
                    returnValue.Add(destinationNode.SourceNode.ParentNode);
            }
            else
            {
                foreach (DestinationNodeMember destinationNodeMember in destinationNode.Members)
                    if (destinationNodeMember.Map &&
                        !returnValue.Exists(node => node.Name == destinationNodeMember.SourceNode.Name))
                        returnValue.Add(destinationNodeMember.SourceNode);
            }

            for (int i = 0; i < returnValue.Count; i++)
            {
                SourceNode? parentNode = returnValue[i].ParentNode;

                while (parentNode != null)
                {
                    if (!returnValue.Exists(node => node.Name == parentNode.Name))
                        returnValue.Add(parentNode);

                    parentNode = parentNode.ParentNode;
                }
            }

            return returnValue;
        }

        static List<DestinationNode> GetDistinctNodes(IEnumerable<DestinationNode> destinationNodes)
        {
            List<DestinationNode> distinctNodes = [];

            foreach (DestinationNode destinationNode in destinationNodes)
                if (!distinctNodes.Exists(n => n.Name == destinationNode.Name))
                    distinctNodes.Add(destinationNode);

            return distinctNodes;
        }

        #endregion

        #region Declare Locals

        void DeclareSourceNodeLocals(SourceNode sourceNode)
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
            else if (sourceNode.Type.IsValueType && sourceNode.Depth != 0 && StructRequired(sourceNode))
            {
                sourceNode.Local = IL.DeclareLocal(sourceNode.Type);
            }
        }

        void DeclareSourceNodesLocals(IEnumerable<SourceNode> sourceNodes)
        {
            foreach (SourceNode sourceNode in sourceNodes)
                DeclareSourceNodeLocals(sourceNode);
        }

        void DeclareDestinationNodeLocals(DestinationNode destinationNode)
        {
            if (destinationNode.Local != null ||
                destinationNode.NullableLocal != null)
                return;

            if (destinationNode.Depth != 0)
            {
                if (MethodType == MethodType.Function && destinationNode.UseMapper) { }
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
                if (MethodType == MethodType.ActionRef)
                {
                    if (destinationNode.NullableUnderlyingType != null ||
                        destinationNode.TypeAdapter != null)
                        destinationNode.Local = IL.DeclareLocal(
                            destinationNode.TypeAdapter ??
                            destinationNode.NullableUnderlyingType);
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

        bool UseDestinationLocals() =>
            !(
                Collections.IsCollection(DestinationType) ||
                MapperTypeInfo.IsBuiltIn(DestinationType) ||
                DestinationType.IsAbstract ||
                DestinationType.IsInterface
            );

        protected void DeclareLocals()
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

        protected void InitDestinationLocals()
        {
            if (!UseDestinationLocals())
                return;

            Schema.ForEachDestinationNode(n =>
                n.Load &&
                n.Type.IsValueType &&
                !n.UseMapper &&
                n.Local != null &&
                n.Depth != 0,
                n => IL.EmitInit(n.Local));
        }

        #endregion

        #region Load

        void Load(SourceNode sourceNode)
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

            MapperTypeMemberInfo memberInfo =
                sourceNode.ParentNode.Members.First(w => w.Name == MapperTypeInfo.GetName(sourceNode.Name));

            if (memberInfo.IsStatic)
            {
                IL.EmitLoadMemberValue(memberInfo);
                return;
            }

            string[] segments = sourceNode.Name.Split(DOT, StringSplitOptions.RemoveEmptyEntries);

            SourceNode parentNode = sourceNode.ParentNode;
            while (parentNode != null)
            {
                if (parentNode.Local != null)
                    LoadLocal(parentNode.Local, true);
                else if (parentNode.IsStatic)
                    IL.EmitLoadMemberValue(parentNode.Member);

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

        void Load(SourceNode sourceNode, MapperTypeMemberInfo sourceNodeMember)
        {
            if (!sourceNodeMember.IsStatic)
                Load(sourceNode);

            IL.EmitLoadMemberValue(sourceNodeMember);
        }

        void LoadLocal(LocalBuilder local, bool loadAddress)
        {
            if (loadAddress)
                IL.EmitLdloca(local.LocalIndex);
            else
                IL.EmitLdloc(local.LocalIndex);
        }

        void Load(DestinationNode destinationNode)
        {
            if (destinationNode.Depth == 0)
            {
                if (MethodType == MethodType.ActionRef)
                {
                    if (destinationNode.NullableUnderlyingType != null ||
                        destinationNode.TypeAdapter != null)
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

            MapperTypeMemberInfo memberInfo =
                destinationNode.ParentNode.Members.First(w => w.Info.Name == MapperTypeInfo.GetName(destinationNode.Name)).Info;

            if (memberInfo.IsStatic)
            {
                IL.EmitLoadMemberValue(memberInfo);
                return;
            }

            string[] segments = destinationNode.Name.Split(DOT, StringSplitOptions.RemoveEmptyEntries);

            DestinationNode parentNode = destinationNode.ParentNode;
            while (parentNode != null)
            {
                if (parentNode.Local != null)
                    LoadLocal(parentNode.Local, parentNode.Local.LocalType.IsValueType);
                else if (parentNode.IsStatic)
                    IL.EmitLoadMemberValue(parentNode.Member);

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

        void Load(DestinationNode destinationNode, MapperTypeMemberInfo destinationNodeMember)
        {
            if (!destinationNodeMember.IsStatic)
                Load(destinationNode);

            IL.EmitLoadMemberValue(destinationNodeMember);
        }

        #endregion

        #region Set

        void StoreDestinationRootNode()
        {
            if (MethodType == MethodType.ActionRef)
                return;

            if (Schema.DestinationRootNode.NullableUnderlyingType != null) { }
            else if (Schema.DestinationRootNode.Type.IsValueType) { }
            else if (Schema.DestinationRootNode.IsStatic) { }
            else
                IL.EmitStloc(Schema.DestinationRootNode.Local.LocalIndex);
        }

        void SetDestinationNode(DestinationNode destinationNode)
        {
            if (destinationNode.ParentNode == null ||
                !destinationNode.Type.IsValueType)
                return;

            if (destinationNode.UseMapper && MethodType == MethodType.Function)
            {
                IL.EmitSetMemberValue(destinationNode.Member);
                return;
            }

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

            if (destinationNode.UseMapper)
            {
                if (destinationNode.NullableUnderlyingType != null)
                    LoadLocal(destinationNode.NullableLocal, false);
                else
                    LoadLocal(destinationNode.Local, false);
            }
            else if (destinationNode.TypeAdapter != null)
            {
                LoadLocal(destinationNode.Local, true);
                IL.Emit(OpCodes.Call, destinationNode.TypeAdapter.GetMethod(TypeAdapters.ToMethodName(destinationNode.Type)));
            }
            else
            {
                LoadLocal(destinationNode.Local, false);
            }

            if (destinationNode.NullableUnderlyingType != null && !destinationNode.UseMapper)
                IL.Emit(OpCodes.Newobj, destinationNode.Type.GetConstructor([destinationNode.NullableUnderlyingType]));

            IL.EmitSetMemberValue(destinationNode.Member);
        }

        void SetDestinationNodes(List<DestinationNode> destinationNodes)
        {
            List<DestinationNode> nodes = GetDistinctNodes(destinationNodes);

            for (int i = 0; i < nodes.Count; i++)
            {
                DestinationNode parentNode = nodes[i].ParentNode;
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

        void SetDestinationNodeMembers(
            DestinationNode destinationNode,
            IEnumerable<DestinationNodeMember> destinationNodeMembers)
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

        void LoadAndSetDestinationNodeMembers(
            DestinationNode destinationNode,
            IEnumerable<DestinationNodeMember> destinationNodeMembers)
        {
            foreach (DestinationNodeMember destinationNodeMember in destinationNodeMembers)
                if (!destinationNodeMember.IsCollection)
                    IL.EmitLoadAndSetValue(
                        () =>
                        {
                            if (!destinationNodeMember.Info.IsStatic)
                                Load(destinationNode);

                            Load(destinationNodeMember.SourceNode, destinationNodeMember.SourceNodeMember);
                        },
                        destinationNodeMember.SourceNodeMember,
                        destinationNodeMember.Info);

            foreach (DestinationNodeMember destinationNodeMember in destinationNodeMembers)
                if (destinationNodeMember.IsCollection)
                    MapCollection(destinationNode, destinationNodeMember);
        }

        #endregion

        protected bool MapsNodeMembers(SourceNode sourceNode)
        {
            if (!sourceNode.Load)
                return false;

            return Schema.DestinationNodes.Exists(n =>
                n.Load &&
                n.MembersMapCount != 0 &&
                n.Members.Exists(m => m.Map && m.SourceNode.Name == sourceNode.Name));
        }

        protected bool MapsNodesMembers(SourceNode sourceNode)
        {
            if (!sourceNode.Load)
                return false;

            List<SourceNode> childNodes = Schema.GetChildNodes(sourceNode, n => n.Load, 0);

            return Schema.DestinationNodes.Exists(n =>
                n.Load &&
                (
                    (n.MembersMapCount != 0 && n.Members.Exists(
                        m => m.Map &&
                        (
                            m.SourceNode.Name == sourceNode.Name ||
                            childNodes.Exists(c => c.Name == m.SourceNode.Name)
                        ))
                    ) ||
                    (n.UseMapper && childNodes.Exists(c => c.Name == n.SourceNode.Name))
                ));
        }

        protected bool StructRequired(SourceNode sourceNode)
        {
            List<SourceNode> childNodes = Schema.GetChildNodes(sourceNode, (n) => n.Load, 1);

            return childNodes.Exists(n => !n.IsStatic) ||
                Schema.DestinationNodes.Exists(n =>
                    n.Load &&
                    n.MembersMapCount != 0 && n.Members.Exists(m =>
                        m.Map &&
                        m.SourceNode.Name == sourceNode.Name &&
                        !m.SourceNodeMember.IsStatic));
        }

        void MapDestinationNodeMembers(
            SourceNode sourceNode,
            DestinationNode destinationNode,
            IEnumerable<DestinationNodeMember> destinationNodeMembers)
        {
            EnsureDestinationNodePath(destinationNode, sourceNode);
            SetDestinationNodeMembers(destinationNode, destinationNodeMembers);
        }

        protected void MapSourceNodes(SourceNode sourceNode)
        {
            if (!MapsNodesMembers(sourceNode))
                return;

            if (sourceNode.Depth == 0)
            {
                MapSourceNode(sourceNode);
            }
            else if (sourceNode.NullableUnderlyingType != null)
            {
                Load(sourceNode.ParentNode, sourceNode.Member);
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
                    Load(sourceNode.ParentNode, sourceNode.Member);
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

        void MapSourceChildNodes(SourceNode sourceNode, ref List<DestinationNode> destinationNodes)
        {
            IEnumerable<SourceNode> childNodes = Schema.GetChildNodes(sourceNode, n => n.Load, 1);
            foreach (SourceNode childNode in childNodes)
            {
                List<DestinationNode> mapperMapDestinationNodes =
                    Schema.DestinationNodes.Where(n => n.UseMapper && n.SourceNode.Name == childNode.Name).ToList();

                for (int d = 0; d < mapperMapDestinationNodes.Count; d++)
                {
                    MapperMap(mapperMapDestinationNodes[d]);

                    if (!destinationNodes.Exists(n => n.Name == mapperMapDestinationNodes[d].ParentNode.Name))
                        destinationNodes.Add(mapperMapDestinationNodes[d].ParentNode);
                }

                MapSourceNodes(childNode);
            }
        }

        void MapSourceNode(SourceNode sourceNode)
        {
            List<DestinationNode> destinationNodes = [];

            Schema.ForEachDestinationNode(
                n => n.Load,
                n =>
                {
                    List<DestinationNodeMember> members = Schema.GetDestinationNodeMembers(n, m =>
                        m.SourceNode != null &&
                        m.SourceNode.Name == sourceNode.Name &&
                        m.Map &&
                        m.Status == Status.Successful).ToList();

                    if (members.Count != 0)
                    {
                        MapDestinationNodeMembers(sourceNode, n, members);
                        destinationNodes.Add(n);
                    }
                });

            MapSourceChildNodes(sourceNode, ref destinationNodes);
            SetDestinationNodes(destinationNodes);
        }

        protected void Init(DestinationNode destinationNode)
        {
            if (destinationNode.Depth != 0)
            {
                if (destinationNode.NullableUnderlyingType != null) { }
                else if (destinationNode.Type.IsValueType) { }
                else if (destinationNode.IsStatic)
                {
                    IL.EmitInit(destinationNode.Type);
                    IL.EmitSetMemberValue(destinationNode.Member);
                }
                else
                {
                    Load(destinationNode.ParentNode);
                    IL.EmitInit(destinationNode.Type);
                    IL.EmitSetMemberValue(destinationNode.Member);
                }

                destinationNode.Loaded = true;

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
                    IL.EmitStore(destinationNode.Type);
                }
                else
                {
                    IL.EmitInit(destinationNode.Local.LocalType);
                    IL.EmitStloc(destinationNode.Local.LocalIndex);
                }
            }

            destinationNode.Loaded = true;
        }

        protected void InitIfNull(DestinationNode destinationNode)
        {
            if (destinationNode.Depth != 0)
            {
                if (destinationNode.NullableUnderlyingType != null) { }
                else if (destinationNode.Type.IsValueType) { }
                else if (destinationNode.IsStatic)
                {
                    IL.EmitLoadMemberValue(destinationNode.Member);
                    IL.EmitBrtrue_s(() => Init(destinationNode));
                }
                else
                {
                    Load(destinationNode);
                    IL.EmitBrtrue_s(() => Init(destinationNode));
                }

                destinationNode.Loaded = true;

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

            destinationNode.Loaded = true;
        }

        /// <summary>
        /// Avoid using valueType destinationNode
        /// </summary>
        /// <param name="destinationNode"></param>
        /// <param name="destinationNodeMembers"></param>
        void InitAndSetDestinationNodeMembers(
            DestinationNode destinationNode,
            IEnumerable<DestinationNodeMember> destinationNodeMembers)
        {
            if (destinationNode.Depth == 0 && MethodType == MethodType.ActionRef)
            {
                InitIfNull(destinationNode);

                LoadAndSetDestinationNodeMembers(destinationNode, destinationNodeMembers);

                return;
            }

            List<DestinationNodeMember> loadAndSetQueue = new();

            if (destinationNode.Depth != 0 && !destinationNode.IsStatic)
                Load(destinationNode.ParentNode);

            IL.EmitInit(destinationNode.Type);
            destinationNode.Loaded = true;

            foreach (DestinationNodeMember destinationNodeMember in destinationNodeMembers)
            {
                if (destinationNodeMember.Info.IsStatic ||
                    destinationNodeMember.IsCollection)
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
                IL.EmitSetMemberValue(destinationNode.Member);
            else
                StoreDestinationRootNode();

            if (loadAndSetQueue.Count != 0)
                LoadAndSetDestinationNodeMembers(destinationNode, loadAndSetQueue);
        }

        void EnsureDestinationNodePath(DestinationNode destinationNode, SourceNode sourceNode)
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

                if (loaded)
                    continue;

                if (MethodType == MethodType.ActionRef)
                    EnsureDestinationNode(destinationNodes[n]);
                else
                    Init(destinationNodes[n]);
            }

            sourceNode.DestinationNodes.Add(destinationNode);
        }

        void EnsureDestinationNode(DestinationNode destinationNode)
        {
            if (destinationNode.Depth == 0)
            {
                InitIfNull(destinationNode);
                return;
            }

            if (destinationNode.NullableUnderlyingType != null)
            {
                Load(destinationNode.ParentNode, destinationNode.Member);
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
                Load(destinationNode.ParentNode, destinationNode.Member);
                IL.EmitStloc(destinationNode.Local.LocalIndex);
            }
            else
            {
                InitIfNull(destinationNode);
            }

            destinationNode.Loaded = true;
        }

        void MapperMapFunc(DestinationNode destinationNode)
        {
            SourceNode sourceNode = destinationNode.SourceNode;
            EnsureDestinationNodePath(destinationNode, sourceNode);

            MethodInfo method = typeof(Mapper<,>).MakeGenericType(new[] { sourceNode.Type, destinationNode.Type })
                .GetMethods(BindingFlags.Public | BindingFlags.Static).First(m =>
                    m.Name == nameof(Mapper<Type, Type>.Map) &&
                    !m.IsGenericMethod &&
                    m.ReturnType == destinationNode.Type &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == sourceNode.Type);

            if (!destinationNode.IsStatic)
                Load(destinationNode.ParentNode);

            if (destinationNode.Type.IsValueType)
            {
                Load(sourceNode.ParentNode, sourceNode.Member);
                IL.Emit(OpCodes.Call, method);
            }
            else
            {
                Load(sourceNode.ParentNode, sourceNode.Member);
                IL.Emit(OpCodes.Call, method);
                IL.EmitSetMemberValue(destinationNode.Member);
            }

            SetDestinationNode(destinationNode);
        }

        void MapperMapActionRef(DestinationNode destinationNode)
        {
            SourceNode sourceNode = destinationNode.SourceNode;
            EnsureDestinationNodePath(destinationNode, sourceNode);

            MethodInfo method = typeof(Mapper<,>).MakeGenericType(new[] { sourceNode.Type, destinationNode.Type })
                .GetMethods(BindingFlags.Public | BindingFlags.Static).First(m =>
                    m.Name == nameof(Mapper<Type, Type>.Map) &&
                    !m.IsGenericMethod &&
                    m.ReturnType == typeof(void) &&
                    m.GetParameters().Length == 2 &&
                    m.GetParameters()[0].ParameterType == sourceNode.Type &&
                    m.GetParameters()[1].ParameterType == destinationNode.Type.MakeByRefType());

            if (!destinationNode.IsStatic)
                Load(destinationNode.ParentNode);
            IL.EmitLoadMemberValue(destinationNode.Member);
            IL.EmitStloc(destinationNode.NullableLocal != null ? destinationNode.NullableLocal.LocalIndex : destinationNode.Local.LocalIndex);

            Load(sourceNode.ParentNode, sourceNode.Member);
            LoadLocal(destinationNode.NullableLocal ?? destinationNode.Local, true);
            IL.Emit(OpCodes.Call, method);

            if (!destinationNode.IsStatic)
                Load(destinationNode.ParentNode);
            LoadLocal(destinationNode.NullableLocal ?? destinationNode.Local, false);
            IL.EmitSetMemberValue(destinationNode.Member);
        }

        void MapperMap(DestinationNode destinationNode)
        {
            if (MethodType == MethodType.Function)
                MapperMapFunc(destinationNode);
            else
                MapperMapActionRef(destinationNode);
        }

    }
}
