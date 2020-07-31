using Air.Mapper;
using AutoFixture;
using Models;
using Xunit;
using static Air.Compare.Members;

namespace Playground
{
    public class Collections
    {
        private Fixture Fixture { get; }

        public Collections()
        {
            Fixture = new Fixture();
        }

        [Fact]
        public void Example()
        {
            var source = new TDictionary<TC0_I0_Members>();
            Mapper<TC0_I0_Members, TC0_I0_Members>.ToDictionary<int, long>(source);

            var destination = new TDictionary<TC0_I0_Members>(source ?? null);

        }

        [Fact]
        public void C1_I0_Array_To_C1_I10_TEnumerable()
        {
            var source = Fixture.Create<TC1_I0_Array_Members>();
            var mapFunc = Mapper<TC1_I0_Array_Members, TC1_I10_TEnumerable_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            var mapAction = Mapper<TC1_I0_Array_Members, TC1_I10_TEnumerable_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void C1_I10_TEnumerable_To_C1_I0_Array()
        {
            var source = Mapper<TC1_I0_Array_Members, TC1_I10_TEnumerable_Members>.Map(Fixture.Create<TC1_I0_Array_Members>());
            var mapFunc = Mapper<TC1_I10_TEnumerable_Members, TC1_I0_Array_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            var mapAction = Mapper<TC1_I10_TEnumerable_Members, TC1_I0_Array_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void C1_I11_Dictionary_To_C1_I11_Dictionary()
        {
            var source = Mapper<TC1_I11_Dictionary_Members, TC1_I11_Dictionary_Members>.Map(Fixture.Create<TC1_I11_Dictionary_Members>());
            var mapFunc = Mapper<TC1_I11_Dictionary_Members, TC1_I11_Dictionary_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            var mapAction = Mapper<TC1_I11_Dictionary_Members, TC1_I11_Dictionary_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void C1_I15_TDictionary_To_C1_I11_Dictionary()
        {
            var source = Mapper<TC1_I11_Dictionary_Members, TC1_I15_TDictionary_Members>.Map(Fixture.Create<TC1_I11_Dictionary_Members>());
            var mapFunc = Mapper<TC1_I15_TDictionary_Members, TC1_I11_Dictionary_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            var mapAction = Mapper<TC1_I15_TDictionary_Members, TC1_I11_Dictionary_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void C1_I11_Dictionary_To_C1_I15_TDictionary()
        {
            var il = Mapper<TC1_I11_Dictionary_Members, TC1_I15_TDictionary_Members>.ViewFuncIL();

            var source = Fixture.Create<TC1_I11_Dictionary_Members>();
            var mapFunc = Mapper<TC1_I11_Dictionary_Members, TC1_I15_TDictionary_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            var mapAction = Mapper<TC1_I11_Dictionary_Members, TC1_I15_TDictionary_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

    }
}
