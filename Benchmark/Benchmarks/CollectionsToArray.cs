using AutoFixture;
using BenchmarkDotNet.Attributes;
using Models;
using System.Collections.Generic;

namespace Benchmark
{
    [InProcess]
    public class CollectionsToArray
    {
        private readonly Fixture _fixture;
        private readonly List<TC0_I0_Members> _sourceList;
        private readonly Queue<TC0_I0_Members> _sourceQueue;
        private readonly HashSet<TC0_I0_Members> _sourceHashSet;

        public CollectionsToArray()
        {
            _fixture = new Fixture();

            _sourceList = _fixture.Create<List<TC0_I0_Members>>();
            for (int i = 0; i < 32; i++)
                _sourceList.AddRange(_fixture.Create<List<TC0_I0_Members>>());
            
            _sourceQueue = new Queue<TC0_I0_Members>(_sourceList);
            _sourceHashSet = new HashSet<TC0_I0_Members>(_sourceList);
        }

        //[Benchmark]
        //public TC0_I0_Members[] List_ToArray_Using_EnumerableToArray()
        //    => CollectionToArray.EnumerableToArray(_sourceList);

        //[Benchmark]
        //public TC0_I0_Members[] List_ToArray_Using_ListToArray()
        //    => CollectionToArray.ListToArray(_sourceList);

        //[Benchmark]
        //public TC0_I0_Members[] Queue_ToArray_Using_EnumerableToArray()
        //  => CollectionToArray.EnumerableToArray(_sourceQueue);

        //[Benchmark]
        //public TC0_I0_Members[] Queue_ToArray_Using_QueueToArray()
        //    => CollectionToArray.QueueToArray(_sourceQueue);

        //[Benchmark]
        //public TC0_I0_Members[] HashSet_ToArray_Using_EnumerableToArray()
        //  => CollectionToArray.EnumerableToArray(_sourceHashSet);

        //[Benchmark]
        //public TC0_I0_Members[] HashSet_ToArray_Using_HashSetToArray()
        //    => CollectionToArray.HashSetToArray(_sourceHashSet);


        //[Benchmark]
        //public int HashSet_IterateForEach()
        //   => CollectionToArray.HashSet_IterateForEach(_sourceHashSet);

        //[Benchmark]
        //public int HashSet_ToArrayIterateForLoop()
        //   => CollectionToArray.HashSet_ToArrayIterateForLoop(_sourceHashSet);

        //[Benchmark]
        //public int List_IterateForLoop()
        //   => CollectionToArray.List_IterateForLoop(_sourceList);

        //[Benchmark]
        //public int List_ToArrayIterateForLoop()
        //   => CollectionToArray.List_ToArrayIterateForLoop(_sourceList);

        [Benchmark]
        public int IEnumerable_IterateForLoop()
           => CollectionToArray.IEnumerable_IterateForLoop(_sourceHashSet);


        [Benchmark]
        public int IEnumerable_IterateForLoop_v2()
           => CollectionToArray.IEnumerable_IterateForLoop_v2(_sourceHashSet);

        [Benchmark]
        public int IEnumerable_ToArray_IterateForLoop()
          => CollectionToArray.IEnumerable_ToArray_IterateForLoop(_sourceHashSet);

        [Benchmark]
        public int IEnumerable_ToArray_IterateForLoop_v2()
        => CollectionToArray.IEnumerable_ToArray_IterateForLoop_v2(_sourceHashSet);
    }
}
