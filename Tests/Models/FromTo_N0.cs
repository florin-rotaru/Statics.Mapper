using Air.Mapper;
using AutoFixture;
using Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Air.Compare.Members;

namespace Internal
{
    public class FromTo_N0<S> where S : new()
    {
        private readonly ITestOutputHelper Console;
        private Fixture Fixture { get; }

        public FromTo_N0(ITestOutputHelper console)
        {
            Console = console;
            Fixture = new Fixture();
        }

        private S NewSource(bool int32MemberOnly = false)
        {
            var members = int32MemberOnly ?
                new TC0_I0_Members { Int32Member = Fixture.Create<int>() } :
                Fixture.Create<TC0_I0_Members>();

            var source = Mapper<TC0_I0_Members, S>.Map(members);
            source = source != null ? source : new S();

            if (source != null)
                return source;

            var nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(S));
            if (nullableUnderlyingType != null)
            {
                var undelyingInstance = Activator.CreateInstance(nullableUnderlyingType);
                source = (S)Activator.CreateInstance(typeof(S), new[] { undelyingInstance });
            }

            return source;
        }

        private S[] NewSourceArray()
        {
            S[] array = new S[3];
            for (int i = 0; i < 3; i++)
                array[i] = NewSource();

            return array;
        }

        private static S DefaultSource() => default;

        private static bool CanSerialize<D>(S source, D destination)
        {
            return JsonConvert.SerializeObject(source) != null &&
                JsonConvert.SerializeObject(destination) != null;
        }

