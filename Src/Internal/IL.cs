using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;

namespace Statics.Mapper.Internal
{
    internal partial class IL(ILGenerator iLGenerator, bool enableLog = false)
    {
        ILGenerator ILGenerator { get; } = iLGenerator;
        bool LogEnabled { get; } = enableLog;
        StringBuilder LocalsBuilder { get; } = new();
        StringBuilder LogBuilder { get; } = new();

        void AppendLineToLocals(string value) =>
            LocalsBuilder.AppendLine(value);
        void AppendLineToLog(string value) =>
            LogBuilder.AppendLine(value);

        public StringBuilder GetLog() =>
            new StringBuilder(LocalsBuilder.ToString())
                .Append(LocalsBuilder.Length != 0 ? Environment.NewLine : string.Empty)
                .Append(LogBuilder);

        #region System.Reflection.Emit.ILGeneration, Version=6.0.0.0

        public int ILOffset => ILGenerator.ILOffset;

        public virtual void BeginCatchBlock(Type exceptionType)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(BeginCatchBlock)} (Type {nameof(exceptionType)}: {exceptionType.FullName})");
            ILGenerator.BeginCatchBlock(exceptionType);
        }
        public virtual void BeginExceptFilterBlock()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(BeginExceptFilterBlock)}");
            ILGenerator.BeginExceptFilterBlock();
        }
        public virtual Label BeginExceptionBlock()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(BeginExceptionBlock)}");
            return ILGenerator.BeginExceptionBlock();
        }
        public virtual void BeginFaultBlock()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(BeginFaultBlock)}");
            ILGenerator.BeginFaultBlock();
        }
        public virtual void BeginFinallyBlock()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(BeginFinallyBlock)}");
            ILGenerator.BeginFinallyBlock();
        }
        public virtual void BeginScope()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(BeginScope)}");
            ILGenerator.BeginScope();
        }
        public virtual LocalBuilder DeclareLocal(Type localType)
        {
            LocalBuilder returnValue = ILGenerator.DeclareLocal(localType);
            if (LogEnabled) AppendLineToLocals($"{nameof(DeclareLocal)} [{returnValue.LocalIndex}] (Type {nameof(localType)}: {localType.FullName})");
            return returnValue;
        }
        public virtual LocalBuilder DeclareLocal(Type localType, bool pinned)
        {
            LocalBuilder returnValue = ILGenerator.DeclareLocal(localType, pinned);
            if (LogEnabled) AppendLineToLocals($"{nameof(DeclareLocal)} [{returnValue.LocalIndex}] (Type {nameof(localType)}: {localType.FullName}, bool pinned: {pinned})");
            return returnValue;
        }
        public virtual Label DefineLabel() =>
            ILGenerator.DefineLabel();
        public virtual void Emit(OpCode opcode)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode})");
            ILGenerator.Emit(opcode);
        }
        public virtual void Emit(OpCode opcode, byte arg)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, byte {nameof(arg)}: {arg})");
            ILGenerator.Emit(opcode, arg);
        }
        public virtual void Emit(OpCode opcode, double arg)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, double {nameof(arg)}: {arg})");
            ILGenerator.Emit(opcode, arg);
        }
        public virtual void Emit(OpCode opcode, short arg)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, short {nameof(arg)}: {arg})");
            ILGenerator.Emit(opcode, arg);
        }
        public virtual void Emit(OpCode opcode, int arg)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, int {nameof(arg)}: {arg})");
            ILGenerator.Emit(opcode, arg);
        }
        public virtual void Emit(OpCode opcode, long arg)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, long {nameof(arg)}: {arg})");
            ILGenerator.Emit(opcode, arg);
        }
        public virtual void Emit(OpCode opcode, ConstructorInfo con)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, ConstructorInfo {nameof(con)}: {con.DeclaringType} {con})");
            ILGenerator.Emit(opcode, con);
        }
        public virtual void Emit(OpCode opcode, Label label)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, Label {nameof(label)}: label_{label.GetHashCode()}) {Environment.NewLine}");
            ILGenerator.Emit(opcode, label);
        }
        public virtual void Emit(OpCode opcode, Label[] labels)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, Label[] {nameof(labels)}: {string.Join(", ", labels.Select(s => "label_" + s.GetHashCode().ToString()))} {Environment.NewLine}");
            ILGenerator.Emit(opcode, labels);
        }
        public virtual void Emit(OpCode opcode, LocalBuilder local)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, LocalBuilder {nameof(local)}: {local})");
            ILGenerator.Emit(opcode, local);
        }
        public virtual void Emit(OpCode opcode, SignatureHelper signature)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, SignatureHelper {nameof(signature)}: {signature})");
            ILGenerator.Emit(opcode, signature);
        }
        public virtual void Emit(OpCode opcode, FieldInfo field)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, FieldInfo {nameof(field)}: {field})");
            ILGenerator.Emit(opcode, field);
        }
        public virtual void Emit(OpCode opcode, MethodInfo meth)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, MethodInfo {meth.DeclaringType} {nameof(meth)}: {meth})");
            ILGenerator.Emit(opcode, meth);
        }
        public virtual void Emit(OpCode opcode, float arg)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, float {nameof(arg)}: {arg})");
            ILGenerator.Emit(opcode, arg);
        }
        public virtual void Emit(OpCode opcode, string str)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, string {nameof(str)}: {str})");
            ILGenerator.Emit(opcode, str);
        }
        public virtual void Emit(OpCode opcode, Type cls)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, Type {nameof(cls)}: {cls})");
            ILGenerator.Emit(opcode, cls);
        }
        public virtual void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EmitCall)} (OpCode {nameof(opcode)}: {opcode}, MethodInfo {methodInfo.DeclaringType} {nameof(methodInfo)}: {methodInfo}, Type[] {nameof(optionalParameterTypes)}: {optionalParameterTypes})");
            ILGenerator.EmitCall(opcode, methodInfo, optionalParameterTypes);
        }
        public virtual void EmitCalli(OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EmitCalli)} (OpCode {nameof(opcode)}: {opcode}, " +
                $"CallingConventions {nameof(callingConvention)}: {callingConvention}, Type {nameof(returnType)}: {returnType}, " +
                $"Type[] {nameof(parameterTypes)}: {parameterTypes}, Type[] {nameof(optionalParameterTypes)}: {optionalParameterTypes})");
            ILGenerator.EmitCalli(opcode, callingConvention, returnType, parameterTypes, optionalParameterTypes);
        }
        public virtual void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EmitCalli)} (OpCode {nameof(opcode)}: {opcode}, " +
              $"CallingConvention {nameof(unmanagedCallConv)}: {unmanagedCallConv}, Type {nameof(returnType)}: {returnType}, " +
              $"Type[] {nameof(parameterTypes)}: {parameterTypes})");
            ILGenerator.EmitCalli(opcode, unmanagedCallConv, returnType, parameterTypes);
        }
        public virtual void EmitWriteLine(LocalBuilder localBuilder)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EmitWriteLine)} (LocalBuilder {nameof(localBuilder)}: {localBuilder})");
            ILGenerator.EmitWriteLine(localBuilder);
        }
        public virtual void EmitWriteLine(FieldInfo fld)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EmitWriteLine)} (FieldInfo {nameof(fld)}: {fld})");
            ILGenerator.EmitWriteLine(fld);
        }
        public virtual void EmitWriteLine(string value)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EmitWriteLine)} (string {nameof(value)}: {value})");
            ILGenerator.EmitWriteLine(value);
        }
        public virtual void EndExceptionBlock()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EndExceptionBlock)}");
            ILGenerator.EndExceptionBlock();
        }
        public virtual void EndScope()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EndScope)}");
            ILGenerator.EndScope();
        }
        public virtual void MarkLabel(Label loc)
        {
            if (LogEnabled) AppendLineToLog($"label_{loc.GetHashCode()} {Environment.NewLine}");
            ILGenerator.MarkLabel(loc);
        }
        public virtual void ThrowException(Type excType)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(ThrowException)} (Type {nameof(excType)}: {excType})");
            ILGenerator.ThrowException(excType);
        }
        public virtual void UsingNamespace(string usingNamespace)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(UsingNamespace)} (string {nameof(usingNamespace)}: {usingNamespace})");
            ILGenerator.UsingNamespace(usingNamespace);
        }

        #endregion

        protected const string To = "To";

        public static readonly FieldInfo DBNullValue =
          typeof(DBNull).GetFields().First(f => f.Name == nameof(DBNull.Value) && f.FieldType == typeof(DBNull));

        public static readonly MethodInfo GetTypeFromHandle =
            typeof(Type).GetMethods(BindingFlags.Public | BindingFlags.Static).Single(m =>
                m.Name == nameof(Type.GetTypeFromHandle) &&
                m.ReturnType == typeof(Type) &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == typeof(RuntimeTypeHandle));

        static readonly MethodInfo EnumParse = typeof(Enum).GetMethods(BindingFlags.Public | BindingFlags.Static).First(m =>
            m.IsGenericMethod &&
            m.Name == nameof(Enum.Parse) &&
            m.GetParameters().Length == 1 &&
            m.GetParameters()[0].ParameterType == typeof(string));

