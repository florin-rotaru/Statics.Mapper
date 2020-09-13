using Air.Mapper;
using AutoFixture;
using Models;
using System.Collections.Generic;
using Xunit;
using static Air.Compare.Members;

namespace Playground
{
    [Collection(nameof(MapperConfig))]
    public class MapperConfig
    {
        private Fixture Fixture { get; }

        public MapperConfig()
        {
            Fixture = new Fixture();
        }

        public class TC0_I0_Members_Local : TC0_I0_Members { }
        public class TC1C0_I0_Members_Local { public TC0_I0_Members_Local N0 { get; set; } }
        public class TC2C1C0_I0_Members_Local { public TC1C0_I0_Members_Local N1 { get; set; } }


        [Fact]
        public void MapRootMember()
        {
            MapperConfig<TC0_I0_Members_Local, TC0_I0_Members_Local>.SetOptions(o => o
                .Map(s => s.Int32Member, d => d.StringMember),
                true);

            var source = Fixture.Create<TC0_I0_Members_Local>();
            var mapFunc = Mapper<TC0_I0_Members_Local, TC0_I0_Members_Local>.CompileFunc();
            var destination = mapFunc(source);
            Assert.Equal(source.Int32Member.ToString(), destination.StringMember);

            destination = null;
            var mapAction = Mapper<TC0_I0_Members_Local, TC0_I0_Members_Local>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.Equal(source.Int32Member.ToString(), destination.StringMember);
        }

        [Fact]
        public void MapN1Member()
        {
            MapperConfig<TC2C1C0_I0_Members_Local, TC2C1C0_I0_Members_Local>.SetOptions(o => o
                .Map(s => s.N1.N0.Int32Member, d => d.N1.N0.StringMember),
                true);

            var source = Fixture.Create<TC2C1C0_I0_Members_Local>();
            var mapFunc = Mapper<TC2C1C0_I0_Members_Local, TC2C1C0_I0_Members_Local>.CompileFunc();
            var destination = mapFunc(source);
            Assert.Equal(source.N1.N0.Int32Member.ToString(), destination.N1.N0.StringMember);

            destination = null;
            var mapAction = Mapper<TC2C1C0_I0_Members_Local, TC2C1C0_I0_Members_Local>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.Equal(source.N1.N0.Int32Member.ToString(), destination.N1.N0.StringMember);
        }

        [Fact]
        public void MapMemberAfterIgnoreNode()
        {
            MapperConfig<TC2C1C0_I0_Members_Local, TC2C1C0_I0_Members_Local>.SetOptions(o => o
                .Ignore(i => i.N1.N0)
                .Map(s => s.N1.N0.Int32Member, d => d.N1.N0.StringMember),
                true);

            var source = Fixture.Create<TC2C1C0_I0_Members_Local>();
            var mapFunc = Mapper<TC2C1C0_I0_Members_Local, TC2C1C0_I0_Members_Local>.CompileFunc();
            var destination = mapFunc(source);
            Assert.Equal(source.N1.N0.Int32Member.ToString(), destination.N1.N0.StringMember);

            destination = null;
            var mapAction = Mapper<TC2C1C0_I0_Members_Local, TC2C1C0_I0_Members_Local>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.Equal(source.N1.N0.Int32Member.ToString(), destination.N1.N0.StringMember);
        }

        [Fact]
        public void IgnoreNodeAfterMapMember()
        {
            MapperConfig<TC2C1C0_I0_Members_Local, TC2C1C0_I0_Members_Local>.SetOptions(o => o
                .Map(s => s.N1.N0.Int32Member, d => d.N1.N0.StringMember)
                .Ignore(i => i.N1.N0),
                true);

            var source = Fixture.Create<TC2C1C0_I0_Members_Local>();
            var mapFunc = Mapper<TC2C1C0_I0_Members_Local, TC2C1C0_I0_Members_Local>.CompileFunc();
            var destination = mapFunc(source);
            Assert.Null(destination.N1);

            destination = null;
            var mapAction = Mapper<TC2C1C0_I0_Members_Local, TC2C1C0_I0_Members_Local>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.Null(destination.N1);
        }

        [Fact]
        public void ApplyNestedTypeOptions()
        {
            MapperConfig<TC0_I0_Members_Local, TC0_I0_Members_Local>.SetOptions(o => o
                .Map(s => s.Int32Member, d => d.StringMember),
                true);

            var source = Fixture.Create<TC2C1C0_I0_Members_Local>();
            var mapFunc = Mapper<TC2C1C0_I0_Members_Local, TC2C1C0_I0_Members_Local>.CompileFunc();
            var destination = mapFunc(source);
            Assert.Equal(source.N1.N0.Int32Member.ToString(), destination.N1.N0.StringMember);

            destination = null;
            var mapAction = Mapper<TC2C1C0_I0_Members_Local, TC2C1C0_I0_Members_Local>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.Equal(source.N1.N0.Int32Member.ToString(), destination.N1.N0.StringMember);
        }

        [Fact]
        public void SetMap()
        {
            MapperConfig<TC1C0_I0_Members_Local, TC1C0_I0_Members_Local>.SetMap(
                (source) => new TC1C0_I0_Members_Local { N0 = new TC0_I0_Members_Local { Int32Member = source.N0.Int32Member } },
                (TC1C0_I0_Members_Local source, ref TC1C0_I0_Members_Local destination) => destination.N0 = new TC0_I0_Members_Local { Int32Member = source.N0.Int32Member });

            var source = Fixture.Create<TC1C0_I0_Members_Local>();

            var destination = new TC1C0_I0_Members_Local();
            Mapper<TC1C0_I0_Members_Local, TC1C0_I0_Members_Local>.Map(source, ref destination);
            Assert.Equal(source.N0.Int32Member, destination.N0.Int32Member);
            Assert.Null(destination.N0.StringMember);

            destination = Mapper<TC1C0_I0_Members_Local, TC1C0_I0_Members_Local>.Map(source);
            Assert.Equal(source.N0.Int32Member, destination.N0.Int32Member);
            Assert.Null(destination.N0.StringMember);

            MapperConfig<TC1C0_I0_Members_Local, TC1C0_I0_Members_Local>.ClearMap();

            destination = Mapper<TC1C0_I0_Members_Local, TC1C0_I0_Members_Local>.Map(source);
            Assert.True(CompareEquals(source, destination));

            destination = new TC1C0_I0_Members_Local();

            Mapper<TC1C0_I0_Members_Local, TC1C0_I0_Members_Local>.Map(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void MapKeyValuePair()
        {
            MapperConfig<KeyValuePair<int, TC0_I0_Members_Local>, KeyValuePair<int, TC0_I0_Members_Local>>.SetOptions(o => o
                .Map(s => s.Value.Int32Member, d => d.Key),
                true);

            var source = Fixture.Create<List<KeyValuePair<int, TC0_I0_Members_Local>>>();
            var mapFunc = Mapper<KeyValuePair<int, TC0_I0_Members_Local>, KeyValuePair<int, TC0_I0_Members_Local>>.CompileFunc();

            foreach (var item in source)
            {
                var destination = mapFunc(item);
                Assert.Equal(item.Value.Int32Member, destination.Key);
            }
        }
    }
}
