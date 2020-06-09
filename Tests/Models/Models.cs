using System;
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
    public class TRecursiveNode
    {
        public string Name { get; set; }

        public TRecursiveNode ParentNode { get; set; }
        public List<TRecursiveNode> ChildNodes { get; set; }
    }
    #endregion







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



    #region Nodes
    public class TMembersNode
    {
        public TC0_I0_Members Members { get; set; }
    }

    public class TStaticMembersNode
    {
        public static TC0_I4_Static_Members Members { get; set; }
    }

    public struct TStructMembersNode
    {
        public TS0_I0_Members Members { get; set; }
    }

    public struct TStaticStructMembersNode
    {
        public static TS0_I3_Static_Members Members { get; set; }
    }

    public struct TNullableStructMembersNode
    {
        public TS0_I0_Members? Members { get; set; }
    }

    public struct TStaticNullableStructMembersNode
    {
        public static TS0_I3_Static_Members? Members { get; set; }
    }

    public class TMembersNodes
    {
        public TMembersNode MembersNode { get; set; }
        public TStructMembersNode StructMembersNode { get; set; }
        public TNullableStructMembersNode? NullableStructMembersNode { get; set; }
    }

    public class TStaticMembersNodes
    {
        public static TStaticMembersNode MembersNode { get; set; }
        public static TStaticStructMembersNode StructMembersNode { get; set; }
        public static TStaticNullableStructMembersNode? NullableStructMembersNode { get; set; }
    }

    public class TSegment
    {
        public TC0_I0_Members Members { get; set; }
        public TC0_I1_Nullable_Members NullableMembers { get; set; }
    }

    public class TStaticSegment
    {
        public TC0_I4_Static_Members Members { get; set; }
        public TC0_I5_StaticNullable_Members NullableMembers { get; set; }
    }

    public class TLiteralSegment
    {
        public TC0_I2_Literal_Members Members { get; set; }
    }

    public class TNode
    {
        public string Name { get; set; }
        public TSegment Segment { get; set; }
    }

    public class TStaticNode
    {
        public static string Name { get; set; }
        public TStaticSegment Segment { get; set; }
    }

    public class TLiteralNode
    {
        public const string Name = nameof(Name);
        public TLiteralSegment Segment { get; set; }
    }

    public class TNodes
    {
        public TNode Node_1 { get; set; }
        public TNode Node_2 { get; set; }
        public TNode Node_3 { get; set; }
    }
    #endregion

    #region StructNodes
    public struct TStructSegment
    {
        public TS0_I0_Members Members { get; set; }
        public TS0_I1_Nullable_Members NullableMembers { get; set; }
    }

    public struct TStaticStructSegment
    {
        public TS0_I3_Static_Members Members { get; set; }
        public TS0_I4_StaticNullable_Members NullableMembers { get; set; }
    }

    public struct TLiteralStructSegment
    {
        public TS0_I2_Literal_Members Members { get; set; }
    }

    public struct TNullableStructSegment
    {
        public TS0_I0_Members? Members { get; set; }
        public TS0_I1_Nullable_Members? NullableMembers { get; set; }
    }

    public struct TNullableStaticStructSegment
    {
        public TS0_I3_Static_Members? Members { get; set; }
        public TS0_I4_StaticNullable_Members? NullableMembers { get; set; }
    }

    public struct TStructNode
    {
        public string Name { get; set; }
        public TStructSegment Segment { get; set; }
    }

    public struct TStaticStructNode
    {
        public static string Name { get; set; }
        public TStaticStructSegment Segment { get; set; }
    }

    public struct TStaticNullableStructNode
    {
        public static string Name { get; set; }
        public TNullableStaticStructSegment? Segment { get; set; }
    }

    public struct TLiteralStructNode
    {
        public const string Name = nameof(Name);
        public TLiteralStructSegment Segment { get; set; }
    }

    public struct TNullableStructNode
    {
        public string Name { get; set; }
        public TNullableStructSegment? Segment { get; set; }
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
        public Dictionary<int, TRecursiveNode> RecursiveNodesDictionary { get; set; }
    }

    public struct TStructWrapper
    {
        public TClassWrapper ClassWrapper { get; set; }

        public TC0_I0_Members[] MembersArray { get; set; }
        public List<TC0_I0_Members> MembersList { get; set; }
        public Dictionary<Type, TC0_I0_Members> MembersTypeDictionary { get; set; }
        public Dictionary<int, TRecursiveNode> RecursiveNodesDictionary { get; set; }
        public Dictionary<int, TC0_I0_Members> MembersIntDictionary { get; set; }
    }

    public class TMisc
    {
        public TStructWrapper? StructWrapper { get; set; }
    }

    #endregion
}
