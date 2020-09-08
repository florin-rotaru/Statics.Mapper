using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Models
{
    #region Enums
    public enum TUndefinedABCEnum
    {
        Undefined,
        A,
        B,
        C
    }

    public enum TABCEnum
    {
        A,
        B,
        C
    }
    #endregion

    #region Abstract Classes / Interfaces
    public interface IInterface
    {
        string Name { get; set; }
    }

    public abstract class TAbstract
    {
        public abstract string Name { get; set; }
    }

    public class TAbstractA : TAbstract
    {
        public override string Name { get; set; }
    }

    public class TAbstractB : TAbstract
    {
        public override string Name { get; set; }
    }

    public class TInterfaceA : IInterface
    {
        public string Name { get; set; }
    }

    public class TInterfaceB : IInterface
    {
        public string Name { get; set; }
    }
    #endregion

    #region Recursive
    public class TNode
    {
        public string Name { get; set; }

        public TNode ParentNode { get; set; }
        public List<TNode> ChildNodes { get; set; }
    }
    #endregion


    public class TC0_Empty { }
    public struct TS0_Empty { }

    public class TEnumerable<T> : IEnumerable<T>
    {
        private List<T> Collection { get; set; }

        public TEnumerable() { }
        public TEnumerable(IEnumerable<T> collection)
        {
            Collection = new List<T>(collection);
        }

        public IEnumerator<T> GetEnumerator() =>
            Collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            Collection.GetEnumerator();
    }

    public class TDictionary<T> : IDictionary<int, T>
    {
        private Dictionary<int, T> Collection { get; set; } = new Dictionary<int, T>();

        public TDictionary() { }

        public TDictionary(IEnumerable<KeyValuePair<int, T>> collection)
        {
            foreach (KeyValuePair<int, T> item in collection)
                Collection.Add(item.Key, item.Value);
        }

        public TDictionary(IDictionary<int, T> collection)
        {
            foreach (KeyValuePair<int, T> item in collection)
                Collection.Add(item.Key, item.Value);
        }

        public T this[int key] { get => Collection[key]; set => Collection[key] = value; }
        public ICollection<int> Keys => Collection.Keys;
        public ICollection<T> Values => Collection.Values;
        public int Count => Collection.Count;
        public bool IsReadOnly => false;
        public void Add(int key, T value) => Collection.Add(key, value);
        public void Add(KeyValuePair<int, T> item) => Collection.Add(item.Key, item.Value);
        public void Clear() => Collection.Clear();
        public bool Contains(KeyValuePair<int, T> item) => Collection.ContainsKey(item.Key);
        public bool ContainsKey(int key) => Collection.ContainsKey(key);
        public void CopyTo(KeyValuePair<int, T>[] array, int arrayIndex) => ((ICollection)Collection).CopyTo(array, arrayIndex);
        public IEnumerator<KeyValuePair<int, T>> GetEnumerator() => Collection.GetEnumerator();
        public bool Remove(int key) => Collection.Remove(key);
        public bool Remove(KeyValuePair<int, T> item) => Collection.Remove(item.Key);
        public bool TryGetValue(int key, [MaybeNullWhen(true)] out T value) => Collection.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => Collection.GetEnumerator();
    }


    public class TMC0_I0_Members
    {
        public TC0_I0_Members M0 { get; set; }
        public TC0_I0_Members M1 { get; set; }
        public TC0_I0_Members M2 { get; set; }
    }

    public class TC_List<T> { public List<T> N0 { get; set; } }

    public class TC1_I0_Array_Members { public TC0_I0_Members[] N0 { get; set; } }
    public class TC1_I1_HashSet_Members { public HashSet<TC0_I0_Members> N0 { get; set; } }
    public class TC1_I2_LinkedList_Members { public LinkedList<TC0_I0_Members> N0 { get; set; } }
    public class TC1_I3_List_Members { public List<TC0_I0_Members> N0 { get; set; } }
    public class TC1_I4_Queue_Members { public Queue<TC0_I0_Members> N0 { get; set; } }
    public class TC1_I5_SortedSet_Members { public SortedSet<TC0_I0_Members> N0 { get; set; } }
    public class TC1_I6_Stack_Members { public Stack<TC0_I0_Members> N0 { get; set; } }
    public class TC1_I7_ConcurrentBag_Members { public ConcurrentBag<TC0_I0_Members> N0 { get; set; } }
    public class TC1_I8_ConcurrentQueue_Members { public ConcurrentQueue<TC0_I0_Members> N0 { get; set; } }
    public class TC1_I9_ConcurrentStack_Members { public ConcurrentStack<TC0_I0_Members> N0 { get; set; } }
    public class TC1_I10_TEnumerable_Members { public TEnumerable<TC0_I0_Members> N0 { get; set; } }

    public class TC1_I11_Dictionary_Members { public Dictionary<int, TC0_I0_Members> N0 { get; set; } }
    public class TC1_I12_SortedDictionary_Members { public SortedDictionary<int, TC0_I0_Members> N0 { get; set; } }
    public class TC1_I13_SortedList_Members { public SortedList<int, TC0_I0_Members> N0 { get; set; } }
    public class TC1_I14_ConcurrentDictionary_Members { public ConcurrentDictionary<int, TC0_I0_Members> N0 { get; set; } }
    public class TC1_I15_TDictionary_Members { public TDictionary<TC0_I0_Members> N0 { get; set; } }


    public struct TS1_I0_Array_Members { public TC0_I0_Members[] N0 { get; set; } }
    public struct TS1_I1_HashSet_Members { public HashSet<TC0_I0_Members> N0 { get; set; } }
    public struct TS1_I2_LinkedList_Members { public LinkedList<TC0_I0_Members> N0 { get; set; } }
    public struct TS1_I3_List_Members { public List<TC0_I0_Members> N0 { get; set; } }
    public struct TS1_I4_Queue_Members { public Queue<TC0_I0_Members> N0 { get; set; } }
    public struct TS1_I5_SortedSet_Members { public SortedSet<TC0_I0_Members> N0 { get; set; } }
    public struct TS1_I6_Stack_Members { public Stack<TC0_I0_Members> N0 { get; set; } }
    public struct TS1_I7_ConcurrentBag_Members { public ConcurrentBag<TC0_I0_Members> N0 { get; set; } }
    public struct TS1_I8_ConcurrentQueue_Members { public ConcurrentQueue<TC0_I0_Members> N0 { get; set; } }
    public struct TS1_I9_ConcurrentStack_Members { public ConcurrentStack<TC0_I0_Members> N0 { get; set; } }
    public struct TS1_I10_TEnumerable_Members { public TEnumerable<TC0_I0_Members> N0 { get; set; } }

    public struct TS1_I11_Dictionary_Members { public Dictionary<int, TC0_I0_Members> N0 { get; set; } }
    public struct TS1_I12_SortedDictionary_Members { public SortedDictionary<int, TC0_I0_Members> N0 { get; set; } }
    public struct TS1_I13_SortedList_Members { public SortedList<int, TC0_I0_Members> N0 { get; set; } }
    public struct TS1_I14_ConcurrentDictionary_Members { public ConcurrentDictionary<int, TC0_I0_Members> N0 { get; set; } }
    public struct TS1_I15_TDictionary_Members { public TDictionary<TC0_I0_Members> N0 { get; set; } }

    #region Collections
    public class TClassArrayClassMembers
    {
        public TC0_I0_Members[] Members { get; set; }
    }

    public class TClassListClassMembers
    {
        public List<TC0_I0_Members> Members { get; set; }
    }

    public class TClassSortedSetClassMembers
    {
        public SortedSet<TC0_I0_Members> Members { get; set; }
    }

    public class TClassStaticArrayClassStaticMembers
    {
        public static TC0_I4_Static_Members[] Members { get; set; }
    }

    public class TClassStaticListClassStaticMembers
    {
        public static List<TC0_I4_Static_Members> Members { get; set; }
    }

    public class TClassStaticSortedSetClassStaticMembers
    {
        public static SortedSet<TC0_I4_Static_Members> Members { get; set; }
    }

    public struct TStructArrayStructMembers
    {
        public TS0_I0_Members[] Members { get; set; }
    }

    public struct TStructListStructMembers
    {
        public List<TS0_I0_Members> Members { get; set; }
    }

    public struct TStructSortedSetStructMembers
    {
        public SortedSet<TS0_I0_Members> Members { get; set; }
    }

    public struct TStructStaticArrayStructStaticMembers
    {
        public static TS0_I3_Static_Members[] Members { get; set; }
    }

    public struct TStructStaticListStructStaticMembers
    {
        public static List<TS0_I3_Static_Members> Members { get; set; }
    }

    public struct TStructStaticSortedSetStructStaticMembers
    {
        public static SortedSet<TS0_I3_Static_Members> Members { get; set; }
    }

    public struct TStructArrayNullableStructMembers
    {
        public TS0_I0_Members?[] Members { get; set; }
    }

    public struct TStructListNullableStructMembers
    {
        public List<TS0_I0_Members?> Members { get; set; }
    }

    public struct TStructSortedSetNullableStructMembers
    {
        public SortedSet<TS0_I0_Members?> Members { get; set; }
    }

    public struct TStructStaticArrayNullableStructStaticMembers
    {
        public static TS0_I3_Static_Members?[] Members { get; set; }
    }

    public struct TStructStaticListNullableStructStaticMembers
    {
        public static List<TS0_I3_Static_Members?> Members { get; set; }
    }

    public struct TStructStaticSortedSetNullableStructStaticMembers
    {
        public static SortedSet<TS0_I3_Static_Members?> Members { get; set; }
    }
    #endregion
}
