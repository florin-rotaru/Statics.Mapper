using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

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




    #region to be removed

    public class TClassWrapper
    {
        public TC0_I0_Members Members { get; set; }
        public TC0_I1_Nullable_Members NullableMembers { get; set; }

        public TC0_I0_Members[] MembersArray { get; set; }
        public List<TC0_I0_Members> MembersList { get; set; }
        public Dictionary<Type, TC0_I0_Members> MembersTypeDictionary { get; set; }
        public Dictionary<int, TC0_I0_Members> MembersIntDictionary { get; set; }
        public Dictionary<int, TNode> RecursiveNodesDictionary { get; set; }
    }

    public struct TStructWrapper
    {
        public TClassWrapper ClassWrapper { get; set; }

        public TC0_I0_Members[] MembersArray { get; set; }
        public List<TC0_I0_Members> MembersList { get; set; }
        public Dictionary<Type, TC0_I0_Members> MembersTypeDictionary { get; set; }
        public Dictionary<int, TNode> RecursiveNodesDictionary { get; set; }
        public Dictionary<int, TC0_I0_Members> MembersIntDictionary { get; set; }
    }

    public class TMisc
    {
        public TStructWrapper? StructWrapper { get; set; }
    }

    #endregion
}
