using Air.Mapper;
using AutoFixture;
using Models;
using System.Collections.Generic;
using Xunit;

namespace Playground
{
    [Collection(nameof(Playground))]
    public class MapperConfig
    {
        private Fixture Fixture { get; }

        public MapperConfig()
        {
            Fixture = new Fixture();
        }

        [Fact]
        public void MapRootMember()
        {
            MapperConfig<TC0_I0_Members, TC0_I0_Members>.SetOptions(o => o
                .Map(s => s.Int32Member, d => d.StringMember),
                true);

            var source = Fixture.Create<TC0_I0_Members>();
            var mapFunc = Mapper<TC0_I0_Members, TC0_I0_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.Equal(source.Int32Member.ToString(), destination.StringMember);

            destination = null;
            var mapAction = Mapper<TC0_I0_Members, TC0_I0_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.Equal(source.Int32Member.ToString(), destination.StringMember);
        }

        [Fact]
        public void MapN1Member()
        {
            MapperConfig<TC2C1C0_I0_Members, TC2C1C0_I0_Members>.SetOptions(o => o
                .Map(s => s.N1.N0.Int32Member, d => d.N1.N0.StringMember),
                true);

            var source = Fixture.Create<TC2C1C0_I0_Members>();
            var mapFunc = Mapper<TC2C1C0_I0_Members, TC2C1C0_I0_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.Equal(source.N1.N0.Int32Member.ToString(), destination.N1.N0.StringMember);

            destination = null;
            var mapAction = Mapper<TC2C1C0_I0_Members, TC2C1C0_I0_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.Equal(source.N1.N0.Int32Member.ToString(), destination.N1.N0.StringMember);
        }

        [Fact]
        public void MapMemberAfterIgnoreNode()
        {
            MapperConfig<TC2C1C0_I0_Members, TC2C1C0_I0_Members>.SetOptions(o => o
                .Ignore(i => i.N1.N0)
                .Map(s => s.N1.N0.Int32Member, d => d.N1.N0.StringMember),
                true);

            var source = Fixture.Create<TC2C1C0_I0_Members>();
            var mapFunc = Mapper<TC2C1C0_I0_Members, TC2C1C0_I0_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.Equal(source.N1.N0.Int32Member.ToString(), destination.N1.N0.StringMember);

            destination = null;
            var mapAction = Mapper<TC2C1C0_I0_Members, TC2C1C0_I0_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.Equal(source.N1.N0.Int32Member.ToString(), destination.N1.N0.StringMember);
        }

        [Fact]
        public void IgnoreNodeAfterMapMember()
        {
            MapperConfig<TC2C1C0_I0_Members, TC2C1C0_I0_Members>.SetOptions(o => o
                .Map(s => s.N1.N0.Int32Member, d => d.N1.N0.StringMember)
                .Ignore(i => i.N1.N0),
                true);

            var source = Fixture.Create<TC2C1C0_I0_Members>();
            var mapFunc = Mapper<TC2C1C0_I0_Members, TC2C1C0_I0_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.Null(destination.N1);

            destination = null;
            var mapAction = Mapper<TC2C1C0_I0_Members, TC2C1C0_I0_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.Null(destination.N1);
        }

        [Fact]
        public void ApplyNestedTypeOptions()
        {
            MapperConfig<TC0_I0_Members, TC0_I0_Members>.SetOptions(o => o
                .Map(s => s.Int32Member, d => d.StringMember),
                true);

            var source = Fixture.Create<TC2C1C0_I0_Members>();
            var mapFunc = Mapper<TC2C1C0_I0_Members, TC2C1C0_I0_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.Equal(source.N1.N0.Int32Member.ToString(), destination.N1.N0.StringMember);

            destination = null;
            var mapAction = Mapper<TC2C1C0_I0_Members, TC2C1C0_I0_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.Equal(source.N1.N0.Int32Member.ToString(), destination.N1.N0.StringMember);
        }

        [Fact]
        public void MapKeyValuePair()
        {
            MapperConfig<KeyValuePair<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>>.SetOptions(o => o
                .Map(s => s.Value.Int32Member, d => d.Key),
                true);

            var source = Fixture.Create<List<KeyValuePair<int, TC0_I0_Members>>>();
            var mapFunc = Mapper<KeyValuePair<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>>.CompileFunc();

            foreach (var item in source)
            {
                var destination = mapFunc(item);
                Assert.Equal(item.Value.Int32Member, destination.Key);
            }
        }
    }
}
