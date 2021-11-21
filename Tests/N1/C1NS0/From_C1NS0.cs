using Internal;
using Models;
using Xunit;
using Xunit.Abstractions;

namespace C1NS0.Tests
{
	public class From_NS_C1NS0_I0_Members : FromTo_N1_NonStatic_Members<TC1NS0_I0_Members> { public From_NS_C1NS0_I0_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C1NS0")]
	public class From_S_C1NS0_I0_Members : FromTo_N1_Static_Members<TC1NS0_I0_Members>{ public From_S_C1NS0_I0_Members(ITestOutputHelper console) : base(console) {} }

	public class From_NS_C1NS0_I1_Nullable_Members : FromTo_N1_NonStatic_Members<TC1NS0_I1_Nullable_Members> { public From_NS_C1NS0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C1NS0")]
	public class From_S_C1NS0_I1_Nullable_Members : FromTo_N1_Static_Members<TC1NS0_I1_Nullable_Members>{ public From_S_C1NS0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	public class From_NS_C1NS0_I2_Literal_Members : FromTo_N1_NonStatic_Members<TC1NS0_I2_Literal_Members> { public From_NS_C1NS0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C1NS0")]
	public class From_S_C1NS0_I2_Literal_Members : FromTo_N1_Static_Members<TC1NS0_I2_Literal_Members>{ public From_S_C1NS0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C1NS0")]
	public class From_C1NS0_I3_Static_Members : FromTo_N1_Members<TC1NS0_I3_Static_Members> { public From_C1NS0_I3_Static_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C1NS0")]
	public class From_C1NS0_I4_StaticNullable_Members : FromTo_N1_Members<TC1NS0_I4_StaticNullable_Members> { public From_C1NS0_I4_StaticNullable_Members(ITestOutputHelper console) : base(console) {} }

}
