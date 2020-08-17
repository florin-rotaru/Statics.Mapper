using Air.Mapper;
using Air.Mapper.Internal;
using AutoFixture;
using Models;
using System.Collections.Generic;
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
            //var il = Mapper<KeyValuePair<int, int>, KeyValuePair<int, int>>.ViewActionRefIL();
            var il = Mapper<TC1_I11_Dictionary_Members, TC1_I11_Dictionary_Members>.ViewActionRefIL();

            var source = Mapper<TC1_I11_Dictionary_Members, TC1_I11_Dictionary_Members>.Map(Fixture.Create<TC1_I11_Dictionary_Members>());
            var mapFunc = Mapper<TC1_I11_Dictionary_Members, TC1_I11_Dictionary_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = new TC1_I11_Dictionary_Members();
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
