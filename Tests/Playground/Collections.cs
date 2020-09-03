using Air.Mapper;
using AutoFixture;
using Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;
using static Air.Compare.Members;

namespace Playground
{
    [Collection(nameof(Playground))]
    public class Collections
    {
        private Fixture Fixture { get; }

        public Collections()
        {
            Fixture = new Fixture();
        }


        [Fact]
        public void ArrayRank2()
        {
            Assert.Throws<NotSupportedException>(() => Mapper<int[,], int[,]>.CompileFunc());
        }

        [Fact]
        public void SzArray()
        {
            Assert.Throws<NotSupportedException>(() => Mapper<int[][], int[][]>.CompileFunc());
        }

        [Fact]
        public void BuiltInValueTypes()
        {
            var il = Mapper<int[], int[]>.ViewFuncIL();

            var source = Fixture.Create<int[]>();
            var mapFunc = Mapper<int[], int[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<int[], int[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ValueTypes()
        {
            var source = Mapper<TC0_I0_Members[], TS0_I0_Members[]>.Map(Fixture.Create<TC0_I0_Members[]>());
            var mapFunc = Mapper<TS0_I0_Members[], TS0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TS0_I0_Members[], TS0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void TypeKeyValuePair()
        {
            var source = new List<KeyValuePair<Type, Type>>
            {
                new KeyValuePair<Type, Type>(typeof(int), typeof(string)),
                new KeyValuePair<Type, Type>(typeof(object), typeof(int)),
                new KeyValuePair<Type, Type>(typeof(object), typeof(object))
            };

            var mapFunc = Mapper<KeyValuePair<Type, Type>, KeyValuePair<Type, Type>>.CompileFunc();

            foreach (var item in source)
            {
                var destination = mapFunc(item);
                Assert.Equal(item.Key, destination.Key);
                Assert.Equal(item.Value, destination.Value);
            }
        }

        [Fact]
        public void Dictionary_To_Dictionary()
        {
            var source = Fixture.Create<Dictionary<int, TC0_I0_Members>>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<Dictionary<int, TC0_I0_Members>, Dictionary<int, TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<Dictionary<int, TC0_I0_Members>, Dictionary<int, TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void List_To_List()
        {
            var source = Fixture.Create<List<TC0_I0_Members>>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<List<TC0_I0_Members>, List<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<List<TC0_I0_Members>, List<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void List_To_IEnumerable()
        {
            var source = Fixture.Create<List<TC0_I0_Members>>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<List<TC0_I0_Members>, IEnumerable<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<List<TC0_I0_Members>, IEnumerable<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void IEnumerable_To_List()
        {
            var sourceData = Fixture.Create<List<TC0_I0_Members>>();
            IEnumerable<TC0_I0_Members> source;
            source = sourceData;
            var mapFunc = Mapper<IEnumerable<TC0_I0_Members>, List<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<IEnumerable<TC0_I0_Members>, List<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void IEnumerable_To_IEnumerable()
        {
            var sourceData = Fixture.Create<List<TC0_I0_Members>>();
            IEnumerable<TC0_I0_Members> source;
            source = sourceData;
            var mapFunc = Mapper<IEnumerable<TC0_I0_Members>, IEnumerable<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<IEnumerable<TC0_I0_Members>, IEnumerable<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void KeyValuePairList_To_KeyValuePairList()
        {
            var source = Fixture.Create<List<KeyValuePair<int, TC0_I0_Members>>>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<List<KeyValuePair<int, TC0_I0_Members>>, List<KeyValuePair<int, TC0_I0_Members>>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<List<KeyValuePair<int, TC0_I0_Members>>, List<KeyValuePair<int, TC0_I0_Members>>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void KeyValuePairArray_To_KeyValuePairList()
        {
            var source = Fixture.Create<KeyValuePair<int, TC0_I0_Members>[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<KeyValuePair<int, TC0_I0_Members>[], List<KeyValuePair<int, TC0_I0_Members>>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<KeyValuePair<int, TC0_I0_Members>[], List<KeyValuePair<int, TC0_I0_Members>>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void KeyValuePairHashset_To_KeyValuePairList()
        {
            var source = Fixture.Create<HashSet<KeyValuePair<int, TC0_I0_Members>>>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<HashSet<KeyValuePair<int, TC0_I0_Members>>, List<KeyValuePair<int, TC0_I0_Members>>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<HashSet<KeyValuePair<int, TC0_I0_Members>>, List<KeyValuePair<int, TC0_I0_Members>>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void KeyValuePairHashset_To_KeyValuePairArray()
        {
            var source = Fixture.Create<HashSet<KeyValuePair<int, TC0_I0_Members>>>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<HashSet<KeyValuePair<int, TC0_I0_Members>>, KeyValuePair<int, TC0_I0_Members>[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<HashSet<KeyValuePair<int, TC0_I0_Members>>, KeyValuePair<int, TC0_I0_Members>[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void TC_List_KeyValuePair()
        {
            var source = Fixture.Create<TC_List<KeyValuePair<int, TC0_I0_Members>>>();
            Assert.NotEmpty(source.N0);

            var mapFunc = Mapper<TC_List<KeyValuePair<int, TC0_I0_Members>>, TC_List<KeyValuePair<int, TC0_I0_Members>>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC_List<KeyValuePair<int, TC0_I0_Members>>, TC_List<KeyValuePair<int, TC0_I0_Members>>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void C1_I0_Array_To_C1_I10_TEnumerable()
        {
            var source = Fixture.Create<TC1_I0_Array_Members>();
            Assert.NotEmpty(source.N0);

            var mapFunc = Mapper<TC1_I0_Array_Members, TC1_I10_TEnumerable_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = new TC1_I10_TEnumerable_Members();
            var mapAction = Mapper<TC1_I0_Array_Members, TC1_I10_TEnumerable_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void C1_I10_TEnumerable_To_C1_I0_Array()
        {
            var source = Mapper<TC1_I0_Array_Members, TC1_I10_TEnumerable_Members>.Map(Fixture.Create<TC1_I0_Array_Members>());
            Assert.NotEmpty(source.N0);

            var mapFunc = Mapper<TC1_I10_TEnumerable_Members, TC1_I0_Array_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = new TC1_I0_Array_Members();
            var mapAction = Mapper<TC1_I10_TEnumerable_Members, TC1_I0_Array_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void C1_I11_Dictionary_To_C1_I11_Dictionary()
        {
            var source = Mapper<TC1_I11_Dictionary_Members, TC1_I11_Dictionary_Members>.Map(Fixture.Create<TC1_I11_Dictionary_Members>());
            Assert.NotEmpty(source.N0);

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
            Assert.NotEmpty(source.N0);

            var mapFunc = Mapper<TC1_I15_TDictionary_Members, TC1_I11_Dictionary_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = new TC1_I11_Dictionary_Members();
            var mapAction = Mapper<TC1_I15_TDictionary_Members, TC1_I11_Dictionary_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void C1_I11_Dictionary_To_C1_I15_TDictionary()
        {
            var source = Fixture.Create<TC1_I11_Dictionary_Members>();
            Assert.NotEmpty(source.N0);

            var mapFunc = Mapper<TC1_I11_Dictionary_Members, TC1_I15_TDictionary_Members>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = new TC1_I15_TDictionary_Members();
            var mapAction = Mapper<TC1_I11_Dictionary_Members, TC1_I15_TDictionary_Members>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }



        #region To_Array

        [Fact]
        public void IEnumerable_To_Array()
        {
            var sourceData = Fixture.Create<List<TC0_I0_Members>>();
            IEnumerable<TC0_I0_Members> source;
            source = sourceData;
            Assert.NotEmpty(source);

            var mapFunc = Mapper<IEnumerable<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<IEnumerable<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void IImmutableList_To_Array()
        {
            IImmutableList<TC0_I0_Members> source;
            source = ImmutableList.CreateRange(Fixture.Create<TC0_I0_Members[]>());
            var mapFunc = Mapper<IImmutableList<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<IImmutableList<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void IImmutableDictionary_To_Array()
        {
            IImmutableDictionary<int, TC0_I0_Members> source;
            source = ImmutableDictionary.CreateRange(Fixture.Create<KeyValuePair<int, TC0_I0_Members>[]>());
            var mapFunc = Mapper<IImmutableDictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<IImmutableDictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void IImmutableSet_To_Array()
        {
            IImmutableSet<TC0_I0_Members> source;
            source = ImmutableHashSet.CreateRange(Fixture.Create<TC0_I0_Members[]>());
            var mapFunc = Mapper<IImmutableSet<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<IImmutableSet<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void IImmutableQueue_To_Array()
        {
            IImmutableQueue<TC0_I0_Members> source;
            source = ImmutableQueue.CreateRange(Fixture.Create<TC0_I0_Members[]>());
            var mapFunc = Mapper<IImmutableQueue<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<IImmutableQueue<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void IImmutableStack_To_Array()
        {
            IImmutableStack<TC0_I0_Members> source;
            source = ImmutableStack.CreateRange(Fixture.Create<TC0_I0_Members[]>());
            var mapFunc = Mapper<IImmutableStack<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<IImmutableStack<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void IList_To_Array()
        {
            IList<TC0_I0_Members> source;
            source = Fixture.Create<List<TC0_I0_Members>>();
            var mapFunc = Mapper<IList<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<IList<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void IDictionary_To_Array()
        {
            IDictionary<int, TC0_I0_Members> source;
            source = Fixture.Create<Dictionary<int, TC0_I0_Members>>();
            var mapFunc = Mapper<IDictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<IDictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void IProducerConsumerCollection_To_Array()
        {
            IProducerConsumerCollection<TC0_I0_Members> source;
            source = new ConcurrentBag<TC0_I0_Members>(Fixture.Create<TC0_I0_Members[]>());
            var mapFunc = Mapper<IProducerConsumerCollection<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<IProducerConsumerCollection<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ICollection_To_Array()
        {
            ICollection<TC0_I0_Members> source;
            source = Fixture.Create<List<TC0_I0_Members>>();
            var mapFunc = Mapper<ICollection<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<ICollection<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void List_To_Array()
        {
            var source = Fixture.Create<List<TC0_I0_Members>>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<List<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<List<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Dictionary_To_Array()
        {
            var source = Fixture.Create<Dictionary<int, TC0_I0_Members>>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<Dictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<Dictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void HashSet_To_Array()
        {
            var source = Fixture.Create<HashSet<TC0_I0_Members>>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<HashSet<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<HashSet<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void LinkedList_To_Array()
        {
            var source = new LinkedList<TC0_I0_Members>(Fixture.Create<List<TC0_I0_Members>>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<LinkedList<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<LinkedList<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Queue_To_Array()
        {
            var source = new Queue<TC0_I0_Members>(Fixture.Create<List<TC0_I0_Members>>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<Queue<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<Queue<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void SortedDictionary_To_Array()
        {
            var source = new SortedDictionary<int, TC0_I0_Members>(Fixture.Create<Dictionary<int, TC0_I0_Members>>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<SortedDictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<SortedDictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void SortedList_To_Array()
        {
            var source = new SortedList<int, TC0_I0_Members>(Fixture.Create<Dictionary<int, TC0_I0_Members>>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<SortedList<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<SortedList<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void SortedSet_To_Array()
        {
            var source = new SortedSet<TC0_I0_Members>(Fixture.Create<List<TC0_I0_Members>>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<SortedSet<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<SortedSet<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Stack_To_Array()
        {
            var source = new Stack<TC0_I0_Members>(Fixture.Create<List<TC0_I0_Members>>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<Stack<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<Stack<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void BlockingCollection_To_Array()
        {
            var source = new BlockingCollection<TC0_I0_Members>
            {
                Fixture.Create<TC0_I0_Members>(),
                Fixture.Create<TC0_I0_Members>(),
                Fixture.Create<TC0_I0_Members>()
            };

            var mapFunc = Mapper<BlockingCollection<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<BlockingCollection<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ConcurrentBag_To_Array()
        {
            var source = new ConcurrentBag<TC0_I0_Members>
            {
                Fixture.Create<TC0_I0_Members>(),
                Fixture.Create<TC0_I0_Members>(),
                Fixture.Create<TC0_I0_Members>()
            };

            var mapFunc = Mapper<ConcurrentBag<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<ConcurrentBag<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ConcurrentDictionary_To_Array()
        {
            var source = new ConcurrentDictionary<int, TC0_I0_Members>(Fixture.Create<Dictionary<int, TC0_I0_Members>>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<ConcurrentDictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<ConcurrentDictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ConcurrentQueue_To_Array()
        {
            var source = new ConcurrentQueue<TC0_I0_Members>(Fixture.Create<List<TC0_I0_Members>>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<ConcurrentQueue<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<ConcurrentQueue<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ConcurrentStack_To_Array()
        {
            var source = new ConcurrentStack<TC0_I0_Members>(Fixture.Create<List<TC0_I0_Members>>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<ConcurrentStack<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<ConcurrentStack<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ImmutableArray_To_Array()
        {
            var source = ImmutableArray.CreateRange(Fixture.Create<TC0_I0_Members[]>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<ImmutableArray<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<ImmutableArray<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ImmutableDictionary_To_Array()
        {
            var source = ImmutableDictionary.CreateRange(Fixture.Create<KeyValuePair<int, TC0_I0_Members>[]>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<ImmutableDictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<ImmutableDictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ImmutableHashSet_To_Array()
        {
            var source = ImmutableHashSet.CreateRange(Fixture.Create<TC0_I0_Members[]>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<ImmutableHashSet<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<ImmutableHashSet<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ImmutableList_To_Array()
        {
            var source = ImmutableList.CreateRange(Fixture.Create<TC0_I0_Members[]>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<ImmutableList<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<ImmutableList<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ImmutableQueue_To_Array()
        {
            var source = ImmutableQueue.CreateRange(Fixture.Create<TC0_I0_Members[]>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<ImmutableQueue<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<ImmutableQueue<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ImmutableSortedDictionary_To_Array()
        {
            var source = ImmutableSortedDictionary.CreateRange(Fixture.Create<KeyValuePair<int, TC0_I0_Members>[]>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<ImmutableSortedDictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<ImmutableSortedDictionary<int, TC0_I0_Members>, KeyValuePair<int, TC0_I0_Members>[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ImmutableSortedSet_To_Array()
        {
            var source = ImmutableSortedSet.CreateRange(Fixture.Create<TC0_I0_Members[]>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<ImmutableSortedSet<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<ImmutableSortedSet<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void ImmutableStack_To_Array()
        {
            var source = ImmutableStack.CreateRange(Fixture.Create<TC0_I0_Members[]>());
            Assert.NotEmpty(source);

            var mapFunc = Mapper<ImmutableStack<TC0_I0_Members>, TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<ImmutableStack<TC0_I0_Members>, TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_Array()
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], TC0_I0_Members[]>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], TC0_I0_Members[]>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        #endregion

        #region Array_To

        [Fact]
        public void Array_To_IEnumerable()
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], IEnumerable<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], IEnumerable<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_IImmutableList()
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], IImmutableList<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], IImmutableList<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_IImmutableDictionary()
        {
            var source = Fixture.Create<Dictionary<int, TC0_I0_Members>>();
            Assert.NotEmpty(source);

            var sourceArray = source.OrderBy(o => o.Key).ToArray();

            var mapFunc = Mapper<Dictionary<int, TC0_I0_Members>, IImmutableDictionary<int, TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            var destinationArray = destination.OrderBy(o => o.Key).ToArray();
            Assert.True(CompareEquals(sourceArray, destinationArray));

            destination = null;
            var mapAction = Mapper<Dictionary<int, TC0_I0_Members>, IImmutableDictionary<int, TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            destinationArray = destination.OrderBy(o => o.Key).ToArray();
            Assert.True(CompareEquals(sourceArray, destinationArray));
        }

        [Fact]
        public void Array_To_IImmutableSet()
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            source = source.OrderBy(o => o.Int32Member).ToArray();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], IImmutableSet<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            var destinationArray = destination.OrderBy(o => o.Int32Member).ToArray();
            Assert.True(CompareEquals(source, destinationArray));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], IImmutableSet<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            destinationArray = destination.OrderBy(o => o.Int32Member).ToArray();
            Assert.True(CompareEquals(source, destinationArray));
        }

        [Fact]
        public void Array_To_IImmutableQueue()
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], IImmutableQueue<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], IImmutableQueue<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_IImmutableStack()
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], IImmutableStack<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            source = source.Reverse().ToArray();
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], IImmutableStack<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            source = source.Reverse().ToArray();
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_IList()
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], IList<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], IList<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_IDictionary()
        {
            var source = Fixture.Create<KeyValuePair<int, TC0_I0_Members>[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<KeyValuePair<int, TC0_I0_Members>[], IDictionary<int, TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<KeyValuePair<int, TC0_I0_Members>[], IDictionary<int, TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_IProducerConsumerCollection() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], IProducerConsumerCollection<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            source = source.Reverse().ToArray();
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], IProducerConsumerCollection<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            source = source.Reverse().ToArray();
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_ICollection() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], ICollection<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], ICollection<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_List() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], List<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], List<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_Dictionary() 
        {
            var source = Fixture.Create<KeyValuePair<int, TC0_I0_Members>[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<KeyValuePair<int, TC0_I0_Members>[], Dictionary<int, TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<KeyValuePair<int, TC0_I0_Members>[], Dictionary<int, TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_HashSet() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], HashSet<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], HashSet<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_LinkedList() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], LinkedList<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], LinkedList<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_Queue() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], Queue<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], Queue<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_SortedDictionary() 
        {
            var source = Fixture.Create<KeyValuePair<int, TC0_I0_Members>[]>();
            Assert.NotEmpty(source);

            source = source.OrderBy(o => o.Key).ToArray();

            var mapFunc = Mapper<KeyValuePair<int, TC0_I0_Members>[], SortedDictionary<int, TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<KeyValuePair<int, TC0_I0_Members>[], SortedDictionary<int, TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_SortedList() 
        {
            var source = Fixture.Create<KeyValuePair<int, TC0_I0_Members>[]>();
            Assert.NotEmpty(source);

            source = source.OrderBy(o => o.Key).ToArray();

            var mapFunc = Mapper<KeyValuePair<int, TC0_I0_Members>[], SortedList<int, TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<KeyValuePair<int, TC0_I0_Members>[], SortedList<int, TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_SortedSet() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            source = source.OrderBy(o => o.Int32Member).ToArray();

            var mapFunc = Mapper<TC0_I0_Members[], SortedSet<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], SortedSet<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_Stack() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], Stack<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            source = source.Reverse().ToArray();
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], Stack<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            source = source.Reverse().ToArray();
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_BlockingCollection() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], BlockingCollection<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], BlockingCollection<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_ConcurrentBag() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], ConcurrentBag<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            source = source.Reverse().ToArray();
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], ConcurrentBag<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            source = source.Reverse().ToArray();
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_ConcurrentDictionary() 
        {
            var source = Fixture.Create<KeyValuePair<int, TC0_I0_Members>[]>();
            Assert.NotEmpty(source);

            source = source.OrderBy(o => o.Key).ToArray();

            var mapFunc = Mapper<KeyValuePair<int, TC0_I0_Members>[], ConcurrentDictionary<int, TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            var destinationArray = destination.OrderBy(o => o.Key).ToArray();
            Assert.True(CompareEquals(source, destinationArray));

            destination = null;
            var mapAction = Mapper<KeyValuePair<int, TC0_I0_Members>[], ConcurrentDictionary<int, TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            destinationArray = destination.OrderBy(o => o.Key).ToArray();
            Assert.True(CompareEquals(source, destinationArray));
        }

        [Fact]
        public void Array_To_ConcurrentQueue() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], ConcurrentQueue<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], ConcurrentQueue<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_ConcurrentStack() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], ConcurrentStack<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            source = source.Reverse().ToArray();
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], ConcurrentStack<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            source = source.Reverse().ToArray();
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_ImmutableArray() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], ImmutableArray<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = new ImmutableArray<TC0_I0_Members>();
            var mapAction = Mapper<TC0_I0_Members[], ImmutableArray<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_ImmutableDictionary() 
        {
            var source = Fixture.Create<KeyValuePair<int, TC0_I0_Members>[]>();
            Assert.NotEmpty(source);

            source = source.OrderBy(o => o.Key).ToArray();

            var mapFunc = Mapper<KeyValuePair<int, TC0_I0_Members>[], ImmutableDictionary<int, TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            var destinationArray = destination.OrderBy(o => o.Key).ToArray();
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<KeyValuePair<int, TC0_I0_Members>[], ImmutableDictionary<int, TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            destinationArray = destination.OrderBy(o => o.Key).ToArray();
            Assert.True(CompareEquals(source, destinationArray));
        }

        [Fact]
        public void Array_To_ImmutableHashSet() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            source = source.OrderBy(o => o.Int32Member).ToArray();

            var mapFunc = Mapper<TC0_I0_Members[], ConcurrentStack<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            var destinationArray = destination.OrderBy(o => o.Int32Member).ToArray();
            Assert.True(CompareEquals(source, destinationArray));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], ConcurrentStack<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            destinationArray = destination.OrderBy(o => o.Int32Member).ToArray();
            Assert.True(CompareEquals(source, destinationArray));
        }

        [Fact]
        public void Array_To_ImmutableList() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], ImmutableList<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], ImmutableList<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_ImmutableQueue() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], ImmutableQueue<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], ImmutableQueue<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_ImmutableSortedDictionary() 
        {
            var source = Fixture.Create<KeyValuePair<int, TC0_I0_Members>[]>();
            Assert.NotEmpty(source);

            source = source.OrderBy(o => o.Key).ToArray();

            var mapFunc = Mapper<KeyValuePair<int, TC0_I0_Members>[], ImmutableSortedDictionary<int, TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<KeyValuePair<int, TC0_I0_Members>[], ImmutableSortedDictionary<int, TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_ImmutableSortedSet() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], ImmutableQueue<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], ImmutableQueue<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void Array_To_ImmutableStack() 
        {
            var source = Fixture.Create<TC0_I0_Members[]>();
            Assert.NotEmpty(source);

            var mapFunc = Mapper<TC0_I0_Members[], ImmutableStack<TC0_I0_Members>>.CompileFunc();
            var destination = mapFunc(source);
            source = source.Reverse().ToArray();
            Assert.True(CompareEquals(source, destination));

            destination = null;
            var mapAction = Mapper<TC0_I0_Members[], ImmutableStack<TC0_I0_Members>>.CompileActionRef();
            mapAction(source, ref destination);
            source = source.Reverse().ToArray();
            Assert.True(CompareEquals(source, destination));
        }

        #endregion
    }
}
