using Air.Mapper;
using AutoFixture;
using Models;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Playground
{
    public class ConvertMember
    {
        protected readonly ITestOutputHelper Console;

        protected Fixture Fixture { get; }

        public ConvertMember(ITestOutputHelper console)
        {
            Console = console;
            Fixture = new Fixture();
        }
        [Fact]
        public void FromStringToBool()
        {
            var source = new TC0_I0_Members
            {
                StringMember = true.ToString()
            };
            var map = Mapper<TC0_I0_Members, TC0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.StringMember, d => d.BooleanMember));
            var destination = map(source);

            Assert.Equal(Convert.ToBoolean(source.StringMember), destination.BooleanMember);
        }

        [Fact]
        public void FromBoolToString()
        {
            var source = Fixture.Create<TC0_I0_Members>();
            var map = Mapper<TC0_I0_Members, TC0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.BooleanMember, m => m.StringMember));
            var destination = map(source);

            Assert.Equal(Convert.ToString(source.BooleanMember), destination.StringMember);
        }

        [Fact]
        public void FromIntToBool()
        {
            var source = Fixture.Create<TC0_I0_Members>();
            var map = Mapper<TC0_I0_Members, TC0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.Int32Member, m => m.BooleanMember));
            var destination = map(source);

            Assert.True(destination.BooleanMember);

            source.Int32Member = 0;
            destination = map(source);

            Assert.False(destination.BooleanMember);
        }

        [Fact]
        public void FromIntToDouble()
        {
            var source = Fixture.Create<TC0_I0_Members>();
            var map = Mapper<TC0_I0_Members, TC0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.Int32Member, t => t.DoubleMember));
            var destination = map(source);

            Assert.Equal(Convert.ToDouble(source.Int32Member), destination.DoubleMember);
        }

        [Fact]
        public void FromNullableBoolToDouble()
        {
            var source = new TC0_I1_Nullable_Members
            {
                BooleanMember = null
            };
            var map = Mapper<TC0_I1_Nullable_Members, TC0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.BooleanMember, t => t.DoubleMember));
            var destination = map(source);

            Assert.True(destination.DoubleMember == default(double));

            source = new TC0_I1_Nullable_Members
            {
                BooleanMember = true
            };
            destination = map(source);

            Assert.Equal(Convert.ToDouble(source.BooleanMember), destination.DoubleMember);
        }

        [Fact]
        public void FromIntToEnum()
        {
            var source = new TC0_I0_Members
            {
                Int32Member = (int)TUndefinedABCEnum.B
            };
            var map = Mapper<TC0_I0_Members, TC0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.Int32Member, t => t.UndefinedEnumMember));
            var destination = map(source);

            Assert.Equal(source.Int32Member, (int)destination.UndefinedEnumMember);
        }

        [Fact]
        public void FromStringToEnum()
        {
            var source = new TC0_I0_Members
            {
                StringMember = TUndefinedABCEnum.B.ToString()
            };
            var map = Mapper<TC0_I0_Members, TC0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.StringMember, t => t.UndefinedEnumMember));
            var destination = map(source);

            Assert.Equal(Enum.Parse<TUndefinedABCEnum>(source.StringMember), destination.UndefinedEnumMember);
        }

        [Fact]
        public void FromEnumToDecimal()
        {
            var source = new TC0_I0_Members
            {
                UndefinedEnumMember = TUndefinedABCEnum.B
            };
            var map = Mapper<TC0_I0_Members, TC0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.UndefinedEnumMember, t => t.DecimalMember));
            var destination = map(source);

            Assert.Equal(Convert.ToDecimal(source.UndefinedEnumMember), destination.DecimalMember);
        }

        [Fact]
        public void FromEnumToInt()
        {
            var source = new TC0_I0_Members
            {
                UndefinedEnumMember = TUndefinedABCEnum.B
            };
            var map = Mapper<TC0_I0_Members, TC0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.UndefinedEnumMember, t => t.Int32Member));

            var destination = map(source);

            Assert.Equal((int)source.UndefinedEnumMember, destination.Int32Member);
        }

        [Fact]
        public void FromEnumToString()
        {
            var source = new TC0_I0_Members
            {
                UndefinedEnumMember = TUndefinedABCEnum.B
            };
            var map = Mapper<TC0_I0_Members, TC0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.UndefinedEnumMember, t => t.StringMember));
            var destination = map(source);

            Assert.Equal(Convert.ToString(source.UndefinedEnumMember), destination.StringMember);
        }

        [Fact]
        public void FromEnumToDtoEnum()
        {
            var source = new TC0_I0_Members
            {
                UndefinedEnumMember = TUndefinedABCEnum.B
            };
            var map = Mapper<TC0_I0_Members, TC0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.UndefinedEnumMember, t => t.EnumMember));
            var destination = map(source);

            Assert.Equal(source.UndefinedEnumMember.ToString(), destination.EnumMember.ToString());
        }
    }
}