#pragma warning disable CS8601 // Possible null reference assignment.
        static readonly ConstructorInfo DateTimeConstructorTicks = typeof(DateTime).GetConstructor([typeof(long)]);
        static readonly ConstructorInfo DateTimeOffsetConstructorDateTime = typeof(DateTimeOffset).GetConstructor([typeof(DateTime)]);
        static readonly ConstructorInfo DecimalConstructorBits = typeof(decimal).GetConstructor([typeof(int[])]);
        static readonly ConstructorInfo GuidConstructorBytes = typeof(Guid).GetConstructor([typeof(byte[])]);
        static readonly ConstructorInfo TimeSpanConstructorTicks = typeof(TimeSpan).GetConstructor([typeof(long)]);
        static readonly MethodInfo ObjectToString = typeof(object).GetMethod(nameof(object.ToString), Type.EmptyTypes);
#pragma warning restore CS8601 // Possible null reference assignment.

        public void EmitCallMethod(MethodInfo method) =>
            Emit(method.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, method);

        public void EmitCallMethod(MapperTypeMemberInfo member, MethodInfo method) =>
            Emit(member.IsStatic || member.MemberOf.IsValueType ?
                OpCodes.Call :
                method.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, method);

        #region Load

        public void EmitLdelem(Type type)
        {
            if (type == typeof(bool)) { Emit(OpCodes.Ldelem_U1); return; }
            if (type == typeof(char)) { Emit(OpCodes.Ldelem_U2); return; }
            if (type == typeof(sbyte)) { Emit(OpCodes.Ldelem_I1); return; }
            if (type == typeof(byte)) { Emit(OpCodes.Ldelem_U1); return; }
            if (type == typeof(short)) { Emit(OpCodes.Ldelem_I2); return; }
            if (type == typeof(ushort)) { Emit(OpCodes.Ldelem_U2); return; }
            if (type == typeof(int)) { Emit(OpCodes.Ldelem_I4); return; }
            if (type == typeof(uint)) { Emit(OpCodes.Ldelem_U4); return; }
            if (type == typeof(long)) { Emit(OpCodes.Ldelem_I8); return; }
            if (type == typeof(ulong)) { Emit(OpCodes.Ldelem_I8); return; }
            if (type == typeof(float)) { Emit(OpCodes.Ldelem_R4); return; }
            if (type == typeof(double)) { Emit(OpCodes.Ldelem_R8); return; }
            if (type == typeof(decimal)) { Emit(OpCodes.Ldelem, type); return; }
            if (type == typeof(string)) { Emit(OpCodes.Ldelem_Ref); return; }
            if (type == typeof(DateTime)) { Emit(OpCodes.Ldelem, type); return; }
            if (type == typeof(DateTimeOffset)) { Emit(OpCodes.Ldelem, type); return; }
            if (type == typeof(Guid)) { Emit(OpCodes.Ldelem, type); return; }
            if (type == typeof(TimeSpan)) { Emit(OpCodes.Ldelem, type); return; }
            if (type.IsEnum) { Emit(OpCodes.Ldelem_I4); return; }
            if (type.IsValueType) { Emit(OpCodes.Ldelem, type); return; }

            Emit(OpCodes.Ldelem_Ref);
        }

        public void EmitStelem(Type type)
        {
            if (type == typeof(bool)) { Emit(OpCodes.Stelem_I1); return; }
            if (type == typeof(char)) { Emit(OpCodes.Stelem_I2); return; }
            if (type == typeof(sbyte)) { Emit(OpCodes.Stelem_I1); return; }
            if (type == typeof(byte)) { Emit(OpCodes.Stelem_I1); return; }
            if (type == typeof(short)) { Emit(OpCodes.Stelem_I2); return; }
            if (type == typeof(ushort)) { Emit(OpCodes.Stelem_I2); return; }
            if (type == typeof(int)) { Emit(OpCodes.Stelem_I4); return; }
            if (type == typeof(uint)) { Emit(OpCodes.Stelem_I4); return; }
            if (type == typeof(long)) { Emit(OpCodes.Stelem_I8); return; }
            if (type == typeof(ulong)) { Emit(OpCodes.Stelem_I8); return; }
            if (type == typeof(float)) { Emit(OpCodes.Stelem_R4); return; }
            if (type == typeof(double)) { Emit(OpCodes.Stelem_R8); return; }
            if (type == typeof(decimal)) { Emit(OpCodes.Stelem, type); return; }
            if (type == typeof(string)) { Emit(OpCodes.Stelem_Ref); return; }
            if (type == typeof(DateTime)) { Emit(OpCodes.Stelem, type); return; }
            if (type == typeof(DateTimeOffset)) { Emit(OpCodes.Stelem, type); return; }
            if (type == typeof(Guid)) { Emit(OpCodes.Stelem, type); return; }
            if (type == typeof(TimeSpan)) { Emit(OpCodes.Stelem, type); return; }
            if (type.IsEnum) { Emit(OpCodes.Stelem_I4); return; }
            if (type.IsValueType) { Emit(OpCodes.Stelem, type); return; }

            Emit(OpCodes.Stelem_Ref);
        }

        public void EmitLdc_I4(int value)
        {
            switch (value)
            {
                case -1: Emit(OpCodes.Ldc_I4_M1); break;
                case 0: Emit(OpCodes.Ldc_I4_0); break;
                case 1: Emit(OpCodes.Ldc_I4_1); break;
                case 2: Emit(OpCodes.Ldc_I4_2); break;
                case 3: Emit(OpCodes.Ldc_I4_3); break;
                case 4: Emit(OpCodes.Ldc_I4_4); break;
                case 5: Emit(OpCodes.Ldc_I4_5); break;
                case 6: Emit(OpCodes.Ldc_I4_6); break;
                case 7: Emit(OpCodes.Ldc_I4_7); break;
                case 8: Emit(OpCodes.Ldc_I4_8); break;
                default:
                    if (value >= sbyte.MinValue && value <= sbyte.MaxValue) { Emit(OpCodes.Ldc_I4_S, (sbyte)value); }
                    else { Emit(OpCodes.Ldc_I4, value); }
                    break;
            }
        }

        public void EmitLdarg(int index)
        {
            if (index < 0 || index >= short.MaxValue) throw new ArgumentOutOfRangeException(nameof(index));
            switch (index)
            {
                case 0: Emit(OpCodes.Ldarg_0); break;
                case 1: Emit(OpCodes.Ldarg_1); break;
                case 2: Emit(OpCodes.Ldarg_2); break;
                case 3: Emit(OpCodes.Ldarg_3); break;
                default:
                    if (index <= byte.MaxValue)
                    {
                        Emit(OpCodes.Ldarg_S, (byte)index);
                    }
                    else
                    {
                        Emit(OpCodes.Ldarg, (short)index);
                    }
                    break;
            }
        }

        public void EmitLdarga(int index)
        {
            if (index < 0 || index >= short.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (index <= byte.MaxValue)
                Emit(OpCodes.Ldarga_S, (byte)index);
            else
                Emit(OpCodes.Ldarga, (short)index);
        }

        public void EmitLoadArgument(Type argumentType, int index, bool isByRef = false)
        {
            if (!argumentType.IsValueType || isByRef)
                EmitLdarg(index);
            else
                EmitLdarga(index);
        }

        public void EmitLdloc(int index)
        {
            if (index < 0 || index >= short.MaxValue) throw new ArgumentOutOfRangeException(nameof(index));
            switch (index)
            {
                case 0: Emit(OpCodes.Ldloc_0); break;
                case 1: Emit(OpCodes.Ldloc_1); break;
                case 2: Emit(OpCodes.Ldloc_2); break;
                case 3: Emit(OpCodes.Ldloc_3); break;
                default:
                    if (index <= byte.MaxValue)
                        Emit(OpCodes.Ldloc_S, (byte)index);
                    else
                        Emit(OpCodes.Ldloc, (short)index);

                    break;
            }
        }

        public void EmitLdloca(int index)
        {
            if (index < 0 || index >= short.MaxValue) throw new ArgumentOutOfRangeException(nameof(index));

            if (index <= byte.MaxValue)
                Emit(OpCodes.Ldloca_S, (byte)index);
            else
                Emit(OpCodes.Ldloca, (short)index);
        }

        public void EmitLoadLocal(int index, bool loadAddress)
        {
            if (loadAddress)
                EmitLdloca(index);
            else
                EmitLdloc(index);
        }

        public void EmitLoadLocal(LocalBuilder local, bool loadAddress) =>
            EmitLoadLocal(local.LocalIndex, loadAddress);

        public void EmitLoadLiteral(Type type, object value)
        {
            if (type == typeof(bool)) { EmitLdc_I4((bool)value ? 1 : 0); return; }
            if (type == typeof(char)) { EmitLdc_I4((char)value); return; }
            if (type == typeof(sbyte)) { EmitLdc_I4((sbyte)value); return; }
            if (type == typeof(byte)) { EmitLdc_I4((byte)value); return; }
            if (type == typeof(short)) { EmitLdc_I4((short)value); return; }
            if (type == typeof(ushort)) { EmitLdc_I4((ushort)value); return; }
            if (type == typeof(int)) { EmitLdc_I4((int)value); return; }
            if (type == typeof(uint)) { Emit(OpCodes.Ldc_I4, (uint)value); return; }
            if (type == typeof(long)) { Emit(OpCodes.Ldc_I8, (long)value); return; }
            if (type == typeof(ulong)) { Emit(OpCodes.Ldc_I8, (long)(ulong)value); Emit(OpCodes.Conv_U8); return; }
            if (type == typeof(float)) { Emit(OpCodes.Ldc_R4, (float)value); return; }
            if (type == typeof(double)) { Emit(OpCodes.Ldc_R8, (double)value); return; }
            if (type == typeof(decimal))
            {
                int[] memberDecimalBits = decimal.GetBits((decimal)value);

                EmitLdc_I4(4);
                Emit(OpCodes.Newarr, typeof(int));

                for (int i = 0; i < memberDecimalBits.Length; i++)
                {
                    Emit(OpCodes.Dup);
                    EmitLdc_I4(i);
                    EmitLdc_I4(memberDecimalBits[i]);
                    Emit(OpCodes.Stelem_I4);
                }

                Emit(OpCodes.Newobj, DecimalConstructorBits);

                return;
            }
            if (type == typeof(string))
            {
                Emit(OpCodes.Ldstr, (string)value);
                return;
            }
            if (type == typeof(DateTime))
            {
                Emit(OpCodes.Ldc_I8, ((DateTime)value).Ticks);
                Emit(OpCodes.Newobj, DateTimeConstructorTicks);
                return;
            }
            if (type == typeof(DateTimeOffset))
            {
                Emit(OpCodes.Ldc_I8, ((DateTimeOffset)value).Ticks);
                Emit(OpCodes.Newobj, DateTimeConstructorTicks);
                Emit(OpCodes.Newobj, DateTimeOffsetConstructorDateTime);
                return;
            }
            if (type.IsEnum)
            {
                EmitLoadLiteral(Enum.GetUnderlyingType(type), value);
                return;
            }
            if (type == typeof(Guid))
            {
                byte[] memberGuidBytes = ((Guid)value).ToByteArray();
                EmitLdc_I4(16);
                Emit(OpCodes.Newarr, typeof(byte));

                for (int i = 0; i < memberGuidBytes.Length; i++)
                {
                    Emit(OpCodes.Dup);
                    EmitLdc_I4(i);
                    EmitLdc_I4(memberGuidBytes[i]);
                    Emit(OpCodes.Stelem_I1);
                }

                Emit(OpCodes.Newobj, GuidConstructorBytes);
                return;
            }
            if (type == typeof(TimeSpan))
            {
                Emit(OpCodes.Ldc_I8, ((TimeSpan)value).Ticks);
                Emit(OpCodes.Newobj, TimeSpanConstructorTicks);
                return;
            }
            else
            {
                throw new NotImplementedException($"{type}");
            }
        }

        public void EmitLoadLiteral(MapperTypeMemberInfo member)
        {
            if (!member.IsLiteral)
                throw new InvalidOperationException($"{member} is not a literal.");

            EmitLoadLiteral(member.Type, member.DefaultValue);
        }

        public void EmitLoadDefaultValue(Type type)
        {
            if (!type.IsValueType)
            {
                Emit(OpCodes.Ldnull);
                return;
            }

            Type? underlyingType = Nullable.GetUnderlyingType(type);

            if (underlyingType != null)
            {
                MethodInfo? method = typeof(ILNullables).GetMethod(nameof(ILNullables.GetDefaultValue));
                EmitCallMethod(method.MakeGenericMethod(underlyingType));
                return;
            }

            if (type == typeof(bool)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(char)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(sbyte)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(byte)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(short)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(ushort)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(int)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(uint)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(long)) { Emit(OpCodes.Ldc_I4_0); Emit(OpCodes.Conv_I8); return; }
            if (type == typeof(ulong)) { Emit(OpCodes.Ldc_I4_0); Emit(OpCodes.Conv_I8); return; }
            if (type == typeof(float)) { Emit(OpCodes.Ldc_R4, (float)0); return; }
            if (type == typeof(double)) { Emit(OpCodes.Ldc_R8, (double)0); return; }
            if (type == typeof(decimal)) { Emit(OpCodes.Ldsfld, typeof(decimal).GetFields().First(f => f.Name == nameof(Decimal.Zero))); return; }
            if (type == typeof(string)) { Emit(OpCodes.Ldnull); return; }
            if (type == typeof(DateTime))
            {
                Emit(OpCodes.Ldc_I4_0);
                Emit(OpCodes.Conv_I8);
                Emit(OpCodes.Newobj, DateTimeConstructorTicks);
                return;
            }
            if (type == typeof(DateTimeOffset))
            {
                Emit(OpCodes.Ldc_I4_0);
                Emit(OpCodes.Conv_I8);
                Emit(OpCodes.Newobj, DateTimeConstructorTicks);
                Emit(OpCodes.Newobj, DateTimeOffsetConstructorDateTime);
                return;
            }
            if (type.IsEnum) { EmitLoadDefaultValue(Enum.GetUnderlyingType(type)); return; }
            if (type == typeof(Guid)) { Emit(OpCodes.Ldsfld, typeof(Guid).GetFields().First(f => f.Name == nameof(Guid.Empty))); return; }
            if (type == typeof(TimeSpan))
            {
                Emit(OpCodes.Ldc_I4_0);
                Emit(OpCodes.Conv_I8);
                Emit(OpCodes.Newobj, TimeSpanConstructorTicks);
                return;
            }
            else
            {
                throw new NotSupportedException($"{type}");
            }
        }

        public void EmitLoadStaticMemberValue(MapperTypeMemberInfo member)
        {
            if (member.MemberType == MemberTypes.Property)
                EmitCallMethod(((PropertyInfo)member.MemberInfo).GetGetMethod());
            else if (member.MemberType == MemberTypes.Field)
                Emit(OpCodes.Ldsfld, (FieldInfo)member.MemberInfo);
            else
                throw new NotImplementedException();
        }

        public void EmitLoadMemberValue(MapperTypeMemberInfo member)
        {
            if (member.IsLiteral)
                EmitLoadLiteral(member);
            else if (member.IsStatic)
                EmitLoadStaticMemberValue(member);
            else if (member.MemberType == MemberTypes.Property)
                EmitCallMethod(((PropertyInfo)member.MemberInfo).GetGetMethod());
            else if (member.MemberType == MemberTypes.Field)
                Emit(OpCodes.Ldfld, (FieldInfo)member.MemberInfo);
            else
                throw new NotImplementedException();
        }

        #endregion


        #region Init

        public void EmitInit(Type type)
        {
            Type typeOrUnderlyingType = type.IsEnum ? Enum.GetUnderlyingType(type) : type;

            if (typeOrUnderlyingType.IsValueType)
                Emit(OpCodes.Initobj, typeOrUnderlyingType);
            else if (typeOrUnderlyingType.GetConstructor(Type.EmptyTypes) != null)
                Emit(OpCodes.Newobj, typeOrUnderlyingType.GetConstructor(Type.EmptyTypes));
            else
                throw new InvalidOperationException($"Failed to initialize Type {type}");
        }

        public void EmitInit(LocalBuilder local)
        {
            if (!local.LocalType.IsValueType)
            {
                EmitInit(local.LocalType);
                EmitStoreLocal(local);
            }
            else
            {
                EmitLoadLocal(local, true);
                EmitInit(local.LocalType);
            }
        }

        #endregion


        #region Store

        public void EmitStarg(int index)
        {
            if (index < 0 || index >= short.MaxValue) throw new ArgumentOutOfRangeException(nameof(index));
            switch (index)
            {
                default:
                    if (index <= byte.MaxValue)
                        Emit(OpCodes.Starg_S, (byte)index);
                    else
                        Emit(OpCodes.Starg, (short)index);

                    break;
            }
        }

        public void EmitStloc(int index)
        {
            if (index < 0 || index >= short.MaxValue) throw new ArgumentOutOfRangeException(nameof(index));
            switch (index)
            {
                case 0: Emit(OpCodes.Stloc_0); break;
                case 1: Emit(OpCodes.Stloc_1); break;
                case 2: Emit(OpCodes.Stloc_2); break;
                case 3: Emit(OpCodes.Stloc_3); break;
                default:
                    if (index <= byte.MaxValue)
                        Emit(OpCodes.Stloc_S, (byte)index);
                    else
                        Emit(OpCodes.Stloc, (short)index);

                    break;
            }
        }

        public void EmitStoreLocal(LocalBuilder local) =>
            EmitStloc(local.LocalIndex);

        public void EmitStore(Type destination)
        {
            if (destination == typeof(bool)) { Emit(OpCodes.Stind_I1); return; }

            if (destination == typeof(sbyte)) { Emit(OpCodes.Stind_I1); return; }
            if (destination == typeof(byte)) { Emit(OpCodes.Stind_I1); return; }

            if (destination == typeof(char)) { Emit(OpCodes.Stind_I2); return; }

            if (destination == typeof(short)) { Emit(OpCodes.Stind_I2); return; }
            if (destination == typeof(ushort)) { Emit(OpCodes.Stind_I2); return; }

            if (destination == typeof(int)) { Emit(OpCodes.Stind_I4); return; }
            if (destination == typeof(uint)) { Emit(OpCodes.Stind_I4); return; }

            if (destination == typeof(long)) { Emit(OpCodes.Stind_I8); return; }
            if (destination == typeof(ulong)) { Emit(OpCodes.Stind_I8); return; }

            if (destination == typeof(float)) { Emit(OpCodes.Stind_R4); return; }
            if (destination == typeof(double)) { Emit(OpCodes.Stind_R8); return; }

            if (destination == typeof(string)) { Emit(OpCodes.Stind_Ref); return; }

            if (destination.IsValueType)
                Emit(OpCodes.Stobj, destination);
            else
                Emit(OpCodes.Stind_Ref);
        }

        #endregion


        #region EmitBrtrue

        /// <summary>
        /// Transfers control to a target instruction if value is true, not null, or non-zero.
        /// </summary>
        /// <param name="whenFalseNullOrZero"></param>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        public void EmitBrtrue(
            Action whenFalseNullOrZero,
            Action whenTrueNotNullOrNonZero,
            bool useBr_S = false)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brtrue, evaluate);

            whenFalseNullOrZero();

            Label end = DefineLabel();
            Emit(useBr_S ? OpCodes.Br_S : OpCodes.Br, end);

            MarkLabel(evaluate);

            whenTrueNotNullOrNonZero();

            MarkLabel(end);
        }

        /// <summary>
        /// Transfers control to a target instruction (short form) if value is true, not null, or non-zero.
        /// </summary>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        public void EmitBrtrue_s(
            Action whenFalseNullOrZero)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brtrue_S, evaluate);

            whenFalseNullOrZero();

            MarkLabel(evaluate);
        }

        /// <summary>
        /// Transfers control to a target instruction if value is true, not null, or non-zero.
        /// </summary>
        /// <param name="whenFalseNullOrZero"></param>
        public void EmitBrtrue(
            Action whenFalseNullOrZero)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brtrue, evaluate);

            whenFalseNullOrZero();

            MarkLabel(evaluate);
        }

        /// <summary>
        /// Transfers control to a target instruction (short form) if value is true, not null, or non-zero.
        /// </summary>
        /// <param name="whenFalseNullOrZero"></param>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        public void EmitBrtrue_s(
            Action whenFalseNullOrZero,
            Action whenTrueNotNullOrNonZero,
            bool useBr_S = false)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brtrue_S, evaluate);

            whenFalseNullOrZero();

            Label end = DefineLabel();
            Emit(useBr_S ? OpCodes.Br_S : OpCodes.Br, end);

            MarkLabel(evaluate);

            whenTrueNotNullOrNonZero();

            MarkLabel(end);
        }

        #endregion


        #region EmitBrfalse

        /// <summary>
        /// Transfers control to a target instruction (short form) if value is false, a null reference, or zero.
        /// </summary>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        /// <param name="whenFalseNullOrZero"></param>
        public void EmitBrfalse_s(
            Action whenTrueNotNullOrNonZero,
            Action whenFalseNullOrZero,
            bool useBr_S = false)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brfalse_S, evaluate);

            whenTrueNotNullOrNonZero();

            Label end = DefineLabel();

            Emit(useBr_S ? OpCodes.Br_S : OpCodes.Br, end);

            MarkLabel(evaluate);

            whenFalseNullOrZero();

            MarkLabel(end);
        }

        /// <summary>
        /// Transfers control to a target instruction if value is false, a null reference, or zero.
        /// </summary>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        /// <param name="whenFalseNullOrZero"></param>
        public void EmitBrfalse(
            Action whenTrueNotNullOrNonZero,
            Action whenFalseNullOrZero,
            bool useBr_S = false)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brfalse, evaluate);

            whenTrueNotNullOrNonZero();

            Label end = DefineLabel();

            Emit(useBr_S ? OpCodes.Br_S : OpCodes.Br, end);

            MarkLabel(evaluate);

            whenFalseNullOrZero();

            MarkLabel(end);
        }

        /// <summary>
        /// Transfers control to a target instruction (short form) if value is false, a null reference, or zero.
        /// </summary>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        public void EmitBrfalse_s(
            Action whenTrueNotNullOrNonZero)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brfalse_S, evaluate);

            whenTrueNotNullOrNonZero();

            MarkLabel(evaluate);
        }

        /// <summary>
        /// Transfers control to a target instruction if value is false, a null reference, or zero.
        /// </summary>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        public void EmitBrfalse(
            Action whenTrueNotNullOrNonZero)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brfalse, evaluate);

            whenTrueNotNullOrNonZero();

            MarkLabel(evaluate);
        }

        #endregion


        #region Convert To MethodInfo

        static MethodInfo? TypeConvertToMethodInfo(Type nonNullableSourceType, Type nonNullableDestinationType) =>
           nonNullableSourceType.GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m =>
              m.Name.Contains(To + nonNullableSourceType.Name) &&
              m.ReturnType == nonNullableDestinationType &&
              m.GetParameters().Length == 1 &&
              m.GetParameters()[0].ParameterType == nonNullableSourceType);

        static MethodInfo? ConvertersToMethodInfo(Type nonNullableSourceType, Type nonNullableDestinationType) =>
            typeof(ILConverters).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m =>
               m.Name.Contains(To + nonNullableDestinationType.Name) &&
               m.ReturnType == nonNullableDestinationType &&
               m.GetParameters().Length == 1 &&
               m.GetParameters()[0].ParameterType == nonNullableSourceType);

        static MethodInfo? ConvertToMethodInfo(Type nonNullableSourceType, Type nonNullableDestinationType) =>
            typeof(Convert).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m =>
                m.Name.Contains(To + nonNullableDestinationType.Name) &&
                m.ReturnType == nonNullableDestinationType &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == nonNullableSourceType);

        #endregion


        public void EmitSetStaticMemberValue(MapperTypeMemberInfo member)
        {
            if (member.MemberType == MemberTypes.Property)
                EmitCallMethod(((PropertyInfo)member.MemberInfo).GetSetMethod());
            else if (member.MemberType == MemberTypes.Field)
                Emit(OpCodes.Stsfld, (FieldInfo)member.MemberInfo);
            else
                throw new NotImplementedException();
        }

        public void EmitSetMemberValue(MapperTypeMemberInfo member)
        {
            if (member.IsLiteral)
                throw new InvalidOperationException($"{nameof(EmitSetMemberValue)} {member.Name} {nameof(member.IsLiteral)}");
            else if (member.IsStatic)
                EmitSetStaticMemberValue(member);
            else if (member.MemberType == MemberTypes.Property)
                EmitCallMethod(((PropertyInfo)member.MemberInfo).GetSetMethod());
            else if (member.MemberType == MemberTypes.Field)
                Emit(OpCodes.Stfld, (FieldInfo)member.MemberInfo);
            else
                throw new NotImplementedException();
        }

        void EmitConvertToNonNullableNumeric(Type nonNullableNumericType)
        {
            if (nonNullableNumericType == typeof(sbyte)) { Emit(OpCodes.Conv_Ovf_I1); return; }
            if (nonNullableNumericType == typeof(byte)) { Emit(OpCodes.Conv_Ovf_I1_Un); return; }
            if (nonNullableNumericType == typeof(short)) { Emit(OpCodes.Conv_Ovf_I2); return; }
            if (nonNullableNumericType == typeof(ushort)) { Emit(OpCodes.Conv_Ovf_I2_Un); return; }
            if (nonNullableNumericType == typeof(int)) { Emit(OpCodes.Conv_Ovf_I4); return; }
            if (nonNullableNumericType == typeof(uint)) { Emit(OpCodes.Conv_Ovf_I4_Un); return; }
            if (nonNullableNumericType == typeof(long)) { Emit(OpCodes.Conv_Ovf_I8); return; }
            if (nonNullableNumericType == typeof(ulong)) { Emit(OpCodes.Conv_Ovf_I8_Un); return; }
            if (nonNullableNumericType == typeof(float)) { Emit(OpCodes.Conv_R4); return; }

            if (nonNullableNumericType == typeof(double)) { Emit(OpCodes.Conv_R8); return; }
        }

        static MethodInfo? GetConvertToMethodInfo(
            Type nonNullableSourceType,
            Type nonNullableDestinationType)
        {
            MethodInfo? returnValue = TypeConvertToMethodInfo(nonNullableSourceType, nonNullableDestinationType);
            if (returnValue != null)
                return returnValue;

            returnValue = ConvertersToMethodInfo(nonNullableSourceType, nonNullableDestinationType);
            if (returnValue != null)
                return returnValue;

            return ConvertToMethodInfo(nonNullableSourceType, nonNullableDestinationType);
        }

        static bool CanEmitConvertToChar(
           Type nonNullableSourceType,
           Type nonNullableDestinationType) =>
               nonNullableSourceType == typeof(string) ||
               MapperTypeInfo.IsEnum(nonNullableSourceType) ||
               GetConvertToMethodInfo(nonNullableSourceType, nonNullableDestinationType) != null;

        static bool CanEmitConvertToNumeric(
            Type nonNullableSourceType,
            Type nonNullableDestinationType) =>
                MapperTypeInfo.IsEnum(nonNullableSourceType) ||
                GetConvertToMethodInfo(nonNullableSourceType, nonNullableDestinationType) != null;

        static bool CanEmitConvertToEnum(Type nonNullableSourceType) =>
            nonNullableSourceType == typeof(string) ||
            nonNullableSourceType == typeof(char) ||
            MapperTypeInfo.IsNumeric(nonNullableSourceType) ||
            MapperTypeInfo.IsEnum(nonNullableSourceType);

        public static bool CanEmitConvert(
            Type source,
            Type destination)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destination);

            if (destination == typeof(object) ||
                destination == typeof(string))
                return true;

            Type nonNullableSourceType = Nullable.GetUnderlyingType(source) ?? source;
            Type nonNullableDestinationType = Nullable.GetUnderlyingType(destination) ?? destination;

            if (nonNullableSourceType == nonNullableDestinationType)
                return true;

            if (MapperTypeInfo.IsBuiltIn(nonNullableSourceType) != MapperTypeInfo.IsBuiltIn(nonNullableDestinationType))
                return false;

            if (nonNullableDestinationType == typeof(char))
                return CanEmitConvertToChar(nonNullableSourceType, nonNullableDestinationType);

            if (MapperTypeInfo.IsNumeric(nonNullableDestinationType))
                return CanEmitConvertToNumeric(nonNullableSourceType, nonNullableDestinationType);

            if (MapperTypeInfo.IsEnum(nonNullableDestinationType))
                return CanEmitConvertToEnum(nonNullableSourceType);

            return GetConvertToMethodInfo(nonNullableSourceType, nonNullableDestinationType) != null;
        }

        public static bool CanEmitLoadAndSetValue(
            MapperTypeMemberInfo source,
            MapperTypeMemberInfo destination)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destination);

            if (source.Type == null ||
                destination.Type == null ||
                !source.HasGetMethod ||
                !destination.HasSetMethod)
                return false;

            return CanEmitConvert(source.Type, destination.Type);
        }

        void EmitConvertTo(
            Type nonNullableSourceType,
            Type nonNullableDestinationType)
        {
            MethodInfo method = GetConvertToMethodInfo(nonNullableSourceType, nonNullableDestinationType) ??
                throw new Exception($"Cannot convert from {nonNullableSourceType} to {nonNullableDestinationType}");
            EmitCallMethod(method);
        }

        void EmitConvertToObject(Type nonNullableSourceType)
        {
            if (nonNullableSourceType != typeof(object))
                ILGenerator.Emit(OpCodes.Box, nonNullableSourceType);
        }

        void EmitConvertToString(Type nonNullableSourceType)
        {
            if (nonNullableSourceType.IsEnum)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                EmitCallMethod(typeof(ILConverters.Enum<>).MakeGenericType(nonNullableSourceType).GetMethod(nameof(Enum.GetName), [nonNullableSourceType]));
#pragma warning restore CS8604 // Possible null reference argument.
            }
            else if (nonNullableSourceType != typeof(string))
            {
                if (nonNullableSourceType != typeof(object))
                    Emit(OpCodes.Box, nonNullableSourceType);

                EmitCallMethod(ObjectToString);
            }
        }

        void EmitConvertToChar(
            Type nonNullableSourceType)
        {
            if (nonNullableSourceType == typeof(string))
            {
#pragma warning disable CS8604 // Possible null reference argument.
                EmitCallMethod(typeof(bool).GetMethod(nameof(bool.Parse), [typeof(string)]));
#pragma warning restore CS8604 // Possible null reference argument.
            }
            else if (MapperTypeInfo.IsEnum(nonNullableSourceType))
            {
                EmitConvertToString(nonNullableSourceType);
                EmitLdc_I4(0);
#pragma warning disable CS8604 // Possible null reference argument.
                EmitCallMethod(typeof(string).GetProperties().First(p => p.GetIndexParameters().Length > 0).GetGetMethod());
#pragma warning restore CS8604 // Possible null reference argument.
            }
            else
            {
                EmitConvertTo(nonNullableSourceType, typeof(char));
            }
        }

        void EmitConvertToNumeric(
            Type nonNullableSourceType,
            Type nonNullableDestinationType)
        {
            if (MapperTypeInfo.IsEnum(nonNullableSourceType))
            {
                if (nonNullableDestinationType != typeof(decimal))
                    EmitConvertToNonNullableNumeric(nonNullableDestinationType);
                else
                    EmitConvertTo(Enum.GetUnderlyingType(nonNullableSourceType), nonNullableDestinationType);
            }
            else if (MapperTypeInfo.IsNumeric(nonNullableSourceType) &&
                nonNullableSourceType != typeof(decimal) &&
                nonNullableDestinationType != typeof(decimal))
            {
                EmitConvertToNonNullableNumeric(nonNullableDestinationType);
            }
            else
            {
                EmitConvertTo(nonNullableSourceType, nonNullableDestinationType);
            }
        }

        void EmitConvertToEnum(
            Type nonNullableSourceType,
            Type nonNullableDestinationType)
        {
            if (nonNullableSourceType == typeof(string))
            {
                EmitCallMethod(EnumParse.MakeGenericMethod(nonNullableDestinationType));
            }
            else if (nonNullableSourceType == typeof(char))
            {
                EmitConvertToString(nonNullableSourceType);
                EmitCallMethod(EnumParse.MakeGenericMethod(nonNullableDestinationType));
            }
            else if (MapperTypeInfo.IsNumeric(nonNullableSourceType))
            {
                if (nonNullableSourceType != typeof(decimal))
                    EmitConvertToNonNullableNumeric(Enum.GetUnderlyingType(nonNullableDestinationType));
                else
                    EmitConvertTo(nonNullableSourceType, Enum.GetUnderlyingType(nonNullableDestinationType));
            }
            else if (MapperTypeInfo.IsEnum(nonNullableSourceType))
            {
                EmitConvertToString(nonNullableSourceType);
                EmitCallMethod(EnumParse.MakeGenericMethod(nonNullableDestinationType));
            }
            else
            {
                EmitConvertTo(nonNullableSourceType, nonNullableDestinationType);
            }
        }

        public void EmitConvert(
            Type source,
            Type destination)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destination);

            if (source == destination)
                return;

            Type? nonNullableSourceType = source;

            if (Nullable.GetUnderlyingType(source) != null)
            {
#pragma warning disable CS8602, CS8604 // Dereference of a possibly null reference. Possible null reference argument.
                EmitCallMethod(typeof(ILNullables).GetMethod(nameof(ILNullables.GetValueOrDefault)).MakeGenericMethod(Nullable.GetUnderlyingType(source)));
#pragma warning restore CS8602, CS8604 // Dereference of a possibly null reference. Possible null reference argument.
                nonNullableSourceType = Nullable.GetUnderlyingType(source);
            }

            if (destination == typeof(object))
            {
#pragma warning disable CS8604 // Possible null reference argument.
                EmitConvertToObject(nonNullableSourceType);
#pragma warning restore CS8604 // Possible null reference argument.
                return;
            }

            if (destination == typeof(string))
            {
#pragma warning disable CS8604 // Possible null reference argument.
                EmitConvertToString(nonNullableSourceType);
#pragma warning restore CS8604 // Possible null reference argument.
                return;
            }

            Type? underlyingDestinationType = Nullable.GetUnderlyingType(destination);
            Type nonNullableDestinationType = underlyingDestinationType ?? destination;

            if (nonNullableDestinationType != null && nonNullableSourceType != null && nonNullableSourceType != nonNullableDestinationType)
            {
                if (nonNullableDestinationType == typeof(char))
                    EmitConvertToChar(nonNullableSourceType);
                else if (MapperTypeInfo.IsNumeric(nonNullableDestinationType))
                    EmitConvertToNumeric(nonNullableSourceType, nonNullableDestinationType);
                else if (MapperTypeInfo.IsEnum(nonNullableDestinationType))
                    EmitConvertToEnum(nonNullableSourceType, nonNullableDestinationType);
                else
                    EmitConvertTo(nonNullableSourceType, nonNullableDestinationType);
            }

            if (underlyingDestinationType != null)
#pragma warning disable CS8604 // Possible null reference argument.
                Emit(OpCodes.Newobj, typeof(Nullable<>).MakeGenericType(underlyingDestinationType).GetConstructor([underlyingDestinationType]));
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public void EmitLoadAndSetValue(
           Action load,
           MapperTypeMemberInfo source,
           MapperTypeMemberInfo destination)
        {
            if (load == null) throw new ArgumentException(null, nameof(load));
            if (source == null || source.Type == null) throw new ArgumentException(null, nameof(source));
            if (destination == null || destination.Type == null) throw new ArgumentException(null, nameof(destination));

            if (source.Type == destination.Type)
            {
                load();
                EmitSetMemberValue(destination);

                return;
            }

            if (MapperTypeInfo.IsBuiltIn(source.Type) != MapperTypeInfo.IsBuiltIn(destination.Type))
                throw new Exception($"Cannot {nameof(EmitLoadAndSetValue)} from {source} to {destination}");

            load();
            EmitConvert(source.Type, destination.Type);
            EmitSetMemberValue(destination);
        }
    }
}