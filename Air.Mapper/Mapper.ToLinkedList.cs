using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        private static LinkedList<D> FromArrayToLinkedList(S[] source)
        {
            D[] destinationArray = new D[source.Length];

            for (int i = 0; i < source.Length; i++)
                destinationArray[i] = CompiledFunc(source[i]);

            return new LinkedList<D>(destinationArray);
        }

        public static LinkedList<D> ToLinkedList(S[] source)
        {
            if (source == null)
                return null;

            return FromArrayToLinkedList(source);
        }

        public static LinkedList<D> ToLinkedList(HashSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToLinkedList(sourceArray);
        }

        public static LinkedList<D> ToLinkedList(LinkedList<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToLinkedList(sourceArray);
        }

        public static LinkedList<D> ToLinkedList(List<S> source)
        {
            if (source == null)
                return null;

            D[] destination = new D[source.Count];
            for (int i = 0; i < source.Count; i++)
                destination[i] = CompiledFunc(source[i]);

            return new LinkedList<D>(destination);
        }

        public static LinkedList<D> ToLinkedList(Queue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToLinkedList(sourceArray);
        }

        public static LinkedList<D> ToLinkedList(SortedSet<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray);
            return FromArrayToLinkedList(sourceArray);
        }

        public static LinkedList<D> ToLinkedList(Stack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToLinkedList(sourceArray);
        }

        public static LinkedList<D> ToLinkedList(ConcurrentBag<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToLinkedList(sourceArray);
        }

        public static LinkedList<D> ToLinkedList(ConcurrentQueue<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToLinkedList(sourceArray);
        }

        public static LinkedList<D> ToLinkedList(ConcurrentStack<S> source)
        {
            if (source == null)
                return null;

            S[] sourceArray = new S[source.Count];
            source.CopyTo(sourceArray, 0);
            return FromArrayToLinkedList(sourceArray);
        }

        public static LinkedList<D> ToLinkedList(IEnumerable<S> source)
        {
            try
            { return FromArrayToLinkedList(source.ToArray()); }
            catch
            { return null; }
        }
    }
}
