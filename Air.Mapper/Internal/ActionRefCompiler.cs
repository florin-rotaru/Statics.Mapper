using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Air.Mapper.Internal
{
    internal class ActionRefCompiler : Compiler
    {
        public ActionRefCompiler(Type sourceType, Type destinationType, MethodType methodType, IEnumerable<IMapOption> mapOptions) :
            base(sourceType, destinationType, methodType, mapOptions)
        { }

        private protected DynamicMethod Method { get; set; }

        private void SetAndReturnDestinationRootNode()
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

        private void ReturnNewInstanceOrDefault()
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
                    IL.Emit(OpCodes.Newobj, destinationNode.Type.GetConstructor(new Type[] { destinationNode.NullableUnderlyingType }));
                    IL.EmitStore(destinationNode.Type);
                }
                else
                {
                    IL.EmitLoadArgument(destinationNode.Type, 1, true);
                    IL.EmitInit(destinationNode.Type);
                    IL.Emit(OpCodes.Stind_Ref);
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
                    IL.Emit(OpCodes.Stind_Ref);
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
                    IL.EmitBrtrue_s(() => newInstance());
                }
            }
            else if (sourceNode.Type.IsValueType)
            {
                if (destinationNode.NullableUnderlyingType != null || !destinationNode.Type.IsValueType)
                {
                    IL.EmitLoadArgument(destinationNode.Type, 1, true);
                    IL.Emit(OpCodes.Ldind_Ref);
                    IL.EmitBrtrue_s(() => newInstance());
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
                    IL.EmitBrtrue_s(() => newInstance());
                }
                else if (!destinationNode.Type.IsValueType)
                {
                    IL.EmitLoadArgument(destinationNode.Type, 1, true);
                    IL.EmitBrfalse_s(() => newInstance());
                }
            }

            IL.Emit(OpCodes.Ret);
        }

        private void ReturnDefault()
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
                IL.Emit(OpCodes.Stind_Ref);
            }

            IL.Emit(OpCodes.Ret);
        }

        private void ReturnDestinationRootNode()
        {
            if (Schema.DestinationRootNode.NullableUnderlyingType != null)
            {
                IL.EmitLdarg(1);

                if (Schema.DestinationRootNode.IsKeyValuePair)
                {
                    IL.EmitLdloca(Schema.DestinationRootNode.Local.LocalIndex);
                    IL.Emit(OpCodes.Call, Schema.DestinationRootNode.Type.GetMethod(nameof(KeyValue<Type, Type>.ToKeyValuePair)));
                }
                else
                {
                    IL.EmitLdloc(Schema.DestinationRootNode.Local.LocalIndex);
                }

                IL.Emit(OpCodes.Newobj, Schema.DestinationRootNode.Type.GetConstructor(new Type[] { Schema.DestinationRootNode.NullableUnderlyingType }));
                IL.Emit(OpCodes.Stobj, Schema.DestinationRootNode.Type);
            }
            else if (Schema.DestinationRootNode.IsKeyValuePair)
            {
                IL.EmitLdarg(1);
                IL.EmitLdloca(Schema.DestinationRootNode.Local.LocalIndex);
                IL.Emit(OpCodes.Call, Schema.DestinationRootNode.Type.GetMethod(nameof(KeyValue<Type, Type>.ToKeyValuePair)));
                IL.Emit(OpCodes.Stobj, typeof(KeyValuePair<,>).MakeGenericType(new Type[]
                {
                    Schema.DestinationRootNode.Type.GenericTypeArguments[0],
                    Schema.DestinationRootNode.Type.GenericTypeArguments[1]
                }));
            }

            IL.Emit(OpCodes.Ret);
        }

        private void CreateSignature(bool debug)
        {
            Method = new DynamicMethod(
                $"{nameof(Air)}{Guid.NewGuid():N}",
                null,
                new[] { SourceType, DestinationType.MakeByRefType() },
                typeof(Mapper<,>).MakeGenericType(SourceType, DestinationType).Module,
                skipVisibility: true);
            IL = new Reflection.Emit.ILGenerator(Method.GetILGenerator(), debug);
        }

        private void CreateBody()
        {
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

            if (Reflection.TypeInfo.IsBuiltIn(SourceType))
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
                IL.EmitBrtrue_s(() => ReturnDefault());

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
            IL.EmitBrtrue_s(() => ReturnDefault());

            SetAndReturnDestinationRootNode();

            #endregion
        }

        private void CompileMethod(bool debug)
        {
            CheckArguments();

            CreateSignature(debug);
            CreateSchema();
            DeclareLocals();
            CreateBody();
        }

        public DynamicMethod Compile(IEnumerable<IMapOption> mapOptions = null)
        {
            MapOptions = mapOptions;
            CompileMethod(false);
            return Method;
        }

        public string ViewIL(IEnumerable<IMapOption> mapOptions = null)
        {
            MapOptions = mapOptions;
            CompileMethod(true);
            return IL.GetLog().ToString();
        }
    }
}
