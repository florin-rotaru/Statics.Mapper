using Models;
using Models.Internal;
using Xunit;
using Xunit.Abstractions;

namespace NS2NS1SS0.Tests
{
    [Collection("S_S2NS1SS0")]
	public class From_NS_NS2NS1SS0_I0_Members : FromTo_N2_NonStatic_Members<TS2NS1SS0_I0_Members?> { public From_NS_NS2NS1SS0_I0_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S2NS1SS0")]
	public class From_S_NS2NS1SS0_I0_Members : FromTo_N2_Static_Members<TS2NS1SS0_I0_Members?>{ public From_S_NS2NS1SS0_I0_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S2NS1SS0")]
	public class From_NS_NS2NS1SS0_I1_Nullable_Members : FromTo_N2_NonStatic_Members<TS2NS1SS0_I1_Nullable_Members?> { public From_NS_NS2NS1SS0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S2NS1SS0")]
	public class From_S_NS2NS1SS0_I1_Nullable_Members : FromTo_N2_Static_Members<TS2NS1SS0_I1_Nullable_Members?>{ public From_S_NS2NS1SS0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S2NS1SS0")]
	public class From_NS_NS2NS1SS0_I2_Literal_Members : FromTo_N2_NonStatic_Members<TS2NS1SS0_I2_Literal_Members?> { public From_NS_NS2NS1SS0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S2NS1SS0")]
	public class From_S_NS2NS1SS0_I2_Literal_Members : FromTo_N2_Static_Members<TS2NS1SS0_I2_Literal_Members?>{ public From_S_NS2NS1SS0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S2NS1SS0")]
	public class From_NS2NS1SS0_I3_Static_Members : FromTo_N2_Members<TS2NS1SS0_I3_Static_Members?> { public From_NS2NS1SS0_I3_Static_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S2NS1SS0")]
	public class From_NS2NS1SS0_I4_StaticNullable_Members : FromTo_N2_Members<TS2NS1SS0_I4_StaticNullable_Members?> { public From_NS2NS1SS0_I4_StaticNullable_Members(ITestOutputHelper console) : base(console) {} }

}
