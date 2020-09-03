using Air.Mapper;
using AutoFixture;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Air.Compare.Members;

namespace Playground
{
    [Collection(nameof(Playground))]
    public class Options
    {
        private readonly ITestOutputHelper Console;

        private Fixture Fixture { get; }

        public Options(ITestOutputHelper console)
        {
            Console = console;
            Fixture = new Fixture();
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var customization = new SupportMutableValueTypesCustomization();
            customization.Customize(Fixture);
        }

        [Fact]
        public void FromPropertyToClass()
        {
            var source = Fixture.Create<TC1C0_I0_Members>();
            var map = Mapper<TC1C0_I0_Members, TC0_I0_Members>.CompileFunc(o => o
              .Ignore(i => i)
              .Map(s => s.N0, d => d, false, false));
            var destination = map(source);

            Assert.True(CompareEquals(source.N0, destination));
        }

        [Fact]
        public void FromClassToProperty()
        {
            var source = Fixture.Create<TC0_I0_Members>();
            var map = Mapper<TC0_I0_Members, TC1C0_I0_Members>.CompileFunc(o => o
                 .Ignore(i => i)
                 .Map(s => s, d => d.N0, false, false));
            var destination = map(source);

            Assert.True(CompareEquals(source, destination.N0));
        }

        [Fact]
        public void IgnoreAll()
        {
            var source = Fixture.Create<TC0_I0_Members>();
            var map = Mapper<TC0_I0_Members, TC0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i));
            var destination = map(source);

            Assert.True(CompareEquals(new TC1C0_I0_Members(), destination));
        }

        [Fact]
        public void IgnoreNode()
        {
            var source = Fixture.Create<TC1C0_I0_Members>();
            var map = Mapper<TC1C0_I0_Members, TC1C0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i.N0));
            var destination = map(source);
            Assert.Null(destination);
        }

        [Fact]
        public void TypeToTypeKeyDictionary()
        {
            var source = Fixture.Create<Dictionary<Type, TC0_I0_Members>>();
            
            var map = Mapper<Dictionary<Type, TC0_I0_Members>, Dictionary<Type, TC0_I0_Members>>.CompileFunc();
            var destination = map(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void RecursiveNodesDictionary()
        {
            var source = Fixture.Create<Dictionary<string, TNode>>();
            var map = Mapper<Dictionary<string, TNode>, Dictionary<string, TNode>>.CompileFunc();
            var destination = map(source);

            Assert.True(CompareEquals(source, destination));
        }

    }
}