using Air.Mapper;
using AutoFixture;
using Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Air.Compare.Members;

namespace Playground
{
    public class IL
    {
        private readonly ITestOutputHelper Console;

        private Fixture Fixture { get; }

        public IL(ITestOutputHelper console)
        {
            Console = console;
            Fixture = new Fixture();
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                   .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var customization = new SupportMutableValueTypesCustomization();
            customization.Customize(Fixture);
        }

        private void WriteLine(string method, Stopwatch stopwatch) =>
            Console.WriteLine("{0,-18};{1,-32}",
                stopwatch.Elapsed,
                method);

        [Fact]
        public void RunBenchmark()
        {
            var list = new Fixture().Create<List<TC0_I0_Members>>();
            for (var i = 0; i < 16; i++)
                list.AddRange(new Fixture().Create<List<TC0_I0_Members>>());

            var source = list.ToArray();
            var destination = Mapper<TC0_I0_Members, TC0_I0_Members>.ToArray(source);

            int runs = 1;
            int actions = 25;

            while (runs < 3)
            {
                Stopwatch stopwatch = new Stopwatch();

                Console.WriteLine($" =======  Run: {runs}; Actions: {actions};  =======");

                stopwatch.Start();
                for (int i = 0; i < actions; i++)
                    destination = Mapper<TC0_I0_Members, TC0_I0_Members>.ToArray(source);

                stopwatch.Stop();
                WriteLine("ToArray", stopwatch);

                stopwatch.Restart();
                for (int i = 0; i < actions; i++)
                    destination = Mapper<TC0_I0_Members, TC0_I0_Members>.ToArray(source);

                stopwatch.Stop();
                WriteLine("ToArray1", stopwatch);

                Console.WriteLine($"cicle {runs}, {destination.Length} entries map");

                runs += 1;
                actions *= 10;
            }
        }


        public void IDictionaryCollection()
        {
            var source = new Dictionary<string, string>();

            var _0 = new Dictionary<string, string>(source);
            var _1 = new SortedDictionary<string, string>(source);
            var _2 = new SortedList<string, string>(source);
            var _9 = new ConcurrentDictionary<string, string>(source);
        }

        public void IListCollection()
        {
            var source = new string[] { };

            var _1 = new HashSet<string>(source); // ICollection
            var _2 = new LinkedList<string>(source); // ICollection
            //var _3 = new LinkedListNode<List<string>>(source);
            var _4 = new List<string>(source); // ICollection
            var _5 = new Queue<string>(source); // IEnumerable
            var _6 = new SortedSet<string>(source); // ICollection
            var _7 = new Stack<string>(source); // IEnumerable
            var _8 = new ConcurrentBag<string>(source); // IProducerConsumerCollection
            var _9 = new ConcurrentQueue<string>(source); // IProducerConsumerCollection
            var _10 = new ConcurrentStack<string>(source); // IProducerConsumerCollection

        }

