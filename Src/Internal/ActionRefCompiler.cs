using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Statics.Mapper.Internal
{
    internal class ActionRefCompiler : Compiler
    {
        protected DynamicMethod CompiledMethod { get; set; }

        public ActionRefCompiler(
            Type sourceType,
            Type destinationType,
            MethodType methodType) :
            base(sourceType, destinationType, methodType)
        {
            CompiledMethod = new DynamicMethod(
                $"{nameof(Statics)}{Guid.NewGuid():N}",
                null,
                [SourceType, DestinationType.MakeByRefType()],
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
                Schema.DestinationNodes.Any(node => node.Load && node.Depth != 0))
                InitIfNull(Schema.DestinationRootNode);

            ReturnDestinationRootNode();
        }

        void ReturnNewInstanceOrDefault()
        {
            SourceNode sourceNode = Schema.SourceRootNode;
            DestinationNode destinationNode = Schema.DestinationRootNode;

            void newInstance()
            {
                if (destinationNode.NullableUnderlyingType != null)
                {
                    destinationNode.Local = IL.DeclareLocal(destinationNode.NullableUnderlyingType);

                    IL.EmitLoadArgument(destinationNode.Type, 1, true);
                    IL.EmitInit(destinationNode.Local);
                    IL.EmitLoadLocal(destinationNode.Local, false);
                    IL.Emit(OpCodes.Newobj, destinationNode.Type.GetConstructor([destinationNode.NullableUnderlyingType]));
                    IL.EmitStore(destinationNode.Type);
                }
                else
                {
                    IL.EmitLoadArgument(destinationNode.Type, 1, true);
                    IL.EmitInit(destinationNode.Type);
                    IL.EmitStore(destinationNode.Type);
                }
            }

            void defaultInstance()
            {
                if (destinationNode.Type.IsValueType)
                {
                    IL.EmitLoadArgument(destinationNode.Type, 1, true);
                    IL.EmitInit(DestinationType);
                }
                else
                {
                    IL.EmitLoadArgument(destinationNode.Type, 1, true);
                    IL.Emit(OpCodes.Ldnull);
                    IL.EmitStore(destinationNode.Type);
                }
            }

            if (sourceNode.NullableUnderlyingType != null)
            {
                IL.EmitLoadArgument(sourceNode.Type, 0);
                IL.Emit(OpCodes.Call, sourceNode.Type.GetProperty(HasValue).GetGetMethod());
                IL.EmitBrtrue_s(
                    () =>
                    {
                        defaultInstance();
                        IL.Emit(OpCodes.Ret);
                    });

                if (destinationNode.NullableUnderlyingType != null || !destinationNode.Type.IsValueType)
                {
                    IL.EmitLoadArgument(destinationNode.Type, 1, true);
                    IL.Emit(OpCodes.Ldind_Ref);
                    IL.EmitBrtrue_s(newInstance);
                }
            }
            else if (sourceNode.Type.IsValueType)
            {
                if (destinationNode.NullableUnderlyingType != null || !destinationNode.Type.IsValueType)
                {
                    IL.EmitLoadArgument(destinationNode.Type, 1, true);
                    IL.Emit(OpCodes.Ldind_Ref);
                    IL.EmitBrtrue_s(newInstance);
                }
            }
            else
            {
                IL.EmitLoadArgument(sourceNode.Type, 0);
                IL.EmitBrtrue_s(
                    () =>
                    {
                        defaultInstance();
                        IL.Emit(OpCodes.Ret);
                    });

                if (destinationNode.NullableUnderlyingType != null)
                {
                    IL.EmitLoadArgument(destinationNode.Type, 1, true);
                    IL.Emit(OpCodes.Call, destinationNode.Type.GetProperty(HasValue).GetGetMethod());
                    IL.EmitBrtrue_s(newInstance);
                }
                else if (!destinationNode.Type.IsValueType)
                {
                    IL.EmitLoadArgument(destinationNode.Type, 1, true);
                    IL.EmitBrfalse_s(newInstance);
                }
            }

            IL.Emit(OpCodes.Ret);
        }

        void ReturnDefault()
        {
            if (Schema.DestinationRootNode.Type.IsValueType)
            {
                IL.EmitLdarg(1);
                IL.EmitInit(Schema.DestinationRootNode.Type);
            }
            else
            {
                IL.EmitLdarg(1);
                IL.Emit(OpCodes.Ldnull);
                IL.EmitStore(Schema.DestinationRootNode.Type);
            }

            IL.Emit(OpCodes.Ret);
        }

        void ReturnDestinationRootNode()
        {
            if (Schema.DestinationRootNode.NullableUnderlyingType != null)
            {
                IL.EmitLdarg(1);

                if (Schema.DestinationRootNode.TypeAdapter != null)
                {
                    IL.EmitLdloca(Schema.DestinationRootNode.Local.LocalIndex);
                    IL.Emit(OpCodes.Call, Schema.DestinationRootNode.TypeAdapter.GetMethod(TypeAdapters.ToMethodName(Schema.DestinationRootNode.Type)));
                }
                else
                {
                    IL.EmitLdloc(Schema.DestinationRootNode.Local.LocalIndex);
                }

                IL.Emit(OpCodes.Newobj, Schema.DestinationRootNode.Type.GetConstructor(new Type[] { Schema.DestinationRootNode.NullableUnderlyingType }));
                IL.Emit(OpCodes.Stobj, Schema.DestinationRootNode.Type);
            }
            else if (Schema.DestinationRootNode.TypeAdapter != null)
            {
                IL.EmitLdarg(1);
                IL.EmitLdloca(Schema.DestinationRootNode.Local.LocalIndex);
                IL.Emit(OpCodes.Call, Schema.DestinationRootNode.TypeAdapter.GetMethod(TypeAdapters.ToMethodName(Schema.DestinationRootNode.Type)));
                IL.Emit(OpCodes.Stobj, Schema.DestinationRootNode.TypeAdapter);
            }

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
                IL.EmitLdarg(1);
                IL.EmitLdarg(0);
                IL.EmitStore(DestinationType);

                IL.Emit(OpCodes.Ret);

                return;
            }

            #endregion

            #region Source: BuiltIn

            if (MapperTypeInfo.IsBuiltIn(SourceType))
            {
                IL.EmitLdarg(1);
                IL.EmitLdarg(0);
                IL.EmitConvert(SourceType, DestinationType);
                IL.EmitStore(DestinationType);

                IL.Emit(OpCodes.Ret);

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
                IL.EmitLdarga(0);
                IL.Emit(OpCodes.Call, Schema.SourceRootNode.Type.GetProperty(HasValue).GetGetMethod());
                IL.EmitBrtrue_s(ReturnDefault);

                SetAndReturnDestinationRootNode();

                return;
            }

            #endregion

            #region Source: Struct

            if (Schema.SourceRootNode.Type.IsValueType)
            {
                SetAndReturnDestinationRootNode();

                return;
            }

            #endregion

            #region Source: Class

            IL.EmitLdarg(0);
            IL.EmitBrtrue_s(ReturnDefault);

            SetAndReturnDestinationRootNode();

            #endregion
        }

        DynamicMethod CompileMethod()
        {
            Evaluate();

            CreateSchema();
            DeclareLocals();
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
