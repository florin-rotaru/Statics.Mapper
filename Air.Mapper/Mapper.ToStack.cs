using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        private static Stack<D> FromArrayToStack(S[] source)
        {
            Stack<D> destination = new Stack<D>(source.Length);

            for (int i = 0; i < source.Length; i++)
                destination.Push(CompiledFunc(source[i]));

            return destination;
        }

        public static Stack<D> ToStack(S[] source)
        {
            if (source == null)
                return null;

            Stack<D> destination = new Stack<D>(source.Length);

            for (int i = 0; i < source.Length; i++)
                destination.Push(CompiledFunc(source[i]));

            return destination;
        }

        public static Stack<D> ToStack(HashSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToStack(sourceArray);
        }

        public static Stack<D> ToStack(LinkedList<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToStack(sourceArray);
        }

        public static Stack<D> ToStack(List<S> source)
        {
            if (source == null)
                return null;

            Stack<D> destination = new Stack<D>(source.Count);
            for (int i = 0; i < source.Count; i++)
                destination.Push(CompiledFunc(source[i]));

            return destination;
        }

        public static Stack<D> ToStack(Queue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToStack(sourceArray);
        }

        public static Stack<D> ToStack(SortedSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToStack(sourceArray);
        }

        public static Stack<D> ToStack(Stack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToStack(sourceArray);
        }

        public static Stack<D> ToStack(ConcurrentBag<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToStack(sourceArray);
        }

        public static Stack<D> ToStack(ConcurrentQueue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToStack(sourceArray);
        }

        public static Stack<D> ToStack(ConcurrentStack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToStack(sourceArray);
        }

        public static Stack<D> ToStack(IEnumerable<S> source)
        {
            try
            { return FromArrayToStack(source.ToArray()); }
            catch
            { return null; }
        }
    }
}
