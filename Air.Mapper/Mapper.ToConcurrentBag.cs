using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        private static ConcurrentBag<D> FromArrayToConcurrentBag(S[] source)
        {
            ConcurrentBag<D> destination = new ConcurrentBag<D>();

            for (int i = 0; i < source.Length; i++)
                destination.Add(CompiledFunc(source[i]));

            return destination;
        }

        public static ConcurrentBag<D> ToConcurrentBag(S[] source)
        {
            if (source == null)
                return null;

            ConcurrentBag<D> destination = new ConcurrentBag<D>();

            for (int i = 0; i < source.Length; i++)
                destination.Add(CompiledFunc(source[i]));

            return destination;
        }

        public static ConcurrentBag<D> ToConcurrentBag(HashSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToConcurrentBag(sourceArray);
        }

        public static ConcurrentBag<D> ToConcurrentBag(LinkedList<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentBag(sourceArray);
        }

        public static ConcurrentBag<D> ToConcurrentBag(List<S> source)
        {
            if (source == null)
                return null;

            ConcurrentBag<D> destination = new ConcurrentBag<D>();
            for (int i = 0; i < source.Count; i++)
                destination.Add(CompiledFunc(source[i]));

            return destination;
        }

        public static ConcurrentBag<D> ToConcurrentBag(Queue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentBag(sourceArray);
        }

        public static ConcurrentBag<D> ToConcurrentBag(SortedSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToConcurrentBag(sourceArray);
        }

        public static ConcurrentBag<D> ToConcurrentBag(Stack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentBag(sourceArray);
        }

        public static ConcurrentBag<D> ToConcurrentBag(ConcurrentBag<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentBag(sourceArray);
        }

        public static ConcurrentBag<D> ToConcurrentBag(ConcurrentQueue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentBag(sourceArray);
        }

        public static ConcurrentBag<D> ToConcurrentBag(ConcurrentStack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentBag(sourceArray);
        }

        public static ConcurrentBag<D> ToConcurrentBag(IEnumerable<S> source)
        {
            try
            { return FromArrayToConcurrentBag(source.ToArray()); }
            catch
            { return null; }
        }
    }
}
