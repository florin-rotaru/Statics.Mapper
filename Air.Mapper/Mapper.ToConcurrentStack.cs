using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        private static ConcurrentStack<D> FromArrayToConcurrentStack(S[] source)
        {
            ConcurrentStack<D> destination = new ConcurrentStack<D>();

            for (int i = 0; i < source.Length; i++)
                destination.Push(CompiledFunc(source[i]));

            return destination;
        }

        public static ConcurrentStack<D> ToConcurrentStack(S[] source)
        {
            if (source == null)
                return null;

            ConcurrentStack<D> destination = new ConcurrentStack<D>();

            for (int i = 0; i < source.Length; i++)
                destination.Push(CompiledFunc(source[i]));

            return destination;
        }

        public static ConcurrentStack<D> ToConcurrentStack(HashSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToConcurrentStack(sourceArray);
        }

        public static ConcurrentStack<D> ToConcurrentStack(LinkedList<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentStack(sourceArray);
        }

        public static ConcurrentStack<D> ToConcurrentStack(List<S> source)
        {
            if (source == null)
                return null;

            ConcurrentStack<D> destination = new ConcurrentStack<D>();
            for (int i = 0; i < source.Count; i++)
                destination.Push(CompiledFunc(source[i]));

            return destination;
        }

        public static ConcurrentStack<D> ToConcurrentStack(Queue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentStack(sourceArray);
        }

        public static ConcurrentStack<D> ToConcurrentStack(SortedSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToConcurrentStack(sourceArray);
        }

        public static ConcurrentStack<D> ToConcurrentStack(Stack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentStack(sourceArray);
        }

        public static ConcurrentStack<D> ToConcurrentStack(ConcurrentBag<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentStack(sourceArray);
        }

        public static ConcurrentStack<D> ToConcurrentStack(ConcurrentQueue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentStack(sourceArray);
        }

        public static ConcurrentStack<D> ToConcurrentStack(ConcurrentStack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToConcurrentStack(sourceArray);
        }

        public static ConcurrentStack<D> ToConcurrentStack(IEnumerable<S> source)
        {
            try
            { return FromArrayToConcurrentStack(source.ToArray()); }
            catch
            { return null; }
        }
    }
}
