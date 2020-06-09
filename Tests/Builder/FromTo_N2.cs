using Air.Mapper;
using AutoFixture;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Air.Compare.Members;

namespace Internal
{
    public class FromTo_N2<S> where S : new()
    {
        protected readonly ITestOutputHelper Console;

        protected Fixture Fixture { get; }

        public FromTo_N2(ITestOutputHelper console)
        {
            Console = console;
            Fixture = new Fixture();
        }

        private S NewSource()
        {
            var members = Fixture.Create<TC2C1C0_I0_Members>();
            var source = Mapper<TC2C1C0_I0_Members, S>.Map(members);
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

        private S DefaultSource() => default;

        private D NullableReadonly<D>()
        {
            return (D)Activator.CreateInstance(
                typeof(D),
                Activator.CreateInstance(Nullable.GetUnderlyingType(typeof(D))));
        }

        private bool CanSerialize<D>(S source, D destination)
        {
            return JsonConvert.SerializeObject(source) != null &&
                JsonConvert.SerializeObject(destination) != null;
        }

        private void AssertEqualsOrDefaultReadonly<D>(S source, D destination) where D : new()
        {
            Assert.True(CanSerialize(source, destination));

            if (Nullable.GetUnderlyingType(typeof(D)) != null)
                Assert.True(CompareEquals(NullableReadonly<D>(), destination));
            else if (typeof(D).IsValueType)
                Assert.True(CompareEquals(new D(), destination));
            else
                Assert.True(CompareEquals(new D(), destination));
        }

        private void AssertEqualsOrDefault<D>(S source, D destination, bool hasReadonlyMembers) where D : new()
        {
            if (hasReadonlyMembers)
                AssertEqualsOrDefaultReadonly(source, destination);

            if (hasReadonlyMembers)
                return;

            Assert.True(CanSerialize(source, destination));

            if (source == null && Nullable.GetUnderlyingType(typeof(D)) == null && typeof(D).IsValueType)
                Assert.True(CompareEquals(destination, new D()));
            else
                Assert.True(CompareEquals(source, destination));
        }

        private void AssertDefaultReadonly<D>(S source, D destination) where D : new()
        {
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
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.True(CompareEquals(NullableReadonly<D>(), destination));
                else if (typeof(D).IsValueType)
                    Assert.True(CompareEquals(new D(), destination));
                else
                    Assert.True(CompareEquals(new D(), destination));
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

        private void AssertDefault<D>(S source, D destination, bool hasReadonlyMembers) where D : new()
        {
            if (hasReadonlyMembers)
                AssertDefaultReadonly(source, destination);

            if (hasReadonlyMembers)
                return;

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

        private string GetStringMemberPath(Type type)
        {
            var path = new List<string>();
            string[] members = new[] { "N1", "N0", "Value", "StringMember" };
            var member = Air.Reflection.TypeInfo.GetMembers(type).FirstOrDefault(m => members.Contains(m.Name));

            while (member != null)
            {
                path.Add(member.Name);
                member = Air.Reflection.TypeInfo.GetMembers(member.Type).FirstOrDefault(m => members.Contains(m.Name));
            }

            return string.Join('.', path);
        }

        private Air.Reflection.MemberInfo GetMemberInfo(Type type)
        {
            string[] members = new[] { "N1", "N0", "Value" };
            var memberInfo = Air.Reflection.TypeInfo.GetMembers(type).FirstOrDefault(m => members.Contains(m.Name));
            if (memberInfo == null)
                return memberInfo;

            return memberInfo.Name == "Value" ? GetMemberInfo(memberInfo.Type) : memberInfo;
        }

        private bool ContainsSameStaticNodes(Type source, Type destination)
        {
            var sourceMember = GetMemberInfo(source);
            var destinationMember = GetMemberInfo(destination);

            while (sourceMember != null)
            {
                if (sourceMember.IsStatic &&
                    destinationMember.IsStatic &&
                    sourceMember.Type == destinationMember.Type)
                    return true;

                sourceMember = GetMemberInfo(sourceMember.Type);
                destinationMember = GetMemberInfo(destinationMember.Type);
            }

            return false;
        }

        private void StringMemberMap<D>(bool hasReadonlyMembers) where D : new()
        {
            if (hasReadonlyMembers)
                return;

            var sourceMemberPath = GetStringMemberPath(typeof(S));
            var destinationMemberPath = GetStringMemberPath(typeof(D));

            if (ContainsSameStaticNodes(typeof(S), typeof(D)))
            {
                Assert.Throws<InvalidOperationException>(
                    () => Mapper<S, D>.CompileActionRef(o => o.Ignore(i => i).Map(sourceMemberPath, destinationMemberPath)));

                Assert.Throws<InvalidOperationException>(
                    () => Mapper<S, D>.CompileFunc(o => o.Ignore(i => i).Map(sourceMemberPath, destinationMemberPath)));

                return;
            }

            // =======
            var convertActionRef = Mapper<S, D>.CompileActionRef(o => o
                .Ignore(i => i)
                .Map(sourceMemberPath, destinationMemberPath));

            var convertFunc = Mapper<S, D>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(sourceMemberPath, destinationMemberPath));

            // =======
            var source = NewSource();
            var destination = new D();
            convertActionRef(source, ref destination);
            destination = convertFunc(source);

            // =======  
            source = DefaultSource();
            convertActionRef(source, ref destination);
            destination = convertFunc(source);
        }

        public void ToClass<D>(bool hasReadonlyMembers) where D : new()
        {
            // =======
            var mapActionRef = Mapper<S, D>.CompileActionRef();
            var mapFunc = Mapper<S, D>.CompileFunc();

            // =======
            S source = NewSource();
            D destination = new D();

            mapActionRef(source, ref destination);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            destination = default;
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

            StringMemberMap<D>(hasReadonlyMembers);
        }

        public void ToStruct<D>(bool hasReadonlyMembers) where D : struct
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

            StringMemberMap<D>(hasReadonlyMembers);
        }

        public void ToNullableStruct<D>(bool hasReadonlyMembers) where D : struct
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

            StringMemberMap<D>(hasReadonlyMembers);
        }
    }
}