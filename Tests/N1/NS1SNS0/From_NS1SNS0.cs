using Models;
using Models.Internal;
using Xunit;
using Xunit.Abstractions;

namespace NS1SNS0.Tests
{
    [Collection("S_S1SNS0")]
	public class From_NS_NS1SNS0_I0_Members : FromTo_N1_NonStatic_Members<TS1SNS0_I0_Members?> { public From_NS_NS1SNS0_I0_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SNS0")]
	public class From_S_NS1SNS0_I0_Members : FromTo_N1_Static_Members<TS1SNS0_I0_Members?>{ public From_S_NS1SNS0_I0_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SNS0")]
	public class From_NS_NS1SNS0_I1_Nullable_Members : FromTo_N1_NonStatic_Members<TS1SNS0_I1_Nullable_Members?> { public From_NS_NS1SNS0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SNS0")]
	public class From_S_NS1SNS0_I1_Nullable_Members : FromTo_N1_Static_Members<TS1SNS0_I1_Nullable_Members?>{ public From_S_NS1SNS0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SNS0")]
	public class From_NS_NS1SNS0_I2_Literal_Members : FromTo_N1_NonStatic_Members<TS1SNS0_I2_Literal_Members?> { public From_NS_NS1SNS0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SNS0")]
	public class From_S_NS1SNS0_I2_Literal_Members : FromTo_N1_Static_Members<TS1SNS0_I2_Literal_Members?>{ public From_S_NS1SNS0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SNS0")]
	public class From_NS1SNS0_I3_Static_Members : FromTo_N1_Members<TS1SNS0_I3_Static_Members?> { public From_NS1SNS0_I3_Static_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S1SNS0")]
	public class From_NS1SNS0_I4_StaticNullable_Members : FromTo_N1_Members<TS1SNS0_I4_StaticNullable_Members?> { public From_NS1SNS0_I4_StaticNullable_Members(ITestOutputHelper console) : base(console) {} }

}