        [Fact]
        public void History()
        {
            var il = string.Empty;

            // expected set_N1 once
            il = Mapper<TC2C1C0_I0_Members, TC2SS1C0_I0_Members>.ViewActionRefIL();
            Assert.True(Regex.Matches(il, "set_N1").Count == 1);

            // expected locals: 2
            il = Mapper<TC1NS0_I3_Static_Members, TC1C0_I4_Static_Members>.ViewFuncIL(o => o.Ignore(i => i).Map("N0.Value.StringMember", "N0.StringMember"));
            Assert.True(Regex.Matches(il, "DeclareLocal").Count == 2);

            // expected locals: 1
            il = Mapper<TC1NS0_I3_Static_Members, TC1C0_I4_Static_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N0.Value.StringMember", "N0.StringMember"));
            Assert.True(Regex.Matches(il, "DeclareLocal").Count == 1);

            // expected locals: 3
            il = Mapper<TC1NS0_I3_Static_Members, TC1NS0_I0_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N0.Value.StringMember", "N0.Value.StringMember"));
            Assert.True(Regex.Matches(il, "DeclareLocal").Count == 3);

            // expected locals: 1
            il = Mapper<TS0_I3_Static_Members?, TS0_I0_Members?>.ViewActionRefIL(o => o.Ignore(i => i).Map("Value.StringMember", "StringMember"));
            Assert.True(Regex.Matches(il, "DeclareLocal").Count == 1);

            // expected locals: 2
            il = Mapper<TC1NS0_I0_Members, TC1C0_I0_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N0.Value.StringMember", "N0.StringMember"));
            Assert.True(Regex.Matches(il, "DeclareLocal").Count == 2);

            // expected locals: 4
            il = Mapper<TC2NS1NS0_I0_Members, TC2C1C0_I0_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N1.Value.N0.Value.StringMember", "N1.N0.StringMember"));
            Assert.True(Regex.Matches(il, "DeclareLocal").Count == 4);

            // expected locals: 1
            il = Mapper<TS2S1SC0_I0_Members, TC2C1C0_I0_Members>.ViewFuncIL(o => o.Ignore(i => i).Map("N1.N0.StringMember", "N1.N0.StringMember"));
            Assert.True(Regex.Matches(il, "DeclareLocal").Count == 1);

            // expected locals: 2
            il = Mapper<TC2NS1C0_I2_Literal_Members, TC2C1C0_I0_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N1.Value.N0.StringMember", "N1.N0.StringMember"));
            Assert.True(Regex.Matches(il, "DeclareLocal").Count == 2);

            // expected locals: 4
            il = Mapper<TC2NS1C0_I0_Members, TC2NS1C0_I0_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N1.Value.N0.StringMember", "N1.Value.N0.StringMember"));
            Assert.True(Regex.Matches(il, "DeclareLocal").Count == 4);
        }

        [Fact]
        public void MapStaticNode()
        {
            Assert.Throws<InvalidOperationException>(
                () => Mapper<TC2SC1C0_I0_Members, TC2SC1C0_I0_Members>.CompileFunc(o => o.Ignore(i => i).Map("N1", "N1", false)));
        }

        [Fact]
        public void MapNodeOfStaticNode()
        {
            Assert.Throws<InvalidOperationException>(
                () => Mapper<TC2SC1C0_I0_Members, TC2SC1C0_I0_Members>.CompileFunc(o => o.Ignore(i => i).Map("N1.N0", "N1.N0", false)));
        }

        [Fact]
        public void MapNodeMemberOfStaticNode()
        {
            Assert.Throws<InvalidOperationException>(
                 () => Mapper<TC2SC1C0_I0_Members, TC2SC1C0_I0_Members>.CompileFunc(o => o.Ignore(i => i).Map("N1.N0.StringMember", "N1.N0.StringMember", false)));
        }

        [Fact]
        public void MapFromBuiltInTypeToNode()
        {
            Assert.Throws<InvalidOperationException>(
                () => Mapper<TC2C1C0_I0_Members, TC2C1C0_I0_Members>.CompileFunc(o => o.Ignore(i => i).Map("N1.N0.StringMember", "N1.N0", false)));
        }

        [Fact]
        public void MapFromNodeToBuiltInType()
        {
            Assert.Throws<InvalidOperationException>(
                () => Mapper<TC2C1C0_I0_Members, TC2C1C0_I0_Members>.CompileFunc(o => o.Ignore(i => i).Map("N1.N0", "N1.N0.StringMember", false)));
        }

        [Fact]
        public void InProgress()
        {
            var il = string.Empty;
            il = Mapper<TS2S1S0_I0_Members, TS2SS1S0_I0_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N1.N0.StringMember", "N1.N0.StringMember"));

            il = Mapper<TC1SS0_I0_Members, TC1C0_I0_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N0.StringMember", "N0.StringMember"));
        }

       

        [Fact]
        public void Collection()
        {
            var source = Fixture.Create<TClassArrayClassMembers>();
            var mapFunc = Mapper<TClassArrayClassMembers, TStructListNullableStructMembers>.CompileFunc();
            var destination = mapFunc(source);


            var il = Mapper<TClassArrayClassMembers, TStructListNullableStructMembers>.ViewFuncIL();

            var mapActionRef = Mapper<TClassArrayClassMembers, TStructListNullableStructMembers>.CompileActionRef();
            mapActionRef(source, ref destination);
            Assert.True(CompareEquals(source, destination));

        }
    }
}
