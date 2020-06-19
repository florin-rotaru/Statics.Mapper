using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        private static D[] FromToArray(S[] source)
        {
            D[] destination = new D[source.Length];

            for (int i = 0; i < source.Length; i++)
                destination[i] = CompiledFunc(source[i]);

            return destination;
        }

        public static D[] ToArray(S[] source)
        {
            if (source == null)
                return null;

            return FromToArray(source);
        }

        public static D[] ToArray(HashSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromToArray(sourceArray);
        }

        public static D[] ToArray(LinkedList<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromToArray(sourceArray);
        }

        public static D[] ToArray(List<S> source)
        {
            if (source == null)
                return null;

            D[] destination = new D[source.Count];

            for (int i = 0; i < source.Count; i++)
                destination[i] = CompiledFunc(source[i]);

            return destination;
        }

        public static D[] ToArray(Queue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromToArray(sourceArray);
        }

        public static D[] ToArray(SortedSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromToArray(sourceArray);
        }

        public static D[] ToArray(Stack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromToArray(sourceArray);
        }

        public static D[] ToArray(ConcurrentBag<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromToArray(sourceArray);
        }

        public static D[] ToArray(ConcurrentQueue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromToArray(sourceArray);
        }

        public static D[] ToArray(ConcurrentStack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromToArray(sourceArray);
        }

        public static D[] ToArray(IEnumerable<S> source)
        {
            try
            { return FromToArray(source.ToArray()); }
            catch
            { return null; }
        }
    }
}
