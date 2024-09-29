using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Statics.Mapper.Internal
{
    internal class MapperTypeMemberInfo
    {
        public string Name => MemberInfo.Name;

        public MemberTypes MemberType => MemberInfo.MemberType;
        public MemberInfo MemberInfo { get; }

        public Type MemberOf { get; }

        public Type Type { get; }

        public bool IsEnum { get; set; }
        public bool IsBuiltIn { get; set; }
        public bool IsEnumerable { get; set; }
        public bool IsNumeric { get; set; }
        public bool HasDefaultConstructor { get; set; }
        public bool IsLiteral { get; set; }

        public bool IsStatic { get; set; }

        public bool HasGetMethod { get; }
        public bool HasSetMethod { get; }
        public object DefaultValue { get; }
        public Func<object, object> GetValue { get; }

        public MapperTypeMemberInfo(Type type, PropertyInfo property)
        {
            MemberOf = type;
            MemberInfo = property;
            Type = property.PropertyType;

            SetFlags();
            IsStatic = property.GetGetMethod().Attributes.HasFlag(MethodAttributes.Static);

            HasGetMethod = ((property.DeclaringType != null) ? property.GetGetMethod() : property.DeclaringType.GetProperty(property.Name).GetGetMethod()) != null;
            HasSetMethod = ((property.DeclaringType != null) ? property.GetSetMethod() : property.DeclaringType.GetProperty(property.Name).GetSetMethod()) != null;

            GetValue = HasGetMethod ? CompileGetValue(type, property) : null;
            DefaultValue = MapperTypeInfo.GetDefaultValue(type, property);
        }

        public MapperTypeMemberInfo(Type type, FieldInfo field)
        {
            MemberOf = type;
            MemberInfo = field;
            Type = field.FieldType;

            SetFlags();
            IsStatic = field.Attributes.HasFlag(FieldAttributes.Static);

            HasGetMethod = true;
            HasSetMethod = !IsLiteral;

            DefaultValue = MapperTypeInfo.GetDefaultValue(type, field);
            GetValue = CompileGetValue(type, field, DefaultValue);
        }

        static bool HasLiteralOrInitOnlyFlag(FieldInfo fieldInfo) =>
            fieldInfo.Attributes.HasFlag(FieldAttributes.Literal) ||
                fieldInfo.Attributes.HasFlag(FieldAttributes.InitOnly);

        void SetFlags()
        {
            IsEnum = MapperTypeInfo.IsEnum(Type);
            IsBuiltIn = MapperTypeInfo.IsBuiltIn(Type);
            IsEnumerable = MapperTypeInfo.IsEnumerable(Type);
            IsNumeric = MapperTypeInfo.IsNumeric(Type);

            HasDefaultConstructor = IsBuiltIn || Type.GetConstructor(Type.EmptyTypes) != null;
            IsLiteral = MemberInfo.MemberType == MemberTypes.Field && HasLiteralOrInitOnlyFlag((FieldInfo)MemberInfo);
        }

        static void CreateSignature(out DynamicMethod dynamicMethod, out IL il)
        {
            dynamicMethod = new DynamicMethod($"{nameof(Statics)}{Guid.NewGuid():N}", typeof(object), [typeof(object)], false);
            il = new IL(dynamicMethod.GetILGenerator(), true);
        }

        static Func<object, object> CompileGetValue(Type type, PropertyInfo property)
        {
            if (property.GetIndexParameters().Length != 0)
                throw new NotSupportedException();

            CreateSignature(out DynamicMethod dynamicMethod, out IL il);

            if (property.GetGetMethod().Attributes.HasFlag(MethodAttributes.Static))
            {
                il.Emit(OpCodes.Call, property.GetGetMethod());
            }
            else if (type.IsValueType)
            {
                LocalBuilder local = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, type);
                il.EmitStloc(local.LocalIndex);
                il.EmitLdloca(local.LocalIndex);
                il.Emit(OpCodes.Call, property.GetGetMethod());
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, type);

                il.EmitCallMethod(property.GetGetMethod());
            }

            if (property.PropertyType.IsValueType)
                il.Emit(OpCodes.Box, property.PropertyType);

            il.Emit(OpCodes.Ret);
            return (Func<object, object>)dynamicMethod.CreateDelegate(typeof(Func<object, object>));
        }

        static Func<object, object> CompileGetValue(Type type, FieldInfo field, object defaultValue)
        {
            CreateSignature(out DynamicMethod dynamicMethod, out IL il);

            if (field.Attributes.HasFlag(FieldAttributes.Literal))
            {
                il.EmitLoadLiteral(field.FieldType, defaultValue);
            }
            else if (field.Attributes.HasFlag(FieldAttributes.Static))
            {
                il.Emit(OpCodes.Ldsfld, field);
            }
            else if (type.IsValueType)
            {
                LocalBuilder local = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, type);
                il.EmitStloc(local.LocalIndex);
                il.EmitLdloca(local.LocalIndex);
                il.Emit(OpCodes.Ldfld, field);
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Ldfld, field);
            }

            if (field.FieldType.IsValueType)
                il.Emit(OpCodes.Box, field.FieldType);

            il.Emit(OpCodes.Ret);

            return (Func<object, object>)dynamicMethod.CreateDelegate(typeof(Func<object, object>));
        }
    }
}