using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        public static IEnumerable<D> ToIEnumerable(S[] source)
        {
            if (source == null)
                return null;

            return FromToArray(source);
        }

        public static IEnumerable<D> ToIEnumerable(HashSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromToArray(sourceArray);
        }

        public static IEnumerable<D> ToIEnumerable(LinkedList<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromToArray(sourceArray);
        }

        public static IEnumerable<D> ToIEnumerable(List<S> source)
        {
            if (source == null)
                return null;

            D[] destination = new D[source.Count];

            for (int i = 0; i < source.Count; i++)
                destination[i] = CompiledFunc(source[i]);

            return destination;
        }

        public static IEnumerable<D> ToIEnumerable(Queue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromToArray(sourceArray);
        }

        public static IEnumerable<D> ToIEnumerable(SortedSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromToArray(sourceArray);
        }

        public static IEnumerable<D> ToIEnumerable(Stack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromToArray(sourceArray);
        }

        public static IEnumerable<D> ToIEnumerable(ConcurrentBag<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromToArray(sourceArray);
        }

        public static IEnumerable<D> ToIEnumerable(ConcurrentQueue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromToArray(sourceArray);
        }

        public static IEnumerable<D> ToIEnumerable(ConcurrentStack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromToArray(sourceArray);
        }

        public static IEnumerable<D> ToIEnumerable(IEnumerable<S> source)
        {
            try
            { return FromToArray(source.ToArray()); }
            catch
            { return null; }
        }
    }
}
