using Models;
using Models.Internal;
using Xunit;
using Xunit.Abstractions;

namespace NS1SC0.Tests
{
    [Collection("S_S1SC0")]
	public class From_NS_NS1SC0_I0_Members : FromTo_N1_NonStatic_Members<TS1SC0_I0_Members?> { public From_NS_NS1SC0_I0_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SC0")]
	public class From_S_NS1SC0_I0_Members : FromTo_N1_Static_Members<TS1SC0_I0_Members?>{ public From_S_NS1SC0_I0_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SC0")]
	public class From_NS_NS1SC0_I1_Nullable_Members : FromTo_N1_NonStatic_Members<TS1SC0_I1_Nullable_Members?> { public From_NS_NS1SC0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SC0")]
	public class From_S_NS1SC0_I1_Nullable_Members : FromTo_N1_Static_Members<TS1SC0_I1_Nullable_Members?>{ public From_S_NS1SC0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SC0")]
	public class From_NS_NS1SC0_I2_Literal_Members : FromTo_N1_NonStatic_Members<TS1SC0_I2_Literal_Members?> { public From_NS_NS1SC0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SC0")]
	public class From_S_NS1SC0_I2_Literal_Members : FromTo_N1_Static_Members<TS1SC0_I2_Literal_Members?>{ public From_S_NS1SC0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SC0")]
	public class From_NS_NS1SC0_I3_Readonly_Members : FromTo_N1_NonStatic_Members<TS1SC0_I3_Readonly_Members?> { public From_NS_NS1SC0_I3_Readonly_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SC0")]
	public class From_S_NS1SC0_I3_Readonly_Members : FromTo_N1_Static_Members<TS1SC0_I3_Readonly_Members?>{ public From_S_NS1SC0_I3_Readonly_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SC0")]
	public class From_NS1SC0_I4_Static_Members : FromTo_N1_Members<TS1SC0_I4_Static_Members?> { public From_NS1SC0_I4_Static_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SC0")]
	public class From_NS1SC0_I5_StaticNullable_Members : FromTo_N1_Members<TS1SC0_I5_StaticNullable_Members?> { public From_NS1SC0_I5_StaticNullable_Members(ITestOutputHelper console) : base(console) {} }

}
