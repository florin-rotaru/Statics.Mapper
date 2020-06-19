using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        private static ConcurrentQueue<D> FromArrayToConcurrentQueue(S[] source)
        {
            ConcurrentQueue<D> destination = new ConcurrentQueue<D>();

            for (int i = 0; i < source.Length; i++)
                destination.Enqueue(CompiledFunc(source[i]));

            return destination;
        }

        public static ConcurrentQueue<D> ToConcurrentQueue(S[] source)
        {
            if (source == null)
                return null;

            ConcurrentQueue<D> destination = new ConcurrentQueue<D>();

            for (int i = 0; i < source.Length; i++)
                destination.Enqueue(CompiledFunc(source[i]));

            return destination;
        }

        public static ConcurrentQueue<D> ToConcurrentQueue(HashSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToConcurrentQueue(sourceArray);
        }

        public static ConcurrentQueue<D> ToConcurrentQueue(LinkedList<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentQueue(sourceArray);
        }

        public static ConcurrentQueue<D> ToConcurrentQueue(List<S> source)
        {
            if (source == null)
                return null;

            ConcurrentQueue<D> destination = new ConcurrentQueue<D>();
            for (int i = 0; i < source.Count; i++)
                destination.Enqueue(CompiledFunc(source[i]));

            return destination;
        }

        public static ConcurrentQueue<D> ToConcurrentQueue(Queue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentQueue(sourceArray);
        }

        public static ConcurrentQueue<D> ToConcurrentQueue(SortedSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToConcurrentQueue(sourceArray);
        }

        public static ConcurrentQueue<D> ToConcurrentQueue(Stack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentQueue(sourceArray);
        }

        public static ConcurrentQueue<D> ToConcurrentQueue(ConcurrentBag<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentQueue(sourceArray);
        }

        public static ConcurrentQueue<D> ToConcurrentQueue(ConcurrentQueue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentQueue(sourceArray);
        }

        public static ConcurrentQueue<D> ToConcurrentQueue(ConcurrentStack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentQueue(sourceArray);
        }

        public static ConcurrentQueue<D> ToConcurrentQueue(IEnumerable<S> source)
        {
            try
            { return FromArrayToConcurrentQueue(source.ToArray()); }
            catch
            { return null; }
        }
    }
}
