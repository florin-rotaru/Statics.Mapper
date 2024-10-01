using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Statics.Mapper.Internal
{
    internal class FuncCompiler : Compiler
    {
        protected DynamicMethod CompiledMethod { get; set; }

        public FuncCompiler(
            Type sourceType,
            Type destinationType,
            MethodType methodType) :
            base(sourceType, destinationType, methodType)
        {
            CompiledMethod = new DynamicMethod(
                $"{nameof(Statics)}{Guid.NewGuid():N}",
                DestinationType,
                [SourceType],
                typeof(Mapper<,>).MakeGenericType(SourceType, DestinationType).Module,
                skipVisibility: true);
            IL = new IL(CompiledMethod.GetILGenerator());
        }

        void SetAndReturnDestinationRootNode()
        {
            if (Schema.SourceRootNode.NullableUnderlyingType != null &&
                StructRequired(Schema.SourceRootNode))
            {
                IL.EmitLdarga(0);
                IL.Emit(OpCodes.Call, Schema.SourceRootNode.Type.GetProperty(Value).GetGetMethod());
                IL.EmitStloc(Schema.SourceRootNode.Local.LocalIndex);
            }

            if (Schema.DestinationRootNode.NullableUnderlyingType != null)
                Init(Schema.DestinationRootNode);

            MapSourceNodes(Schema.SourceRootNode);

            if (Schema.DestinationRootNode.NullableUnderlyingType == null &&
                Schema.DestinationNodes.Exists(node => node.Load && node.Depth != 0))
                InitIfNull(Schema.DestinationRootNode);

            ReturnDestinationRootNode();
        }

        void ReturnNewInstanceOrDefault()
        {
            DestinationNode destinationNode = Schema.DestinationRootNode;
            SourceNode sourceNode = Schema.SourceRootNode;

            if (destinationNode.NullableUnderlyingType != null)
            {
                destinationNode.Local = IL.DeclareLocal(destinationNode.NullableUnderlyingType);
                destinationNode.NullableLocal = IL.DeclareLocal(destinationNode.Type);

                IL.EmitInit(destinationNode.NullableLocal);
            }
            else
            {
                destinationNode.Local = IL.DeclareLocal(destinationNode.Type);

                if (destinationNode.Type.IsValueType)
                {
                    IL.EmitInit(destinationNode.Local);
                }
                else
                {
                    IL.Emit(OpCodes.Ldnull);
                    IL.EmitStoreLocal(destinationNode.Local);
                }
            }

            void newInstance()
            {
                if (destinationNode.NullableUnderlyingType != null)
                {
                    IL.EmitLoadLocal(destinationNode.NullableLocal, true);
                    IL.EmitInit(destinationNode.Local);
                    IL.EmitLoadLocal(destinationNode.Local, false);
                    IL.Emit(OpCodes.Call, destinationNode.Type.GetConstructor([destinationNode.NullableUnderlyingType]));
                }
                else if (!destinationNode.Type.IsValueType)
                {
                    IL.EmitInit(destinationNode.Local);
                }
            }

            void loadDestination()
            {
                if (destinationNode.NullableUnderlyingType != null)
                    IL.EmitLoadLocal(destinationNode.NullableLocal, false);
                else
                    IL.EmitLoadLocal(destinationNode.Local, false);
            }

            if (sourceNode.NullableUnderlyingType != null)
            {
                IL.EmitLoadArgument(sourceNode.Type, 0);
                IL.Emit(OpCodes.Call, sourceNode.Type.GetProperty(HasValue).GetGetMethod());
                IL.EmitBrfalse_s(newInstance);

                loadDestination();
            }
            else if (sourceNode.Type.IsValueType)
            {
                newInstance();

                loadDestination();
            }
            else
            {
                IL.EmitLoadArgument(sourceNode.Type, 0);
                IL.EmitBrfalse_s(newInstance);

                loadDestination();
            }

            IL.Emit(OpCodes.Ret);
        }

        void SetDefaultDestinationRootNode()
        {
            bool setNewDestinationRootNodeInstance = false;

            SourceNode sourceNode = Schema.SourceRootNode;
            while (sourceNode != null)
            {
                if (sourceNode.NullableUnderlyingType != null ||
                    !sourceNode.Type.IsValueType)
                    break;

                if (MapsNodeMembers(sourceNode))
                {
                    setNewDestinationRootNodeInstance = true;
                    break;
                }

                sourceNode =
                    Schema.GetChildNodes(sourceNode, sn => sn.Load, 1).FirstOrDefault();
            }

            if (Schema.DestinationRootNode.NullableUnderlyingType != null)
            {
                IL.EmitLdloca(Schema.DestinationRootNode.NullableLocal.LocalIndex);
                IL.EmitInit(Schema.DestinationRootNode.NullableLocal.LocalType);
            }
            else if (Schema.DestinationRootNode.Type.IsValueType)
            {
                IL.EmitLdloca(Schema.DestinationRootNode.Local.LocalIndex);
                IL.EmitInit(Schema.DestinationRootNode.Local.LocalType);
            }
            else
            {
                if (!setNewDestinationRootNodeInstance)
                {
                    IL.Emit(OpCodes.Ldnull);
                    IL.EmitStloc(Schema.DestinationRootNode.Local.LocalIndex);
                }
            }
        }

        void ReturnDefault()
        {
            if (Schema.DestinationRootNode.NullableUnderlyingType != null)
            {
                if (Schema.DestinationRootNode.NullableLocal != null)
                    IL.EmitLdloc(Schema.DestinationRootNode.NullableLocal.LocalIndex);
                else
                    IL.Emit(OpCodes.Ldnull);
            }
            else
            {
                if (Schema.DestinationRootNode.Local != null)
                {
                    IL.EmitLdloc(Schema.DestinationRootNode.Local.LocalIndex);
                }
                else if (Schema.DestinationRootNode.Type.IsValueType)
                {
                    Schema.DestinationRootNode.Local = IL.DeclareLocal(Schema.DestinationRootNode.Type);
                    IL.EmitLdloca(Schema.DestinationRootNode.Local.LocalIndex);
                    IL.EmitInit(Schema.DestinationRootNode.Type);
                    IL.EmitLdloc(Schema.DestinationRootNode.Local.LocalIndex);
                }
                else
                {
                    IL.Emit(OpCodes.Ldnull);
                }
            }

            IL.Emit(OpCodes.Ret);
        }

        void ReturnDestinationRootNode()
        {
            if (Schema.DestinationRootNode.NullableUnderlyingType != null)
            {
                IL.EmitLdloca(Schema.DestinationRootNode.NullableLocal.LocalIndex);

                if (Schema.DestinationRootNode.TypeAdapter != null)
                {
                    IL.EmitLdloca(Schema.DestinationRootNode.Local.LocalIndex);
                    IL.Emit(OpCodes.Call, Schema.DestinationRootNode.TypeAdapter.GetMethod(TypeAdapters.ToMethodName(Schema.DestinationRootNode.Type)));
                }
                else
                {
                    IL.EmitLdloc(Schema.DestinationRootNode.Local.LocalIndex);
                }

                IL.Emit(OpCodes.Call, Schema.DestinationRootNode.Type.GetConstructor([Schema.DestinationRootNode.NullableUnderlyingType]));
                IL.EmitLdloc(Schema.DestinationRootNode.NullableLocal.LocalIndex);
            }
            else
            {
                if (Schema.DestinationRootNode.TypeAdapter != null)
                {
                    IL.EmitLdloca(Schema.DestinationRootNode.Local.LocalIndex);
                    IL.Emit(OpCodes.Call, Schema.DestinationRootNode.TypeAdapter.GetMethod(TypeAdapters.ToMethodName(Schema.DestinationRootNode.Type)));
                }
                else
                {
                    IL.EmitLdloc(Schema.DestinationRootNode.Local.LocalIndex);
                }
            }

            IL.Emit(OpCodes.Ret);
        }

        void SetAndReturnFromToBuiltIn()
        {
            IL.EmitLdarg(0);
            IL.EmitConvert(SourceType, DestinationType);
            IL.Emit(OpCodes.Ret);
        }

        void CreateBody()
        {
            #region Destination: Collection

            if (Collections.IsCollection(DestinationType))
            {
                MapCollection(SourceType, DestinationType);

                return;
            }

            #endregion

            #region Destination: Abstract / Interface

            if (DestinationType.IsAbstract || DestinationType.IsInterface)
            {
                IL.EmitLdarg(0);
                IL.EmitConvert(SourceType, DestinationType);
                IL.Emit(OpCodes.Ret);

                return;
            }

            #endregion

            #region Source: BuiltIn

            if (MapperTypeInfo.IsBuiltIn(SourceType))
            {
                SetAndReturnFromToBuiltIn();

                return;
            }

            #endregion

            #region Return default

            if (!Schema.DestinationRootNode.Load)
            {
                ReturnNewInstanceOrDefault();
                return;
            }

            #endregion

            #region Source: Nullable

            if (Schema.SourceRootNode.NullableUnderlyingType != null)
            {
                SetDefaultDestinationRootNode();

                IL.EmitLdarga(0);
                IL.Emit(OpCodes.Call, Schema.SourceRootNode.Type.GetProperty(HasValue).GetGetMethod());

                IL.EmitBrtrue_s(() => ReturnDefault());

                SetAndReturnDestinationRootNode();

                return;
            }

            #endregion

            #region Source: Struct

            if (Schema.SourceRootNode.Type.IsValueType)
            {
                SetDefaultDestinationRootNode();

                SetAndReturnDestinationRootNode();

                return;
            }

            #endregion

            #region Source: Class

            SetDefaultDestinationRootNode();

            IL.EmitLdarg(0);
            IL.EmitBrtrue_s(() => ReturnDefault());

            SetAndReturnDestinationRootNode();

            #endregion
        }

        DynamicMethod CompileMethod()
        {
            Evaluate();

            CreateSchema();
            DeclareLocals();
            InitDestinationLocals();
            CreateBody();

            return CompiledMethod;
        }

        public DynamicMethod Compile(List<IMapperOptionArguments>? mapOptions = null)
        {
            MapOptions = mapOptions;
            return CompileMethod();
        }

        public string ViewIL(List<IMapperOptionArguments>? mapOptions = null)
        {
            MapOptions = mapOptions;
            IL = new IL(CompiledMethod.GetILGenerator(), true);
            CompileMethod();
            return IL.GetLog().ToString();
        }
    }
}
