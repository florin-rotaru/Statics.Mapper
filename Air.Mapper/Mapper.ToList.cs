using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        private static List<D> FromArrayToList(S[] source)
        {
            List<D> destination = new List<D>(source.Length);

            for (int i = 0; i < source.Length; i++)
                destination.Add(CompiledFunc(source[i]));

            return destination;
        }

        public static List<D> ToList(S[] source)
        {
            if (source == null)
                return null;

            List<D> destination = new List<D>(source.Length);

            for (int i = 0; i < source.Length; i++)
                destination.Add(CompiledFunc(source[i]));

            return destination;
        }

        public static List<D> ToList(HashSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToList(sourceArray);
        }

        public static List<D> ToList(LinkedList<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToList(sourceArray);
        }

        public static List<D> ToList(List<S> source)
        {
            if (source == null)
                return null;

            List<D> destination = new List<D>(source.Count);
            for (int i = 0; i < source.Count; i++)
                destination.Add(CompiledFunc(source[i]));

            return destination;
        }

        public static List<D> ToList(Queue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToList(sourceArray);
        }

        public static List<D> ToList(SortedSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToList(sourceArray);
        }

        public static List<D> ToList(Stack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToList(sourceArray);
        }

        public static List<D> ToList(ConcurrentBag<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToList(sourceArray);
        }

        public static List<D> ToList(ConcurrentQueue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToList(sourceArray);
        }

        public static List<D> ToList(ConcurrentStack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToList(sourceArray);
        }

        public static List<D> ToList(IEnumerable<S> source)
        {
            try
            { return FromArrayToList(source.ToArray()); }
            catch
            { return null; }
        }
    }
}
