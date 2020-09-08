using Air.Mapper;
using AutoFixture;
using BenchmarkDotNet.Attributes;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace Benchmarks
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

        public static TC0_I0_Members[] ListToArray(List<TC0_I0_Members> list)
           => list.ToArray();

        public static TC0_I0_Members[] EnumerableToArray(IEnumerable<TC0_I0_Members> enumerable)
            => enumerable.ToArray();

        [Benchmark]
        public TC0_I0_Members[] List_ToArray_Using_EnumerableToArray()
            => EnumerableToArray(_sourceList);

        [Benchmark]
        public TC0_I0_Members[] List_ToArray_Using_ListToArray()
            => ListToArray(_sourceList);

        [Benchmark]
        public TC0_I0_Members[] Queue_ToArray_Using_EnumerableToArray()
          => EnumerableToArray(_sourceQueue);

        [Benchmark]
        public TC0_I0_Members[] Queue_ToArray_Using_QueueToArray()
        {
            if (_sourceQueue == null)
                return null;

            TC0_I0_Members[] entries = new TC0_I0_Members[_sourceQueue.Count];
            _sourceQueue.CopyTo(entries, 0);
            return entries;
        }

        [Benchmark]
        public TC0_I0_Members[] HashSetCopyToArray()
        {
            if (_sourceHashSet == null)
                return null;

            TC0_I0_Members[] entries = new TC0_I0_Members[_sourceHashSet.Count];
            _sourceHashSet.CopyTo(entries, 0);
            return entries;
        }

        [Benchmark]
        public TC0_I0_Members[] HashSetToArray()
        {
            if (_sourceHashSet == null)
                return null;

            return _sourceHashSet.ToArray();
        }

        [Benchmark]
        public int HashSet_LoopForEach()
        {
            int returnValue = 0;

            foreach (var item in _sourceHashSet)
                if (item != null)
                    returnValue++;

            return returnValue;
        }

        [Benchmark]
        public int HashSet_CopyToLoopIndex()
        {
            int returnValue = 0;
            var array = Air.Reflection.Emit.ILGenerator.Converters.ToArray(_sourceHashSet);

            for (int i = 0; i < array.Length; i++)
                if (array[i] != null)
                    returnValue++;

            return returnValue;
        }

        [Benchmark]
        public int HashSet_ToArrayLoopIndex()
        {
            int returnValue = 0;
            var array = _sourceHashSet.ToArray();

            for (int i = 0; i < array.Length; i++)
                if (array[i] != null)
                    returnValue++;

            return returnValue;
        }

        [Benchmark]
        public KeyValuePair<int, TC0_I0_Members>[] DictionaryCopyToArray()
        {
            if (_sourceDictionary == null)
                return null;

            KeyValuePair<int, TC0_I0_Members>[] entries = new KeyValuePair<int, TC0_I0_Members>[_sourceDictionary.Count];
            ((ICollection<KeyValuePair<int, TC0_I0_Members>>)_sourceDictionary).CopyTo(entries, 0);
            return entries;
        }

        [Benchmark]
        public KeyValuePair<int, TC0_I0_Members>[] DictionaryToArray()
        {
            if (_sourceDictionary == null)
                return null;

            return _sourceDictionary.ToArray();
        }

        [Benchmark]
        public int Dictionary_LoopEnumeratorMoveNext()
        {
            int returnValue = 0;
            var enumerator = _sourceDictionary.GetEnumerator();

            while (enumerator.MoveNext())
                if (enumerator.Current.Key > 0)
                    returnValue++;

            return returnValue;
        }

        [Benchmark]
        public int Dictionary_LoopEnumerableEnumeratorMoveNext()
        {
            int returnValue = 0;
            var enumerator = ((ICollection<KeyValuePair<int, TC0_I0_Members>>)_sourceDictionary).GetEnumerator();

            while (enumerator.MoveNext())
                if (enumerator.Current.Key > 0)
                    returnValue++;

            return returnValue;
        }

        [Benchmark]
        public int Dictionary_LoopForEach()
        {
            int returnValue = 0;

            foreach (var item in _sourceDictionary)
                if (item.Key > 0)
                    returnValue++;

            return returnValue;
        }

        [Benchmark]
        public int Dictionary_CopyToLoopIndex()
        {
            int returnValue = 0;
            KeyValuePair<int, TC0_I0_Members>[] array = Air.Reflection.Emit.ILGenerator.Converters.ToArray(_sourceDictionary);

            for (int i = 0; i < array.Length; i++)
                if (array[i].Key > 0)
                    returnValue++;

            return returnValue;
        }

        [Benchmark]
        public int Dictionary_ICollectionToArrayLoopIndex()
        {
            int returnValue = 0;
            KeyValuePair<int, TC0_I0_Members>[] array = Air.Reflection.Emit.ILGenerator.Converters.ToArray(_sourceDictionary);

            for (int i = 0; i < array.Length; i++)
                if (array[i].Key > 0)
                    returnValue++;

            return returnValue;
        }

        [Benchmark]
        public int Dictionary_ToArrayLoopIndex()
        {
            int returnValue = 0;
            KeyValuePair<int, TC0_I0_Members>[] array = _sourceDictionary.ToArray();

            for (int i = 0; i < array.Length; i++)
                if (array[i].Key > 0)
                    returnValue++;

            return returnValue;
        }

        [Benchmark]
        public int Queue_LoopForEach()
        {
            int returnValue = 0;

            foreach (var item in _sourceQueue)
                if (item != null)
                    returnValue++;

            return returnValue;
        }

        [Benchmark]
        public int Queue_CopyToLoopIndex()
        {
            int returnValue = 0;
            TC0_I0_Members[] array = _sourceQueue.ToArray();

            for (int i = 0; i < array.Length; i++)
                if (array[i] != null)
                    returnValue++;

            return returnValue;
        }

        [Benchmark]
        public int List_IterateForLoop()
        {
            int returnValue = 0;

            for (int i = 0; i < _sourceList.Count; i++)
                if (_sourceList[i] != null)
                    returnValue++;

            return returnValue;
        }

        [Benchmark]
        public int List_ToArrayIterateForLoop()
        {
            int returnValue = 0;
            TC0_I0_Members[] array = _sourceList.ToArray();

            for (int i = 0; i < array.Length; i++)
                if (array[i] != null)
                    returnValue++;

            return returnValue;
        }

        [Benchmark]
        public int IEnumerable_IterateForLoop()
        {
            int returnValue = 0;

            foreach (TC0_I0_Members item in _sourceHashSet)
            {
                if (item != null)
                    returnValue++;
            }

            return returnValue;
        }

        [Benchmark]
        public int IEnumerable_IterateForLoop_v2()
        {
            int returnValue = 0;
            IEnumerator<TC0_I0_Members> enumerator = _sourceHashSet.GetEnumerator();

            while (enumerator.MoveNext())
            {
                TC0_I0_Members item = enumerator.Current;
                if (item != null)
                    returnValue++;
            }

            return returnValue;
        }

        [Benchmark]
        public int IEnumerable_ToArray_IterateForLoop()
        {
            var _source = _sourceHashSet.ToList();

            List<TC0_I0_Members> destination;
            if (_source != null)
            {
                int i = 0;
                int len = _source.Count;
                destination = new List<TC0_I0_Members>(len);

                while (i < len)
                {
                    destination.Add(Mapper<TC0_I0_Members, TC0_I0_Members>.Map(_source[i]));
                    i++;
                }
            }
            else
            {
                destination = null;
            }

            return destination.Count;
        }

        [Benchmark]
        public int IEnumerable_ToArray_IterateForLoop_v2()
        {
            var _source = _sourceHashSet.ToList();

            List<TC0_I0_Members> destination;
            if (_source != null)
            {
                int i = 0;
                int len = _source.Count;
                destination = new List<TC0_I0_Members>(len);

                var mapper = Mapper<TC0_I0_Members, TC0_I0_Members>.CompiledFunc;

                while (i < len)
                {
                    destination.Add(mapper(_source[i]));
                    i++;
                }
            }
            else
            {
                destination = null;
            }

            return destination.Count;
        }
    }
}
