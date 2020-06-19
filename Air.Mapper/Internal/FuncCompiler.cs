﻿using System;
using System.Linq;
using System.Reflection.Emit;

namespace Air.Mapper.Internal
{
    internal class FuncCompiler<S, D> : Compiler<S, D>
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

        private void SetDefaultDestinationRootNode()
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
                    Schema.GetChildNodes(sourceNode, 1, sn => sn.Load).FirstOrDefault();
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

        private void ReturnNewInstanceOrDefault()
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
                    IL.Emit(OpCodes.Call, destinationNode.Type.GetConstructor(new Type[] { destinationNode.NullableUnderlyingType }));
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
                IL.EmitBrfalse_s(
                    () => newInstance());

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
                IL.EmitBrfalse_s(
                    () => newInstance());

                loadDestination();
            }

            IL.Emit(OpCodes.Ret);
        }

        private void ReturnDefault()
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

        private void ReturnDestinationRootNode()
        {
            if (Schema.DestinationRootNode.NullableUnderlyingType != null)
            {
                IL.EmitLdloca(Schema.DestinationRootNode.NullableLocal.LocalIndex);
                IL.EmitLdloc(Schema.DestinationRootNode.Local.LocalIndex);
                IL.Emit(OpCodes.Call, Schema.DestinationRootNode.Type.GetConstructor(new Type[] { Schema.DestinationRootNode.NullableUnderlyingType }));
            }

            if (Schema.DestinationRootNode.NullableUnderlyingType != null)
                IL.EmitLdloc(Schema.DestinationRootNode.NullableLocal.LocalIndex);
            else
                IL.EmitLdloc(Schema.DestinationRootNode.Local.LocalIndex);

            IL.Emit(OpCodes.Ret);
        }

        private void CreateSignature(bool debug)
        {
            Method = new DynamicMethod($"{nameof(Air)}{Guid.NewGuid():N}", Destination.Type, new[] { Source.Type }, Source.Type.Module, skipVisibility: false);
            IL = new Reflection.Emit.ILGenerator(Method.GetILGenerator(), debug);
        }

        private void SetAndReturnFromToBuiltIn()
        {
            IL.EmitLdarg(0);
            IL.EmitConvert(Source.Type, Destination.Type);
            IL.Emit(OpCodes.Ret);
        }

        private void CreateBody()
        {
            #region Destination: Abstract / Interface

            if (Destination.Type.IsAbstract || Destination.Type.IsInterface)
            {
                IL.EmitLdarg(0);
                IL.EmitConvert(Source.Type, Destination.Type);
                IL.Emit(OpCodes.Ret);

                return;
            }

            #endregion

            #region Source: BuiltIn

            if (Source.IsBuiltIn)
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

        private void CompileMethod(bool debug)
        {
            CheckArguments();

            CreateSignature(debug);
            CreateSchema();
            DeclareLocals();
            InitDestinationLocals();
            CreateBody();
        }

        public Func<S, D> Compile(Action<MapOptions<S, D>> mapOptions = null)
        {
            SetMapOptions(mapOptions);
            CompileMethod(false);
            return (Func<S, D>)Method.CreateDelegate(typeof(Func<S, D>));
        }

        public override string ViewIL(Action<MapOptions<S, D>> mapOptions = null)
        {
            SetMapOptions(mapOptions);
            CompileMethod(true);
            return IL.GetLog().ToString();
        }
    }
}
