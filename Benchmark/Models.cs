using System;

namespace Benchmark
{
    public class Models
    {
        public class Nodes
        {
            public Node Node_1 { get; set; }
            public Node Node_2 { get; set; }
            public Node Node_3 { get; set; }
        }

        public struct StructNodes
        {
            public StructNode Node_1 { get; set; }
            public StructNode Node_2 { get; set; }
            public StructNode Node_3 { get; set; }
        }

        public enum EnumMember
        {
            Undefined,
            A,
            B,
            C
        }

        public enum DtoEnumMember
        {
            A,
            B,
            C
        }

        public class Node
        {
            public string Name { get; set; }
            public Segment Segment { get; set; }
        }

        public class StaticNode
        {
            public static string Name { get; set; }
            public static Segment Segment { get; set; }
        }

        public struct StructNode
        {
            public string Name { get; set; }
            public StructSegment Segment { get; set; }
        }

        public class Segment
        {
            public SystemTypeCodes SystemTypeCodes { get; set; }
            public NullableSystemTypeCodes NullableSystemTypeCodes { get; set; }
        }

        public class SystemTypeCodes
        {
            public bool BooleanMember { get; set; }
            public char CharMember { get; set; }
            public sbyte SByteMember { get; set; }
            public byte ByteMember { get; set; }
            public short Int16SMember { get; set; }
            public ushort UInt16Member { get; set; }
            public int Int32Member { get; set; }
            public uint UInt32Member { get; set; }
            public long Int64Member { get; set; }
            public ulong UInt64Member { get; set; }
            public float SingleMember { get; set; }
            public double DoubleMember { get; set; }
            public decimal DecimalMember { get; set; }
            public string StringMember { get; set; }

            public DateTime DateTimeMember { get; set; }
            public DateTimeOffset DateTimeOffsetMember { get; set; }
            public TimeSpan TimeSpanMember { get; set; }

            public Guid GuidMember { get; set; }

            public EnumMember EnumMember { get; set; }
            public DtoEnumMember DtoEnumMember { get; set; }
        }

        public class ReadonlySystemTypeCodes
        {
            public bool BooleanMember { get; }
            public char CharMember { get; }
            public sbyte SByteMember { get; }
            public byte ByteMember { get; }
            public short Int16SMember { get; }
            public ushort UInt16Member { get; }
            public int Int32Member { get; }
            public uint UInt32Member { get; }
            public long Int64Member { get; }
            public ulong UInt64Member { get; }
            public float SingleMember { get; }
            public double DoubleMember { get; }
            public decimal DecimalMember { get; }
            public string StringMember { get; }

            public DateTime DateTimeMember { get; }
            public DateTimeOffset DateTimeOffsetMember { get; }
            public TimeSpan TimeSpanMember { get; }

            public Guid GuidMember { get; }

            public EnumMember EnumMember { get; }
            public DtoEnumMember DtoEnumMember { get; }
        }

        public class NullableSystemTypeCodes
        {
            public bool? BooleanMember { get; set; }
            public char? CharMember { get; set; }
            public sbyte? SByteMember { get; set; }
            public byte? ByteMember { get; set; }
            public short? Int16SMember { get; set; }
            public ushort? UInt16Member { get; set; }
            public int? Int32Member { get; set; }
            public uint? UInt32Member { get; set; }
            public long? Int64Member { get; set; }
            public ulong? UInt64Member { get; set; }
            public float? SingleMember { get; set; }
            public double? DoubleMember { get; set; }
            public decimal? DecimalMember { get; set; }
            public string StringMember { get; set; }

            public DateTime? DateTimeMember { get; set; }
            public DateTimeOffset? DateTimeOffsetMember { get; set; }
            public TimeSpan? TimeSpanMember { get; set; }

            public Guid? GuidMember { get; set; }

            public EnumMember? EnumMember { get; set; }
            public DtoEnumMember? DtoEnumMember { get; set; }
        }

