using Statics.Mapper;
using AutoFixture;
using Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Statics.Compare.Members;

namespace Playground.Tests
{
    [Collection(nameof(Types))]
    public class Types
    {
        private readonly ITestOutputHelper Console;

        private Fixture Fixture { get; }

        public Types(ITestOutputHelper console)
        {
            Console = console;
            Fixture = new Fixture();
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var customization = new SupportMutableValueTypesCustomization();
            customization.Customize(Fixture);
        }

        TNode GetRecursiveNode(int recursion)
        {
            return new TNode
            {
                Name = Fixture.Create<string>(),
                ChildNodes = recursion > 0 ? new List<TNode>
                    {
                        GetRecursiveNode(recursion - 1),
                        GetRecursiveNode(recursion - 1),
                        GetRecursiveNode(recursion - 1)
                    } : new List<TNode>(),
                ParentNode = recursion > 0 ? new TNode
                {
                    ChildNodes = recursion > 0 ? new List<TNode>
                    {
                        GetRecursiveNode(recursion - 1),
                        GetRecursiveNode(recursion - 1),
                        GetRecursiveNode(recursion - 1)
                    } : new List<TNode>(),
                    ParentNode = recursion > 0 ? GetRecursiveNode(recursion - 1) : new TNode()
                } : new TNode()
            };
        }

        [Fact]
        public void Expando()
        {
            dynamic source = new ExpandoObject();
            source.IntMember = 7;
            source.StringMember = nameof(Expando);

            var destination = Mapper<ExpandoObject, IEnumerable<KeyValuePair<string, object>>>.Map(source);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Nodes()
        {
            var source = GetRecursiveNode(2);

            var map = Mapper<TNode, TNode>.CompileFunc();
            var destination = map(source);

            Assert.True(CompareEquals(source, destination));

            map = Mapper<TNode, TNode>.CompileFunc(o => o
                .Map(s => s.ParentNode, d => d.ParentNode));
            destination = map(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void FromNonBuiltInToBuiltInStruct()
        {
            Assert.Throws<NotSupportedException>(() => { Mapper<TC0_I0_Members, int>.CompileActionRef(); });
            Assert.Throws<NotSupportedException>(() => { Mapper<TC0_I0_Members, int>.CompileFunc(); });
        }

        [Fact]
        public void FromNonBuiltInToNullableBuiltInStruct()
        {
            Assert.Throws<NotSupportedException>(() => { Mapper<TC0_I0_Members, int?>.CompileActionRef(); });
            Assert.Throws<NotSupportedException>(() => { Mapper<TC0_I0_Members, int?>.CompileFunc(); });
        }

        [Fact]
        public void FromInterface()
        {
            var source = Fixture.Create<TInterfaceA>();
            var map = Mapper<IInterface, TInterfaceB>.CompileFunc();
            var destination = map(source);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ToInterface()
        {
            Assert.Throws<NotSupportedException>(() => { Mapper<TInterfaceA, IInterface>.CompileFunc(); });
        }

        [Fact]
        public void FromAbstract()
        {
            var source = Fixture.Create<TAbstractA>();
            var map = Mapper<TAbstract, TAbstractB>.CompileFunc();
            var destination = map(source);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ToAbstract()
        {
            Assert.Throws<NotSupportedException>(() => { Mapper<TAbstractA, TAbstract>.CompileFunc(); });
        }
    }
}
