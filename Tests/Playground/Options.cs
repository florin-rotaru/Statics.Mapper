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


        //[Fact]
        //public void EvaluatePath()
        //{
        //    var source = new TNodes();
        //    var map = Mapper<TNodes, TNodes>.CompileFunc(o => o
        //        .Ignore(i => i)
        //        .Map(s => s.Node_1.Segment.Members.GuidMember, d => d.Node_1.Segment.Members.GuidMember)
        //        .Map(s => s.Node_1.Segment.Members.Int16SMember, d => d.Node_1.Segment.Members.Int16SMember)
        //        .Map(s => s.Node_1.Segment.Members.Int32Member, d => d.Node_1.Segment.Members.Int32Member)

        //        .Map(s => s.Node_2.Segment.Members.Int64Member, d => d.Node_1.Segment.Members.Int64Member)
        //        .Map(s => s.Node_2.Segment.Members.UInt16Member, d => d.Node_1.Segment.Members.UInt16Member)
        //        .Map(s => s.Node_2.Segment.Members.UInt32Member, d => d.Node_1.Segment.Members.UInt32Member)

        //        .Map(s => s.Node_3.Segment.Members.UInt64Member, d => d.Node_1.Segment.Members.UInt64Member)
        //        .Map(s => s.Node_3.Segment.Members.DateTimeMember, d => d.Node_1.Segment.Members.DateTimeMember)
        //        .Map(s => s.Node_3.Segment.Members.DecimalMember, d => d.Node_1.Segment.Members.DecimalMember));

        //    source.Node_1 = new TNode
        //    {
        //        Segment = new TSegment
        //        {
        //            Members = new TC0_I0_Members
        //            {
        //                GuidMember = Fixture.Create<Guid>(),
        //                Int16SMember = Fixture.Create<short>(),
        //                Int32Member = Fixture.Create<int>()
        //            }
        //        }
        //    };

        //    source.Node_2 = new TNode
        //    {
        //        Segment = new TSegment
        //        {
        //            Members = new TC0_I0_Members
        //            {
        //                Int64Member = Fixture.Create<long>(),
        //                UInt16Member = Fixture.Create<ushort>(),
        //                UInt32Member = Fixture.Create<uint>()
        //            }
        //        }
        //    };

        //    source.Node_3 = new TNode
        //    {
        //        Segment = new TSegment
        //        {
        //            Members = new TC0_I0_Members
        //            {
        //                UInt64Member = Fixture.Create<ulong>(),
        //                DateTimeMember = Fixture.Create<DateTime>(),
        //                DecimalMember = Fixture.Create<decimal>()
        //            }
        //        }
        //    };

        //    var destination = map(source);

        //    Assert.Equal(source.Node_1.Segment.Members.GuidMember, destination.Node_1.Segment.Members.GuidMember);
        //    Assert.Equal(source.Node_1.Segment.Members.Int16SMember, destination.Node_1.Segment.Members.Int16SMember);
        //    Assert.Equal(source.Node_1.Segment.Members.Int32Member, destination.Node_1.Segment.Members.Int32Member);

        //    Assert.Equal(source.Node_2.Segment.Members.Int64Member, destination.Node_1.Segment.Members.Int64Member);
        //    Assert.Equal(source.Node_2.Segment.Members.UInt16Member, destination.Node_1.Segment.Members.UInt16Member);
        //    Assert.Equal(source.Node_2.Segment.Members.UInt32Member, destination.Node_1.Segment.Members.UInt32Member);

        //    Assert.Equal(source.Node_3.Segment.Members.UInt64Member, destination.Node_1.Segment.Members.UInt64Member);
        //    Assert.Equal(source.Node_3.Segment.Members.DateTimeMember, destination.Node_1.Segment.Members.DateTimeMember);
        //    Assert.Equal(source.Node_3.Segment.Members.DecimalMember, destination.Node_1.Segment.Members.DecimalMember);

        //    source.Node_3 = null;

        //    destination = map(source);

        //    Assert.Equal(source.Node_1.Segment.Members.GuidMember, destination.Node_1.Segment.Members.GuidMember);
        //    Assert.Equal(source.Node_1.Segment.Members.Int16SMember, destination.Node_1.Segment.Members.Int16SMember);
        //    Assert.Equal(source.Node_1.Segment.Members.Int32Member, destination.Node_1.Segment.Members.Int32Member);

        //    Assert.Equal(source.Node_2.Segment.Members.Int64Member, destination.Node_1.Segment.Members.Int64Member);
        //    Assert.Equal(source.Node_2.Segment.Members.UInt16Member, destination.Node_1.Segment.Members.UInt16Member);
        //    Assert.Equal(source.Node_2.Segment.Members.UInt32Member, destination.Node_1.Segment.Members.UInt32Member);

        //    Assert.Equal(default(ulong), destination.Node_1.Segment.Members.UInt64Member);
        //    Assert.Equal(default(DateTime), destination.Node_1.Segment.Members.DateTimeMember);
        //    Assert.Equal(default(decimal), destination.Node_1.Segment.Members.DecimalMember);

        //}

        //[Fact]
        //public void FromPropertyToClass()
        //{
        //    var source = Fixture.Create<TNode>();
        //    var map = Mapper<TNode, TC0_I0_Members>.CompileFunc(o => o
        //      .Ignore(i => i)
        //      .Map(s => s.Segment.Members, d => d));
        //    var destination = map(source);

        //    Assert.True(CompareEquals(destination, source.Segment.Members));
        //}

        //[Fact]
        //public void FromClassToProperty()
        //{
        //    var source = Fixture.Create<TC0_I0_Members>();
        //    var map = Mapper<TC0_I0_Members, TNode>.CompileFunc(o => o
        //         .Ignore(i => i)
        //         .Map(s => s, d => d.Segment.Members));
        //    var destination = map(source);

        //    Assert.True(CompareEquals(source, destination.Segment.Members));
        //}



        //[Fact]
        //public void IgnoreAll()
        //{
        //    var source = Fixture.Create<TNode>();
        //    var map = Mapper<TNode, TNode>.CompileFunc(o => o
        //        .Ignore(i => i));
        //    var destination = map(source);

        //    Assert.True(CompareNoneEquals(source, destination, evaluateChildNodes: true));
        //}

        //[Fact]
        //public void IgnoreNode()
        //{
        //    var source = Fixture.Create<TNode>();
        //    var map = Mapper<TNode, TNode>.CompileFunc(o => o
        //        .Ignore(i => i.Segment.NullableMembers));
        //    var destination = map(source);

        //    Assert.True(CompareEquals(source.Segment.Members, destination.Segment.Members));
        //    Assert.Null(destination.Segment.NullableMembers);

        //    map = Mapper<TNode, TNode>.CompileFunc(o => o
        //       .Ignore(i => i.Segment));
        //    destination = map(source);

        //    Assert.Equal(source.Name, destination.Name);
        //    Assert.Null(destination.Segment);
        //}




        // todo rework

        //[Fact]
        //public void ArraysFromClassToClass()
        //{
        //    var source = Fixture.Create<TArrays>();
        //    var mapFunc = Mapper<TArrays, TArrays>.CompileFunc(o => o
        //        .Ignore(i => i)
        //        .Map(s => s.NS_CArrays.Members, d => d.NS_CArrays.Members));
        //    var destination = mapFunc(source);
        //    Assert.True(CompareEquals(source.NS_CArrays.Members, destination.NS_CArrays.Members));

        //    destination = null;
        //    var mapActionRef = Mapper<TArrays, TArrays>.CompileActionRef(o => o
        //        .Ignore(i => i)
        //        .Map(s => s.NS_CArrays.Members, d => d.NS_CArrays.Members));
        //    mapActionRef(source, ref destination);
        //    Assert.True(CompareEquals(source.NS_CArrays.Members, destination.NS_CArrays.Members));
        //}

        //[Fact]
        //public void ArraysFromClassToClassViaStaticClass()
        //{
        //    var source = Fixture.Create<TArrays>();
        //    var mapFunc = Mapper<TArrays, TArrays>.CompileFunc(o => o
        //        .Ignore(i => i)
        //        .Map("NS_CArrays.Members", "S_CArrays.Members"));
        //    var destination = mapFunc(source);
        //    Assert.True(CompareEquals(source.NS_CArrays.Members, TArrays.S_CArrays.Members));

        //    destination = null;
        //    var mapActionRef = Mapper<TArrays, TArrays>.CompileActionRef(o => o
        //        .Ignore(i => i)
        //        .Map("NS_CArrays.Members", "S_CArrays.Members"));
        //    mapActionRef(source, ref destination);
        //    Assert.True(CompareEquals(source.NS_CArrays.Members, TArrays.S_CArrays.Members));
        //}

        //[Fact]
        //public void ArraysFromClassToStaticClass()
        //{
        //    var source = Fixture.Create<TArrays>();
        //    var mapFunc = Mapper<TArrays, TArrays>.CompileFunc(o => o
        //        .Ignore(i => i)
        //        .Map("CArrays.Members", "S_CArrays.Members"));
        //    TArrays.S_CArrays.Members = new TMembers[] { };
        //    var destination = mapFunc(source);
        //    Assert.True(CompareEquals(source.NS_CArrays.Members, TArrays.S_CArrays.Members));

        //    destination = null;
        //    var mapActionRef = Mapper<TArrays, TArrays>.CompileActionRef(o => o
        //        .Ignore(i => i)
        //        .Map("CArrays.Members", "S_CArrays.StaticMembers"));
        //    TArrays.S_CArrays.Members = new TMembers[] { };
        //    mapActionRef(source, ref destination);
        //    Assert.True(CompareEquals(source.NS_CArrays.Members, TArrays.S_CArrays.Members));
        //}







        [Fact]
        public void IntToInKeyDictionary()
        {
            var source = Fixture.Create<TClassWrapper>();
            var map = Mapper<TClassWrapper, TClassWrapper>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.MembersIntDictionary, d => d.MembersIntDictionary));
            var destination = map(source);

            Assert.True(CompareEquals(source.MembersIntDictionary, destination.MembersIntDictionary));
        }

        [Fact]
        public void TypeToTypeKeyDictionary()
        {
            var source = Fixture.Create<TClassWrapper>();
            var map = Mapper<TClassWrapper, TClassWrapper>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.MembersTypeDictionary, d => d.MembersTypeDictionary));
            var destination = map(source);

            Assert.True(CompareEquals(source.MembersTypeDictionary, destination.MembersTypeDictionary));
        }

        [Fact]
        public void RecursiveNodesDictionary()
        {
            var source = Fixture.Create<TClassWrapper>();
            var map = Mapper<TClassWrapper, TClassWrapper>.CompileFunc(o => o
                .Ignore(i => i)
                .Map(s => s.RecursiveNodesDictionary, d => d.RecursiveNodesDictionary));
            var destination = map(source);

            Assert.True(CompareEquals(source.RecursiveNodesDictionary, destination.RecursiveNodesDictionary));
        }

        [Fact]
        public void MixedTypes()
        {
            var source = new TMisc
            {
                StructWrapper = new TStructWrapper
                {
                    ClassWrapper = new TClassWrapper
                    {
                        NullableMembers = Fixture.Create<TC0_I1_Nullable_Members>()
                    }
                }
            };

            var il = Mapper<TMisc, TMisc>.ViewFuncIL(o =>
                   o.Ignore(i => i).Map(s => s.StructWrapper.Value.ClassWrapper.NullableMembers, d => d.StructWrapper.Value.ClassWrapper.Members));

            var map = Mapper<TMisc, TMisc>.CompileFunc(o =>
                   o.Ignore(i => i).Map(s => s.StructWrapper.Value.ClassWrapper.NullableMembers, d => d.StructWrapper.Value.ClassWrapper.Members));
            var destination = map(source);

            Assert.True(
                CompareEquals(
                    source.StructWrapper.Value.ClassWrapper.NullableMembers,
                    destination.StructWrapper.Value.ClassWrapper.Members));
        }
    }
}