        public class StaticSystemTypeCodes
        {
            public static bool BooleanMember { get; set; }
            public static char CharMember { get; set; }
            public static sbyte SByteMember { get; set; }
            public static byte ByteMember { get; set; }
            public static short Int16SMember { get; set; }
            public static ushort UInt16Member { get; set; }
            public static int Int32Member { get; set; }
            public static uint UInt32Member { get; set; }
            public static long Int64Member { get; set; }
            public static ulong UInt64Member { get; set; }
            public static float SingleMember { get; set; }
            public static double DoubleMember { get; set; }
            public static decimal DecimalMember { get; set; }
            public static string StringMember { get; set; }

            public static DateTime DateTimeMember { get; set; }
            public static DateTimeOffset DateTimeOffsetMember { get; set; }
            public static TimeSpan TimeSpanMember { get; set; }

            public static Guid GuidMember { get; set; }

            public static EnumMember EnumMember { get; set; }
            public static DtoEnumMember DtoEnumMember { get; set; }
        }

        public class StaticNullableSystemTypeCodes
        {
            public static bool? BooleanMember { get; set; }
            public static char? CharMember { get; set; }
            public static sbyte? SByteMember { get; set; }
            public static byte? ByteMember { get; set; }
            public static short? Int16SMember { get; set; }
            public static ushort? UInt16Member { get; set; }
            public static int? Int32Member { get; set; }
            public static uint? UInt32Member { get; set; }
            public static long? Int64Member { get; set; }
            public static ulong? UInt64Member { get; set; }
            public static float? SingleMember { get; set; }
            public static double? DoubleMember { get; set; }
            public static decimal? DecimalMember { get; set; }
            public static string StringMember { get; set; }

            public static DateTime? DateTimeMember { get; set; }
            public static DateTimeOffset? DateTimeOffsetMember { get; set; }
            public static TimeSpan? TimeSpanMember { get; set; }

            public static Guid? GuidMember { get; set; }

            public static EnumMember? EnumMember { get; set; }
            public static DtoEnumMember? DtoEnumMember { get; set; }
        }
        public struct StructSegment
        {
            public StructSystemTypeCodes SystemTypeCodes { get; set; }
            public StructNullableSystemTypeCodes NullableSystemTypeCodes { get; set; }
        }

        public struct StructSystemTypeCodes
        {
            public bool BooleanMember { get; set; }
            public char CharMember { get; set; }
            public sbyte SByteMember { get; set; }
            public byte ByteMember { get; set; }
            public short Int16SMember { get; set; }
            public ushort UInt16Member { get; set; }
            public int Int32Member { get; set; }
            public uint UInt32Member { get; set; }
            public long Int64Member { get; set; }
            public ulong UInt64Member { get; set; }
            public float SingleMember { get; set; }
            public double DoubleMember { get; set; }
            public decimal DecimalMember { get; set; }
            public string StringMember { get; set; }

            public DateTime DateTimeMember { get; set; }
            public DateTimeOffset DateTimeOffsetMember { get; set; }
            public TimeSpan TimeSpanMember { get; set; }

            public Guid GuidMember { get; set; }

            public EnumMember EnumMember { get; set; }
            public DtoEnumMember DtoEnumMember { get; set; }
        }

        public struct StructNullableSystemTypeCodes
        {
            public bool? BooleanMember { get; set; }
            public char? CharMember { get; set; }
            public sbyte? SByteMember { get; set; }
            public byte? ByteMember { get; set; }
            public short? Int16SMember { get; set; }
            public ushort? UInt16Member { get; set; }
            public int? Int32Member { get; set; }
            public uint? UInt32Member { get; set; }
            public long? Int64Member { get; set; }
            public ulong? UInt64Member { get; set; }
            public float? SingleMember { get; set; }
            public double? DoubleMember { get; set; }
            public decimal? DecimalMember { get; set; }
            public string StringMember { get; set; }

            public DateTime? DateTimeMember { get; set; }
            public DateTimeOffset? DateTimeOffsetMember { get; set; }
            public TimeSpan? TimeSpanMember { get; set; }

            public Guid? GuidMember { get; set; }

            public EnumMember? EnumMember { get; set; }
            public DtoEnumMember? DtoEnumMember { get; set; }
        }
    }
}
