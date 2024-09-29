using Statics.Mapper;
using AutoFixture;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Statics.Compare.Members;

namespace Mapper.UnitTests
{
    [Collection(nameof(IL))]
    public partial class IL
    {
        readonly ITestOutputHelper Console;

        Fixture Fixture { get; }

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

        [GeneratedRegex("DeclareLocal")]
        public static partial Regex DeclareLocalRegex();

        void WriteLine(string method, Stopwatch stopwatch) =>
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
            var destination = Mapper<TC0_I0_Members[], TC0_I0_Members[]>.Map(source);

            int runs = 1;
            int actions = 25;

            while (runs < 3)
            {
                Stopwatch stopwatch = new();

                Console.WriteLine($" =======  Run: {runs}; Actions: {actions};  =======");

                stopwatch.Start();
                for (int i = 0; i < actions; i++)
                    destination = Mapper<TC0_I0_Members[], TC0_I0_Members[]>.Map(source);

                stopwatch.Stop();
                WriteLine("ToArray", stopwatch);

                stopwatch.Restart();
                for (int i = 0; i < actions; i++)
                    destination = Mapper<TC0_I0_Members[], TC0_I0_Members[]>.Map(source);

                stopwatch.Stop();
                WriteLine("ToArray", stopwatch);

                Console.WriteLine($"cicle {runs}, {destination.Length} entries map");

                runs += 1;
                actions *= 10;
            }
        }

        [Fact]
        public void History()
        {
            var il = string.Empty;

            // expected set_N1 once
            il = Mapper<TC2C1C0_I0_Members, TC2SS1C0_I0_Members>.ViewActionRefIL();
            Assert.Equal(1, Regex.Matches(il, "set_N1").Count);

            // expected locals: 2
            il = Mapper<TC1NS0_I3_Static_Members, TC1C0_I4_Static_Members>.ViewIL(o => o.Ignore(i => i).Map("N0.Value.StringMember", "N0.StringMember"));
            Assert.Equal(2, DeclareLocalRegex().Matches(il).Count);

            // expected locals: 1
            il = Mapper<TC1NS0_I3_Static_Members, TC1C0_I4_Static_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N0.Value.StringMember", "N0.StringMember"));
            Assert.Single(DeclareLocalRegex().Matches(il));

            // expected locals: 3
            il = Mapper<TC1NS0_I3_Static_Members, TC1NS0_I0_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N0.Value.StringMember", "N0.Value.StringMember"));
            Assert.Equal(3, DeclareLocalRegex().Matches(il).Count);

            // expected locals: 1
            il = Mapper<TS0_I3_Static_Members?, TS0_I0_Members?>.ViewActionRefIL(o => o.Ignore(i => i).Map("Value.StringMember", "StringMember"));
            Assert.Single(DeclareLocalRegex().Matches(il));

            // expected locals: 2
            il = Mapper<TC1NS0_I0_Members, TC1C0_I0_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N0.Value.StringMember", "N0.StringMember"));
            Assert.Equal(2, DeclareLocalRegex().Matches(il).Count);

            // expected locals: 4
            il = Mapper<TC2NS1NS0_I0_Members, TC2C1C0_I0_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N1.Value.N0.Value.StringMember", "N1.N0.StringMember"));
            Assert.Equal(4, DeclareLocalRegex().Matches(il).Count);

            // expected locals: 1
            il = Mapper<TS2S1SC0_I0_Members, TC2C1C0_I0_Members>.ViewIL(o => o.Ignore(i => i).Map("N1.N0.StringMember", "N1.N0.StringMember"));
            Assert.Single(DeclareLocalRegex().Matches(il));

            // expected locals: 2
            il = Mapper<TC2NS1C0_I2_Literal_Members, TC2C1C0_I0_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N1.Value.N0.StringMember", "N1.N0.StringMember"));
            Assert.Equal(2, DeclareLocalRegex().Matches(il).Count);

            il = Mapper<TC2NS1C0_I2_Literal_Members, TC2C1C0_I0_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N1.Value.N0.StringMember", "N1.N0.StringMember"));
            Assert.Equal(2, DeclareLocalRegex().Matches(il).Count);

            // expected locals: 4
            il = Mapper<TC2NS1C0_I0_Members, TC2NS1C0_I0_Members>.ViewActionRefIL(o => o.Ignore(i => i).Map("N1.Value.N0.StringMember", "N1.Value.N0.StringMember"));
            Assert.Equal(4, DeclareLocalRegex().Matches(il).Count);
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
            Assert.True(CompareEquals(source, destination));

            var mapActionRef = Mapper<TClassArrayClassMembers, TStructListNullableStructMembers>.CompileActionRef();
            mapActionRef(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }
    }
}
