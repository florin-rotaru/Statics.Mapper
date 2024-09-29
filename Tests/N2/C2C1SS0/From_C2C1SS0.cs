using Models;
using Models.Internal;
using Xunit;
using Xunit.Abstractions;

namespace C2C1SS0.Tests
{
    [Collection("S_C2C1SS0")]
	public class From_NS_C2C1SS0_I0_Members : FromTo_N2_NonStatic_Members<TC2C1SS0_I0_Members> { public From_NS_C2C1SS0_I0_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C2C1SS0")]
	public class From_S_C2C1SS0_I0_Members : FromTo_N2_Static_Members<TC2C1SS0_I0_Members>{ public From_S_C2C1SS0_I0_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C2C1SS0")]
	public class From_NS_C2C1SS0_I1_Nullable_Members : FromTo_N2_NonStatic_Members<TC2C1SS0_I1_Nullable_Members> { public From_NS_C2C1SS0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C2C1SS0")]
	public class From_S_C2C1SS0_I1_Nullable_Members : FromTo_N2_Static_Members<TC2C1SS0_I1_Nullable_Members>{ public From_S_C2C1SS0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C2C1SS0")]
	public class From_NS_C2C1SS0_I2_Literal_Members : FromTo_N2_NonStatic_Members<TC2C1SS0_I2_Literal_Members> { public From_NS_C2C1SS0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C2C1SS0")]
	public class From_S_C2C1SS0_I2_Literal_Members : FromTo_N2_Static_Members<TC2C1SS0_I2_Literal_Members>{ public From_S_C2C1SS0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C2C1SS0")]
	public class From_C2C1SS0_I3_Static_Members : FromTo_N2_Members<TC2C1SS0_I3_Static_Members> { public From_C2C1SS0_I3_Static_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C2C1SS0")]
	public class From_C2C1SS0_I4_StaticNullable_Members : FromTo_N2_Members<TC2C1SS0_I4_StaticNullable_Members> { public From_C2C1SS0_I4_StaticNullable_Members(ITestOutputHelper console) : base(console) {} }

}
