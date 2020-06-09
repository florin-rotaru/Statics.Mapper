using Air.Mapper;
using AutoFixture;
using Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        private TNullableStructNode? GetNullableStructNode()
        {
            var sourceClass = Fixture.Create<TNode>();
            var nullableStructNode = new TNullableStructNode?();
            var mapClassToStruct = Mapper<TNode, TNullableStructNode?>.CompileFunc();
            nullableStructNode = mapClassToStruct(sourceClass);

            return nullableStructNode;
        }

        private TStaticNode GetStaticNode()
        {
            var sourceClass = Fixture.Create<TNode>();
            var staticNode = new TStaticNode();
            var mapClassToStruct = Mapper<TNode, TStaticNode>.CompileFunc();
            staticNode = mapClassToStruct(sourceClass);

            return staticNode;
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

            var _1 = new HashSet<string>(source);
            var _2 = new LinkedList<string>(source);
            //var _3 = new LinkedListNode<List<string>>(source);
            var _4 = new List<string>(source);
            var _5 = new Queue<string>(source);
            var _6 = new SortedSet<string>(source);
            var _7 = new Stack<string>(source);
            var _8 = new ConcurrentBag<string>(source);
            var _9 = new ConcurrentQueue<string>(source);
            var _10 = new ConcurrentStack<string>(source);

        }


        [Fact]
        public void EvaluatePath()
        {
            var source = new TNodes();
            var map = Mapper<TNodes, TNodes>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.Node_1.Segment.Members.GuidMember, d => d.Node_2.Segment.Members.GuidMember)
                .Map(s => s.Node_1.Segment.Members.Int16SMember, d => d.Node_2.Segment.Members.Int16SMember)
                .Map(s => s.Node_1.Segment.Members.Int32Member, d => d.Node_2.Segment.Members.Int32Member)

                .Map(s => s.Node_2.Segment.Members.Int64Member, d => d.Node_3.Segment.Members.Int64Member)
                .Map(s => s.Node_2.Segment.Members.UInt16Member, d => d.Node_3.Segment.Members.UInt16Member)
                .Map(s => s.Node_2.Segment.Members.UInt32Member, d => d.Node_3.Segment.Members.UInt32Member)

                .Map(s => s.Node_3.Segment.Members.UInt64Member, d => d.Node_1.Segment.Members.UInt64Member)
                .Map(s => s.Node_3.Segment.Members.DateTimeMember, d => d.Node_1.Segment.Members.DateTimeMember)
                .Map(s => s.Node_3.Segment.Members.DecimalMember, d => d.Node_1.Segment.Members.DecimalMember));

            source.Node_1 = new TNode
            {
                Segment = new TSegment
                {
                    Members = new TC0_I0_Members
                    {
                        GuidMember = Fixture.Create<Guid>(),
                        Int16SMember = Fixture.Create<short>(),
                        Int32Member = Fixture.Create<int>()
                    }
                }
            };

            source.Node_2 = new TNode
            {
                Segment = new TSegment
                {
                    Members = new TC0_I0_Members
                    {
                        Int64Member = Fixture.Create<long>(),
                        UInt16Member = Fixture.Create<ushort>(),
                        UInt32Member = Fixture.Create<uint>()
                    }
                }
            };

            source.Node_3 = new TNode
            {
                Segment = new TSegment
                {
                    Members = new TC0_I0_Members
                    {
                        UInt64Member = Fixture.Create<ulong>(),
                        DateTimeMember = Fixture.Create<DateTime>(),
                        DecimalMember = Fixture.Create<decimal>()
                    }
                }
            };

            var destination = map(source);

            Assert.Equal(source.Node_1.Segment.Members.GuidMember, destination.Node_2.Segment.Members.GuidMember);
            Assert.Equal(source.Node_1.Segment.Members.Int16SMember, destination.Node_2.Segment.Members.Int16SMember);
            Assert.Equal(source.Node_1.Segment.Members.Int32Member, destination.Node_2.Segment.Members.Int32Member);

            Assert.Equal(source.Node_2.Segment.Members.Int64Member, destination.Node_3.Segment.Members.Int64Member);
            Assert.Equal(source.Node_2.Segment.Members.UInt16Member, destination.Node_3.Segment.Members.UInt16Member);
            Assert.Equal(source.Node_2.Segment.Members.UInt32Member, destination.Node_3.Segment.Members.UInt32Member);

            Assert.Equal(source.Node_3.Segment.Members.UInt64Member, destination.Node_1.Segment.Members.UInt64Member);
            Assert.Equal(source.Node_3.Segment.Members.DateTimeMember, destination.Node_1.Segment.Members.DateTimeMember);
            Assert.Equal(source.Node_3.Segment.Members.DecimalMember, destination.Node_1.Segment.Members.DecimalMember);

            source.Node_1 = null;

            destination = map(source);

            Assert.Equal(source.Node_2.Segment.Members.GuidMember, destination.Node_3.Segment.Members.GuidMember);
            Assert.Equal(source.Node_2.Segment.Members.Int16SMember, destination.Node_3.Segment.Members.Int16SMember);
            Assert.Equal(source.Node_2.Segment.Members.Int32Member, destination.Node_3.Segment.Members.Int32Member);

            Assert.Equal(source.Node_3.Segment.Members.Int64Member, destination.Node_1.Segment.Members.Int64Member);
            Assert.Equal(source.Node_3.Segment.Members.UInt16Member, destination.Node_1.Segment.Members.UInt16Member);
            Assert.Equal(source.Node_3.Segment.Members.UInt32Member, destination.Node_1.Segment.Members.UInt32Member);

            Assert.Equal(default(ulong), destination.Node_1.Segment.Members.UInt64Member);
            Assert.Equal(default(DateTime), destination.Node_1.Segment.Members.DateTimeMember);
            Assert.Equal(default(decimal), destination.Node_1.Segment.Members.DecimalMember);

        }

        private TC2NS1C0_I2_Literal_Members NewSource()
        {
            var members = Fixture.Create<TC2C1C0_I0_Members>();
            var source = Mapper<TC2C1C0_I0_Members, TC2NS1C0_I2_Literal_Members>.Map(members);
            source = source != null ? source : new TC2NS1C0_I2_Literal_Members();

            if (source != null)
                return source;

            var nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TC2NS1C0_I2_Literal_Members));
            if (nullableUnderlyingType != null)
            {
                var undelyingInstance = Activator.CreateInstance(nullableUnderlyingType);
                source = (TC2NS1C0_I2_Literal_Members)Activator.CreateInstance(typeof(TC2NS1C0_I2_Literal_Members), new[] { undelyingInstance });
            }

            return source;
        }

        [Fact]
        public void IsRequiredProbe()
        {
            var sourceMemberPath = "Value.N1.N0.StringMember";
            var destinationMemberPath = "N1.N0.StringMember";

            var il = Mapper<TS2C1SC0_I0_Members?, TC2C1C0_I0_Members>.ViewFuncIL(o => o
                .Ignore(i => i)
                .Map(sourceMemberPath, destinationMemberPath));

            il = Mapper<TS2C1SC0_I0_Members?, TC2C1C0_I0_Members>.ViewActionRefIL(o => o
                .Ignore(i => i)
                .Map(sourceMemberPath, destinationMemberPath));

            //il = Mapper<TS2SS1SS0_I0_Members, TS2S1SC0_I0_Members>.ViewFuncIL(o => o
            //    .Ignore(i => i)
            //    .Map(sourceMemberPath, destinationMemberPath));



            var convertActionRef = Mapper<TC2NS1C0_I2_Literal_Members, TC2C1C0_I0_Members>.CompileActionRef(o => o
                .Ignore(i => i)
                .Map(sourceMemberPath, destinationMemberPath));

            var convertFunc = Mapper<TC2NS1C0_I2_Literal_Members, TC2C1C0_I0_Members>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(sourceMemberPath, destinationMemberPath));

            // =======
            var source = NewSource();
            var destination = new TC2C1C0_I0_Members();
            convertActionRef(source, ref destination);
            destination = convertFunc(source);

            // =======  
            source = null;
            convertActionRef(source, ref destination);
            destination = convertFunc(source);
        }


        [Fact]
        public void History()
        {
            var il = string.Empty;

            // expected locals: 1
            il = Mapper<TLiteralStructSegment, TSegment>.ViewActionRefIL();
            Assert.True(Regex.Matches(il, "DeclareLocal").Count == 1);

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


            var il = Mapper<TClassArrayClassMembers, TStructListNullableStructMembers>.ViewActionRefIL();

            var mapActionRef = Mapper<TClassArrayClassMembers, TStructListNullableStructMembers>.CompileActionRef();
            mapActionRef(source, ref destination);
            Assert.True(CompareEquals(source, destination));

        }


        [Fact]
        public void ViewIL()
        {
            string il = null;


            il = Mapper<TS2S1SC0_I0_Members, TC2C1C0_I0_Members>.ViewFuncIL(o => o
                .Ignore(i => i)
                .Map("N1.N0.StringMember", "N1.N0.StringMember"));

            il = Mapper<TC1SS0_I0_Members, TC1C0_I0_Members>.ViewActionRefIL(o => o
                .Ignore(i => i)
                .Map("N0.StringMember", "N0.StringMember"));


            il = Mapper<TC0_I2_Literal_Members, TC0_I0_Members>.ViewFuncIL();


            il = Mapper<TNode, TNode>.ViewActionRefIL();

            il = Mapper<TMisc, TMisc>.ViewFuncIL(o =>
                o.Ignore(i => i).Map(s => s.StructWrapper.Value.ClassWrapper.NullableMembers, d => d.StructWrapper.Value.ClassWrapper.Members));

            var sourceA = new TMisc();
            var structWrapper = new TStructWrapper();
            structWrapper.ClassWrapper = new TClassWrapper();
            var nullableMembers = Fixture.Create<TC0_I1_Nullable_Members>();
            structWrapper.ClassWrapper.NullableMembers = nullableMembers;
            sourceA.StructWrapper = structWrapper;

            var mapperA = Mapper<TMisc, TMisc>.CompileFunc(o =>
               o.Ignore(i => i).Map(s => s.StructWrapper.Value.ClassWrapper.NullableMembers, d => d.StructWrapper.Value.ClassWrapper.Members));
            var destinationA = mapperA(sourceA);
        }
    }
}
