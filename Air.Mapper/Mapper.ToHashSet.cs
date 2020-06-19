using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        private static HashSet<D> FromArrayToHashSet(S[] source)
        {
            HashSet<D> destination = new HashSet<D>(source.Length);

            for (int i = 0; i < source.Length; i++)
                destination.Add(CompiledFunc(source[i]));

            return destination;
        }

        public static HashSet<D> ToHashSet(S[] source)
        {
            if (source == null)
                return null;

            HashSet<D> destination = new HashSet<D>(source.Length);

            for (int i = 0; i < source.Length; i++)
                destination.Add(CompiledFunc(source[i]));

            return destination;
        }

        public static HashSet<D> ToHashSet(HashSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToHashSet(sourceArray);
        }

        public static HashSet<D> ToHashSet(LinkedList<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToHashSet(sourceArray);
        }

        public static HashSet<D> ToHashSet(List<S> source)
        {
            if (source == null)
                return null;

            HashSet<D> destination = new HashSet<D>(source.Count);
            for (int i = 0; i < source.Count; i++)
                destination.Add(CompiledFunc(source[i]));

            return destination;
        }

        public static HashSet<D> ToHashSet(Queue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToHashSet(sourceArray);
        }

        public static HashSet<D> ToHashSet(SortedSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToHashSet(sourceArray);
        }

        public static HashSet<D> ToHashSet(Stack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToHashSet(sourceArray);
        }

        public static HashSet<D> ToHashSet(ConcurrentBag<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToHashSet(sourceArray);
        }

        public static HashSet<D> ToHashSet(ConcurrentQueue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToHashSet(sourceArray);
        }

        public static HashSet<D> ToHashSet(ConcurrentStack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToHashSet(sourceArray);
        }

        public static HashSet<D> ToHashSet(IEnumerable<S> source)
        {
            try
            { return FromArrayToHashSet(source.ToArray()); }
            catch
            { return null; }
        }
    }
}
