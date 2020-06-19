using Air.Mapper;
using AutoFixture;
using Models;
using Xunit;

namespace Playground
{
    public class Enumerables
    {
        private Fixture Fixture { get; }

        public Enumerables()
        {
            Fixture = new Fixture();
        }

        [Fact]
        public void TArrayToTEnumerable()
        {
            var source = Fixture.Create<TC1_I0_Array_Members>();
            var mapFunc = Mapper<TC1_I0_Array_Members, TC1_I10_TEnumerable_Members>.CompileFunc();
            var destination = mapFunc(source);

            var mapAction = Mapper<TC1_I0_Array_Members, TC1_I10_TEnumerable_Members>.CompileActionRef();
            mapAction(source, ref destination);
        }

        [Fact]
        public void TEnumerableToArray()
        {
            var source = Mapper<TC1_I0_Array_Members, TC1_I10_TEnumerable_Members>.Map(Fixture.Create<TC1_I0_Array_Members>());
            var mapFunc = Mapper<TC1_I10_TEnumerable_Members, TC1_I0_Array_Members>.CompileFunc();
            var destination = mapFunc(source);

            var mapAction = Mapper<TC1_I10_TEnumerable_Members, TC1_I0_Array_Members>.CompileActionRef();
            mapAction(source, ref destination);
        }

        [Fact]
        public void TDictionaryToDictionary()
        {
           
        }
    }
}
