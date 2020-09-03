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
        private readonly Dictionary<int, TC0_I0_Members> _sourceDictionary;

        public CollectionsToArray()
        {
            _fixture = new Fixture();

            _sourceList = _fixture.Create<List<TC0_I0_Members>>();
            _sourceDictionary = new Dictionary<int, TC0_I0_Members>();
            for (int i = 0; i < 32; i++)
            {
                _sourceList.AddRange(_fixture.Create<List<TC0_I0_Members>>());
                _sourceDictionary.Add(i, _fixture.Create<TC0_I0_Members>());
            }

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
        //public TC0_I0_Members[] HashSetCopyToArray()
        //    => CollectionToArray.HashSetCopyToArray(_sourceHashSet);

        //[Benchmark]
        //public TC0_I0_Members[] HashSetToArray()
        //    => CollectionToArray.HashSetToArray(_sourceHashSet);

        //[Benchmark]
        //public int HashSet_LoopForEach()
        //   => CollectionToArray.HashSet_LoopForEach(_sourceHashSet);

        //[Benchmark]
        //public int HashSet_CopyToLoopIndex()
        //   => CollectionToArray.HashSet_CopyToLoopIndex(_sourceHashSet);

        //[Benchmark]
        //public int HashSet_ToArrayLoopIndex()
        //   => CollectionToArray.HashSet_ToArrayLoopIndex(_sourceHashSet);

        //[Benchmark]
        //public KeyValuePair<int, TC0_I0_Members>[] DictionaryCopyToArray()
        //    => CollectionToArray.DictionaryCopyToArray<TC0_I0_Members>(_sourceDictionary);

        //[Benchmark]
        //public KeyValuePair<int, TC0_I0_Members>[] DictionaryToArray()
        //    => CollectionToArray.DictionaryToArray(_sourceDictionary);

        //[Benchmark]
        //public int Dictionary_LoopEnumeratorMoveNext()
        //  => CollectionToArray.Dictionary_LoopEnumeratorMoveNext(_sourceDictionary);

        //[Benchmark]
        //public int Dictionary_LoopEnumerableEnumeratorMoveNext()
        //    => CollectionToArray.Dictionary_LoopEnumerableEnumeratorMoveNext(_sourceDictionary);

        //[Benchmark]
        //public int Dictionary_LoopForEach()
        //   => CollectionToArray.Dictionary_LoopForEach(_sourceDictionary);

        //[Benchmark]
        //public int Dictionary_CopyToLoopIndex()
        //   => CollectionToArray.Dictionary_CopyToLoopIndex(_sourceDictionary);

        //[Benchmark]
        //public int Dictionary_ICollectionToArrayLoopIndex()
        //  => CollectionToArray.Dictionary_ICollectionToArrayLoopIndex(_sourceDictionary);

        //[Benchmark]
        //public int Dictionary_ToArrayLoopIndex()
        //   => CollectionToArray.Dictionary_ToArrayLoopIndex(_sourceDictionary);

        //[Benchmark]
        //public int Queue_LoopForEach()
        //  => CollectionToArray.Queue_LoopForEach(_sourceQueue);

        //[Benchmark]
        //public int Queue_CopyToLoopIndex()
        //   => CollectionToArray.Queue_CopyToLoopIndex(_sourceQueue);


        //[Benchmark]
        //public int List_IterateForLoop()
        //   => CollectionToArray.List_IterateForLoop(_sourceList);

        //[Benchmark]
        //public int List_ToArrayIterateForLoop()
        //   => CollectionToArray.List_ToArrayIterateForLoop(_sourceList);

        //[Benchmark]
        //public int IEnumerable_IterateForLoop()
        //   => CollectionToArray.IEnumerable_IterateForLoop(_sourceHashSet);


        //[Benchmark]
        //public int IEnumerable_IterateForLoop_v2()
        //   => CollectionToArray.IEnumerable_IterateForLoop_v2(_sourceHashSet);

        //[Benchmark]
        //public int IEnumerable_ToArray_IterateForLoop()
        //  => CollectionToArray.IEnumerable_ToArray_IterateForLoop(_sourceHashSet);

        //[Benchmark]
        //public int IEnumerable_ToArray_IterateForLoop_v2()
        //=> CollectionToArray.IEnumerable_ToArray_IterateForLoop_v2(_sourceHashSet);
    }
}