        private static void AssertInstanceOrDefaultReadonly<D>(S source, D destination) where D : new()
        {
            Assert.True(CanSerialize(source, destination));

            if (source != null)
            {
                Assert.NotNull(destination);
            }
            else
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                    Assert.True(CompareEquals(new D(), destination));
                else
                    Assert.Null(destination);
            }
        }

        private static void AssertEqualsOrDefault<D>(
           S[] source,
           D[] destination,
           bool hasReadonlyMembers,
           bool hasStaticMembers) where D : new()
        {
            if (hasStaticMembers)
                return;

            for (int i = 0; i < source.Length; i++)
                AssertEqualsOrDefault(source[i], destination[i], hasReadonlyMembers);
        }

        private static void AssertEqualsOrDefault<D>(
            S source,
            D destination,
            bool hasReadonlyMembers) where D : new()
        {
            if (hasReadonlyMembers)
            {
                AssertInstanceOrDefaultReadonly(source, destination);
                return;
            }

            Assert.True(CanSerialize(source, destination));

            if (source == null && Nullable.GetUnderlyingType(typeof(D)) == null && typeof(D).IsValueType)
                Assert.True(CompareEquals(destination, new D()));
            else
                Assert.True(CompareEquals(source, destination));
        }

        private static void AssertDefault<D>(
            S source,
            D destination,
            bool hasReadonlyMembers) where D : new()
        {
            if (hasReadonlyMembers)
            {
                AssertInstanceOrDefaultReadonly(source, destination);
                return;
            }

            Assert.True(CanSerialize(source, destination));

            if (Nullable.GetUnderlyingType(typeof(S)) != null)
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                    Assert.True(CompareEquals(new D(), destination));
                else
                    Assert.Null(destination);
            }
            else if (typeof(S).IsValueType)
            {
                Assert.True(CompareEquals(source, destination, ignoreDefaultLeftValues: true));
            }
            else
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                    Assert.True(CompareEquals(new D(), destination));
                else
                    Assert.Null(destination);
            }
        }

        private static object MemberValue<T>(T source, string memberName)
        {
            return Air.Reflection.TypeInfo.GetMembers(typeof(T), true)
                .First(m => m.Name == memberName)
                .GetValue(source);
        }

        private static void AssertIntToStringEquals<D>(S source, D destination)
        {
            Assert.True(
                CompareEquals(
                    MemberValue(source, "Int32Member"),
                    MemberValue(destination, "StringMember"),
                    ignoreDefaultLeftValues: true,
                    useConvert: true));
        }

        private static void AssertDefaultStringMemberValue<D>(S source, D destination) where D : new()
        {
            if (Nullable.GetUnderlyingType(typeof(S)) != null)
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                {
                    if (source != null)
                        Assert.Null(MemberValue(destination, "StringMember"));
                }
                else
                    Assert.Null(destination);
            }
            else if (typeof(S).IsValueType)
            {
                AssertIntToStringEquals(source, destination);
            }
            else
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                    Assert.True(CompareEquals(new D(), destination));
                else
                    Assert.Null(destination);
            }
        }

        private static void AssertIntToDecimalEquals<D>(S source, D destination)
        {
            Assert.True(
                CompareEquals(
                    MemberValue(source, "Int32Member"),
                    MemberValue(destination, "DecimalMember"),
                    ignoreDefaultLeftValues: true,
                    useConvert: true));
        }

        private static void AssertDefaultDecimalMemberValue<D>(S source, D destination) where D : new()
        {
            if (Nullable.GetUnderlyingType(typeof(S)) != null)
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                {
                    if (source != null)
                        Assert.True(
                            CompareEquals(
                                0,
                                MemberValue(destination, "DecimalMember"),
                                ignoreDefaultRightValues: true,
                                useConvert: true));

                }
                else
                    Assert.Null(destination);
            }
            else if (typeof(S).IsValueType)
            {
                AssertIntToDecimalEquals(source, destination);
            }
            else
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                    Assert.True(CompareEquals(new D(), destination));
                else
                    Assert.Null(destination);
            }
        }

        private void MapperConvert<D>(bool hasReadonlyMembers) where D : new()
        {
            if (hasReadonlyMembers)
            {
                Assert.Throws<InvalidOperationException>(() => Mapper<S, D>.CompileActionRef(o => o
                    .Ignore(i => i)
                    .Map("Int32Member", "StringMember")));

                Assert.Throws<InvalidOperationException>(() => Mapper<S, D>.CompileFunc(o => o
                    .Ignore(i => i)
                    .Map("Int32Member", "DecimalMember")));

                return;
            }

            // =======
            var convertActionRef = Mapper<S, D>.CompileActionRef(o => o
                .Ignore(i => i)
                .Map("Int32Member", "StringMember"));

            var convertFunc = Mapper<S, D>.CompileFunc(o => o
                .Ignore(i => i)
                .Map("Int32Member", "DecimalMember"));

            // =======
            var source = NewSource(true);
            var destination = new D();
            convertActionRef(source, ref destination);
            AssertIntToStringEquals(source, destination);

            destination = default;
            convertActionRef(source, ref destination);
            AssertIntToStringEquals(source, destination);

            destination = convertFunc(source);
            AssertIntToDecimalEquals(source, destination);

            // =======
            source = DefaultSource();
            convertActionRef(source, ref destination);
            AssertDefaultStringMemberValue(source, destination);

            destination = convertFunc(source);
            AssertDefaultDecimalMemberValue(source, destination);
        }

        public void ToClass<D>(bool hasReadonlyMembers, bool hasStaticMembers) where D : new()
        {
            // =======
            var mapActionRef = Mapper<S, D>.CompileActionRef();
            var mapFunc = Mapper<S, D>.CompileFunc();

            // =======
            S source = NewSource();
            D destination = new D();
            mapActionRef(source, ref destination);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            destination = new D();
            Mapper<S, D>.Map(source, ref destination);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            destination = default;
            mapActionRef(source, ref destination);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            destination = default;
            Mapper<S, D>.Map(source, ref destination);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            destination = mapFunc(source);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            destination = Mapper<S, D>.Map(source);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            // =======
            source = DefaultSource();
            mapActionRef(source, ref destination);
            AssertDefault(source, destination, hasReadonlyMembers);

            Mapper<S, D>.Map(source, ref destination);
            AssertDefault(source, destination, hasReadonlyMembers);

            destination = mapFunc(source);
            AssertDefault(source, destination, hasReadonlyMembers);

            destination = Mapper<S, D>.Map(source);
            AssertDefault(source, destination, hasReadonlyMembers);

            MapperConvert<D>(hasReadonlyMembers);
            ToArray<D>(hasReadonlyMembers, hasStaticMembers);
        }

        public void ToArray<D>(bool hasReadonlyMembers, bool hasStaticMembers) where D : new()
        {
            // =======
            S[] source = NewSourceArray();
            D[] destination = Mapper<S[], D[]>.Map(source);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers, hasStaticMembers);

            source = Array.Empty<S>();
            destination = Mapper<S[], D[]>.Map(source);
            Assert.Empty(destination);
        }

        public void ToStruct<D>(bool hasReadonlyMembers, bool hasStaticMembers) where D : struct
        {
            // =======
            var mapActionRef = Mapper<S, D>.CompileActionRef();
            var mapFunc = Mapper<S, D>.CompileFunc();

            // =======
            S source = NewSource();
            D destination = new D();
            mapActionRef(source, ref destination);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            destination = new D();
            Mapper<S, D>.Map(source, ref destination);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            destination = new D();
            mapActionRef(source, ref destination);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            destination = new D();
            Mapper<S, D>.Map(source, ref destination);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            destination = mapFunc(source);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            destination = Mapper<S, D>.Map(source);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            // =======
            source = DefaultSource();
            mapActionRef(source, ref destination);
            AssertDefault(source, destination, hasReadonlyMembers);

            Mapper<S, D>.Map(source, ref destination);
            AssertDefault(source, destination, hasReadonlyMembers);

            destination = mapFunc(source);
            AssertDefault(source, destination, hasReadonlyMembers);

            destination = Mapper<S, D>.Map(source);
            AssertDefault(source, destination, hasReadonlyMembers);

            MapperConvert<D>(hasReadonlyMembers);
            ToArray<D>(hasReadonlyMembers, hasStaticMembers);
        }

        public void ToNullableStruct<D>(bool hasReadonlyMembers, bool hasStaticMembers) where D : struct
        {
            // =======
            var mapActionRef = Mapper<S, D?>.CompileActionRef();
            var mapFunc = Mapper<S, D?>.CompileFunc();

            // =======
            S source = NewSource();
            D? destination = new D?();
            mapActionRef(source, ref destination);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            destination = new D?();
            mapActionRef(source, ref destination);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            destination = mapFunc(source);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            // =======
            source = DefaultSource();
            mapActionRef(source, ref destination);
            AssertDefault(source, destination, hasReadonlyMembers);

            destination = mapFunc(source);
            AssertDefault(source, destination, hasReadonlyMembers);

            MapperConvert<D>(hasReadonlyMembers);
            ToArray<D>(hasReadonlyMembers, hasStaticMembers);
        }
    }
}
