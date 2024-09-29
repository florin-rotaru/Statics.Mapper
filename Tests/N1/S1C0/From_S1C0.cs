using Models;
using Models.Internal;
using Xunit;
using Xunit.Abstractions;

namespace S1C0.Tests
{
    public class From_NS_S1C0_I0_Members : FromTo_N1_NonStatic_Members<TS1C0_I0_Members> { public From_NS_S1C0_I0_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1C0")]
	public class From_S_S1C0_I0_Members : FromTo_N1_Static_Members<TS1C0_I0_Members>{ public From_S_S1C0_I0_Members(ITestOutputHelper console) : base(console) {} }

	public class From_NS_S1C0_I1_Nullable_Members : FromTo_N1_NonStatic_Members<TS1C0_I1_Nullable_Members> { public From_NS_S1C0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1C0")]
	public class From_S_S1C0_I1_Nullable_Members : FromTo_N1_Static_Members<TS1C0_I1_Nullable_Members>{ public From_S_S1C0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	public class From_NS_S1C0_I2_Literal_Members : FromTo_N1_NonStatic_Members<TS1C0_I2_Literal_Members> { public From_NS_S1C0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1C0")]
	public class From_S_S1C0_I2_Literal_Members : FromTo_N1_Static_Members<TS1C0_I2_Literal_Members>{ public From_S_S1C0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	public class From_NS_S1C0_I3_Readonly_Members : FromTo_N1_NonStatic_Members<TS1C0_I3_Readonly_Members> { public From_NS_S1C0_I3_Readonly_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1C0")]
	public class From_S_S1C0_I3_Readonly_Members : FromTo_N1_Static_Members<TS1C0_I3_Readonly_Members>{ public From_S_S1C0_I3_Readonly_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1C0")]
	public class From_S1C0_I4_Static_Members : FromTo_N1_Members<TS1C0_I4_Static_Members> { public From_S1C0_I4_Static_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1C0")]
	public class From_S1C0_I5_StaticNullable_Members : FromTo_N1_Members<TS1C0_I5_StaticNullable_Members> { public From_S1C0_I5_StaticNullable_Members(ITestOutputHelper console) : base(console) {} }

}
