using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        private static Queue<D> FromArrayToQueue(S[] source)
        {
            Queue<D> destination = new Queue<D>(source.Length);

            for (int i = 0; i < source.Length; i++)
                destination.Enqueue(CompiledFunc(source[i]));

            return destination;
        }

        public static Queue<D> ToQueue(S[] source)
        {
            if (source == null)
                return null;

            Queue<D> destination = new Queue<D>(source.Length);

            for (int i = 0; i < source.Length; i++)
                destination.Enqueue(CompiledFunc(source[i]));

            return destination;
        }

        public static Queue<D> ToQueue(HashSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToQueue(sourceArray);
        }

        public static Queue<D> ToQueue(LinkedList<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToQueue(sourceArray);
        }

        public static Queue<D> ToQueue(List<S> source)
        {
            if (source == null)
                return null;

            Queue<D> destination = new Queue<D>(source.Count);
            for (int i = 0; i < source.Count; i++)
                destination.Enqueue(CompiledFunc(source[i]));

            return destination;
        }

        public static Queue<D> ToQueue(Queue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToQueue(sourceArray);
        }

        public static Queue<D> ToQueue(SortedSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToQueue(sourceArray);
        }

        public static Queue<D> ToQueue(Stack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToQueue(sourceArray);
        }

        public static Queue<D> ToQueue(ConcurrentBag<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToQueue(sourceArray);
        }

        public static Queue<D> ToQueue(ConcurrentQueue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToQueue(sourceArray);
        }

        public static Queue<D> ToQueue(ConcurrentStack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToQueue(sourceArray);
        }

        public static Queue<D> ToQueue(IEnumerable<S> source)
        {
            try
            { return FromArrayToQueue(source.ToArray()); }
            catch
            { return null; }
        }
    }
}
