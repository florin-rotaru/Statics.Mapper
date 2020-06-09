using System;
using System.Reflection.Emit;

namespace Air.Mapper.Internal
{
    internal class ActionRefCompiler<S, D> : Compiler<S, D>
    {
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
            {
                IL.EmitLdloca(Schema.DestinationRootNode.Local.LocalIndex);
                IL.EmitInit(Schema.DestinationRootNode.Local.LocalType);

                Schema.DestinationRootNode.Loaded = true;
            }

            MapSourceNodes(Schema.SourceRootNode);

            ReturnDestinationRootNode();
        }

        private void ReturnDefaultDestinationRootNode()
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
                IL.EmitLdloc(Schema.DestinationRootNode.Local.LocalIndex);
                IL.Emit(OpCodes.Newobj, Schema.DestinationRootNode.Type.GetConstructor(new Type[] { Schema.DestinationRootNode.NullableUnderlyingType }));
                IL.Emit(OpCodes.Stobj, Schema.DestinationRootNode.Type);
            }

            IL.Emit(OpCodes.Ret);
        }

        private void CreateSignature(bool debug)
        {
            Method = new DynamicMethod($"{nameof(Air)}{Guid.NewGuid():N}", null, new[] { Source.Type, Destination.Type.MakeByRefType() }, Source.Type.Module, skipVisibility: false);
            IL = new Reflection.Emit.ILGenerator(Method.GetILGenerator(), debug);
        }

        private void CreateBody()
        {
            #region Destination: Abstract / Interface

            if (Destination.Type.IsAbstract || Destination.Type.IsInterface)
            {
                IL.EmitLdarg(1);
                IL.EmitLdarg(0);
                IL.EmitStore(Destination.Type);

                IL.Emit(OpCodes.Ret);

                return;
            }

            #endregion

            #region Source: BuiltIn

            if (Source.IsBuiltIn)
            {
                IL.EmitLdarg(1);
                IL.EmitLdarg(0);
                IL.EmitSetOrConvert(Source.Type, Destination.Type);
                IL.EmitStore(Destination.Type);

                IL.Emit(OpCodes.Ret);

                return;
            }

            #endregion

            #region Return default

            if (!Schema.DestinationRootNode.Load)
            {
                ReturnDefaultDestinationRootNode();

                return;
            }

            #endregion

            #region Source: Nullable

            if (Schema.SourceRootNode.NullableUnderlyingType != null)
            {
                IL.EmitLdarga(0);
                IL.Emit(OpCodes.Call, Schema.SourceRootNode.Type.GetProperty(HasValue).GetGetMethod());
                IL.EmitBrtrue_s(() => ReturnDefaultDestinationRootNode());

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
            IL.EmitBrtrue_s(() => ReturnDefaultDestinationRootNode());

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

        public Mapper<S, D>.ActionRef Compile(Action<MapOptions<S, D>> mapOptions = null)
        {
            SetMapOptions(mapOptions);
            CompileMethod(false);
            return (Mapper<S, D>.ActionRef)Method.CreateDelegate(typeof(Mapper<S, D>.ActionRef));
        }

        public override string ViewIL(Action<MapOptions<S, D>> mapOptions = null)
        {
            SetMapOptions(mapOptions);
            CompileMethod(true);
            return IL.GetLog().ToString();
        }
    }
}
