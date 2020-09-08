using BenchmarkDotNet.Attributes;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace Benchmarks.Benchmarks
{
    [InProcess]
    [MemoryDiagnoser]
    public class IterateIEnumerable
    {
        private readonly IEnumerable<int> Enumerable;
        private readonly CustomEnumerable CustomEnumerable = new CustomEnumerable();
        private readonly Dictionary<int, int> Dictionary;
        private readonly KeyValuePair<int, int>[] DictionaryEntries;
        private readonly IEnumerable<KeyValuePair<int, int>> KeyValuePairsEnumerable;
        private readonly IList<int> IList = new List<int>();
        private readonly List<int> List = new List<int>();
        private readonly HashSet<int> HashSet = new HashSet<int>();


        public IterateIEnumerable()
        {
            var entries = 256;

            var array = new int[entries];
            Dictionary = new Dictionary<int, int>(entries);

            for (int i = 0; i < entries; i++)
            {
                array[i] = i;
                Dictionary.Add(i, i);
                IList.Add(i);
                List.Add(i);
                HashSet.Add(i);
                CustomEnumerable.Add(i);
            }

            Enumerable = array;
            DictionaryEntries = new KeyValuePair<int, int>[entries];
            ((ICollection<KeyValuePair<int, int>>)Dictionary).CopyTo(DictionaryEntries, 0);
            KeyValuePairsEnumerable = Dictionary.ToArray();
        }

        [Benchmark]
        public int DictionaryAdd()
        {
            var length = DictionaryEntries.Length;
            Dictionary<int, int> collection = new Dictionary<int, int>(length);

            for (int i = 0; i < length; i++)
                collection.Add(i, i);

            return collection.Count;
        }

        [Benchmark]
        public int ICollectionKeyValuePairAdd()
        {
            var length = DictionaryEntries.Length;
            ICollection<KeyValuePair<int, int>> collection = new Dictionary<int, int>(length);

            for (int i = 0; i < length; i++)
                collection.Add(new KeyValuePair<int, int>(i, i));

            return collection.Count;
        }

        [Benchmark]
        public int IDictionaryAdd()
        {
            var length = DictionaryEntries.Length;
            IDictionary<int, int> collection = new Dictionary<int, int>(length);

            for (int i = 0; i < length; i++)
                collection.Add(i, i);

            return collection.Count;
        }

        [Benchmark]
        public int ICollectionAdd()
        {
            var length = List.Count;
            ICollection<int> collection = new List<int>(length);

            for (int i = 0; i < length; i++)
                collection.Add(i);

            return collection.Count;
        }

        [Benchmark]
        public int IListAdd()
        {
            var length = List.Count;
            IList<int> collection = new List<int>(length);

            for (int i = 0; i < length; i++)
                collection.Add(i);

            return collection.Count;
        }


        [Benchmark]
        public int ListAdd()
        {
            var length = List.Count;
            List<int> collection = new List<int>(length);

            for (int i = 0; i < length; i++)
                collection.Add(i);

            return collection.Count;
        }

        [Benchmark]
        public int CustomEnumerableForeach()
        {
            int result = 0;
            foreach (var entry in CustomEnumerable)
                if (entry != 0)
                    result++;

            return result;
        }

        [Benchmark]
        public int CustomEnumerableToArray()
        {
            int result = 0;
            var collection = CustomEnumerable.ToArray();
            for (int i = 0; i < collection.Length; i++)
                if (collection[i] != 0)
                    result++;

            return result;
        }

        [Benchmark]
        public int IEnumerableForeachLoop()
        {
            int result = 0;
            foreach (var entry in Enumerable)
                if (entry != 0)
                    result++;

            return result;
        }

        [Benchmark]
        public int IEnumerableToArrayLoop()
        {
            int result = 0;
            var collection = Enumerable.ToArray();
            for (int i = 0; i < collection.Length; i++)
                if (collection[i] != 0)
                    result++;

            return result;
        }


        [Benchmark]
        public int HashSetForeachLoop()
        {
            int result = 0;
            foreach (var entry in HashSet)
                if (entry != 0)
                    result++;

            return result;
        }

        [Benchmark]
        public int HashSetToArrayLoop()
        {
            int result = 0;
            var collection = new int[HashSet.Count];
            HashSet.CopyTo(collection, 0);
            for (int i = 0; i < collection.Length; i++)
                if (collection[i] != 0)
                    result++;

            return result;
        }


        [Benchmark]
        public int HashSetCopyTo()
        {
            int result = 0;
            var collection = new int[HashSet.Count];
            HashSet.CopyTo(collection, 0);
            for (int i = 0; i < collection.Length; i++)
                if (collection[i] != 0)
                    result++;

            return result;
        }


        [Benchmark]
        public int HashSetICollectionCopyTo()
        {
            int result = 0;
            var collection = new int[HashSet.Count];
            ((ICollection<int>)HashSet).CopyTo(collection, 0);
            for (int i = 0; i < collection.Length; i++)
                if (collection[i] != 0)
                    result++;

            return result;
        }


        [Benchmark]
        public int LinkedListForeachLoop()
        {
            int result = 0;
            foreach (var entry in HashSet)
                if (entry != 0)
                    result++;

            return result;
        }

        [Benchmark]
        public int LinkedListToArrayLoop()
        {
            int result = 0;
            var collection = new int[HashSet.Count];
            HashSet.CopyTo(collection, 0);
            for (int i = 0; i < collection.Length; i++)
                if (collection[i] != 0)
                    result++;

            return result;
        }


        [Benchmark]
        public int IListLoop()
        {
            int result = 0;
            var collection = IList;
            for (int i = 0; i < collection.Count; i++)
                if (collection[i] != 0)
                    result++;

            return result;
        }

        [Benchmark]
        public int IListToListLoop()
        {
            int result = 0;
            var collection = IList.ToList();
            for (int i = 0; i < collection.Count; i++)
                if (collection[i] != 0)
                    result++;

            return result;
        }

        [Benchmark]
        public int ListLoop()
        {
            int result = 0;
            var collection = List;
            for (int i = 0; i < collection.Count; i++)
                if (collection[i] != 0)
                    result++;

            return result;
        }

        [Benchmark]
        public int DictionaryForeach()
        {
            int result = 0;
            foreach (var entry in Dictionary)
                if (entry.Value != 0)
                    result++;

            return result;
        }

        [Benchmark]
        public int DictionaryToArray()
        {
            int result = 0;
            var collection = new KeyValuePair<int, int>[Dictionary.Count];
            ((ICollection<KeyValuePair<int, int>>)Dictionary).CopyTo(collection, 0);
            for (int i = 0; i < collection.Length; i++)
                if (collection[i].Value != 0)
                    result++;

            return result;
        }

        [Benchmark]
        public int KeyValuePairsForeach()
        {
            int result = 0;
            foreach (var entry in KeyValuePairsEnumerable)
                if (entry.Value != 0)
                    result++;

            return result;
        }

        [Benchmark]
        public int KeyValuePairsToArray()
        {
            int result = 0;
            var collection = KeyValuePairsEnumerable.ToArray();
            for (int i = 0; i < collection.Length; i++)
                if (collection[i].Value != 0)
                    result++;

            return result;
        }

        [Benchmark]
        public int ArrayForeach()
        {
            int result = 0;
            foreach (var entry in Enumerable)
                if (entry != 0)
                    result++;

            return result;
        }


        [Benchmark]
        public int ArrayToList()
        {
            int result = 0;
            var collection = Enumerable.ToList();
            for (int i = 0; i < collection.Count; i++)
                if (collection[i] != 0)
                    result++;

            return result;
        }

        [Benchmark]
        public int ArrayToArray()
        {
            int result = 0;
            var collection = Enumerable.ToArray();
            for (int i = 0; i < collection.Length; i++)
                if (collection[i] != 0)
                    result++;

            return result;
        }
    }
}
