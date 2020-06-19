using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        private static SortedSet<D> FromArrayToSortedSet(S[] source)
        {
            SortedSet<D> destination = new SortedSet<D>();

            for (int i = 0; i < source.Length; i++)
                destination.Add(CompiledFunc(source[i]));

            return destination;
        }

        public static SortedSet<D> ToSortedSet(S[] source)
        {
            if (source == null)
                return null;

            SortedSet<D> destination = new SortedSet<D>();

            for (int i = 0; i < source.Length; i++)
                destination.Add(CompiledFunc(source[i]));

            return destination;
        }

        public static SortedSet<D> ToSortedSet(HashSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToSortedSet(sourceArray);
        }

        public static SortedSet<D> ToSortedSet(LinkedList<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToSortedSet(sourceArray);
        }

        public static SortedSet<D> ToSortedSet(List<S> source)
        {
            if (source == null)
                return null;

            SortedSet<D> destination = new SortedSet<D>();
            for (int i = 0; i < source.Count; i++)
                destination.Add(CompiledFunc(source[i]));

            return destination;
        }

        public static SortedSet<D> ToSortedSet(Queue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToSortedSet(sourceArray);
        }

        public static SortedSet<D> ToSortedSet(SortedSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToSortedSet(sourceArray);
        }

        public static SortedSet<D> ToSortedSet(Stack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToSortedSet(sourceArray);
        }

        public static SortedSet<D> ToSortedSet(ConcurrentBag<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToSortedSet(sourceArray);
        }

        public static SortedSet<D> ToSortedSet(ConcurrentQueue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToSortedSet(sourceArray);
        }

        public static SortedSet<D> ToSortedSet(ConcurrentStack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToSortedSet(sourceArray);
        }

        public static SortedSet<D> ToSortedSet(IEnumerable<S> source)
        {
            try
            { return FromArrayToSortedSet(source.ToArray()); }
            catch
            { return null; }
        }
    }
}
