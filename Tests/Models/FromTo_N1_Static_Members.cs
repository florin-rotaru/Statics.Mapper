using Models;
using Xunit;
using Xunit.Abstractions;

namespace Internal
{
    public class FromTo_N1_Static_Members<S> : FromTo_N1<S> where S : new()
    {
        public FromTo_N1_Static_Members(ITestOutputHelper console) : base(console) { }

        #region To C1C0
        [Fact]
        public void To_C1C0_I4_Static_Members() => ToClass<TC1C0_I4_Static_Members>(false);
        [Fact]
        public void To_C1C0_I5_StaticNullable_Members() => ToClass<TC1C0_I5_StaticNullable_Members>(false);
        #endregion
        #region To C1S0
        [Fact]
        public void To_C1S0_I3_Static_Members() => ToClass<TC1S0_I3_Static_Members>(false);
        [Fact]
        public void To_C1S0_I4_StaticNullable_Members() => ToClass<TC1S0_I4_StaticNullable_Members>(false);
        #endregion
        #region To C1NS0
        [Fact]
        public void To_C1NS0_I3_Static_Members() => ToClass<TC1NS0_I3_Static_Members>(false);
        [Fact]
        public void To_C1NS0_I4_StaticNullable_Members() => ToClass<TC1NS0_I4_StaticNullable_Members>(false);
        #endregion
        #region To S1C0
        [Fact]
        public void To_S1C0_I4_Static_Members() => ToStruct<TS1C0_I4_Static_Members>(false);
        [Fact]
        public void To_S1C0_I5_StaticNullable_Members() => ToStruct<TS1C0_I5_StaticNullable_Members>(false);
        #endregion
        #region To NS1C0
        [Fact]
        public void To_NS1C0_I4_Static_Members() => ToNullableStruct<TS1C0_I4_Static_Members>(false);
        [Fact]
        public void To_NS1C0_I5_StaticNullable_Members() => ToNullableStruct<TS1C0_I5_StaticNullable_Members>(false);
        #endregion
        #region To S1S0
        [Fact]
        public void To_S1S0_I3_Static_Members() => ToStruct<TS1S0_I3_Static_Members>(false);
        [Fact]
        public void To_S1S0_I4_StaticNullable_Members() => ToStruct<TS1S0_I4_StaticNullable_Members>(false);
        #endregion
        #region To NS1S0
        [Fact]
        public void To_NS1S0_I3_Static_Members() => ToNullableStruct<TS1S0_I3_Static_Members>(false);
        [Fact]
        public void To_NS1S0_I4_StaticNullable_Members() => ToNullableStruct<TS1S0_I4_StaticNullable_Members>(false);
        #endregion
        #region To S1NS0
        [Fact]
        public void To_S1NS0_I3_Static_Members() => ToStruct<TS1NS0_I3_Static_Members>(false);
        [Fact]
        public void To_S1NS0_I4_StaticNullable_Members() => ToStruct<TS1NS0_I4_StaticNullable_Members>(false);
        #endregion
        #region To NS1NS0
        [Fact]
        public void To_NS1NS0_I3_Static_Members() => ToNullableStruct<TS1NS0_I3_Static_Members>(false);
        [Fact]
        public void To_NS1NS0_I4_StaticNullable_Members() => ToNullableStruct<TS1NS0_I4_StaticNullable_Members>(false);
        #endregion
        #region To C1SC0
        [Fact]
        public void To_C1SC0_I0_Members() => ToClass<TC1SC0_I0_Members>(false);
        [Fact]
        public void To_C1SC0_I1_Nullable_Members() => ToClass<TC1SC0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C1SC0_I2_Literal_Members() => ToClass<TC1SC0_I2_Literal_Members>(true);
        [Fact]
        public void To_C1SC0_I3_Readonly_Members() => ToClass<TC1SC0_I3_Readonly_Members>(true);
        [Fact]
        public void To_C1SC0_I4_Static_Members() => ToClass<TC1SC0_I4_Static_Members>(false);
        [Fact]
        public void To_C1SC0_I5_StaticNullable_Members() => ToClass<TC1SC0_I5_StaticNullable_Members>(false);
        #endregion
        #region To C1SS0
        [Fact]
        public void To_C1SS0_I0_Members() => ToClass<TC1SS0_I0_Members>(false);
        [Fact]
        public void To_C1SS0_I1_Nullable_Members() => ToClass<TC1SS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C1SS0_I2_Literal_Members() => ToClass<TC1SS0_I2_Literal_Members>(true);
        [Fact]
        public void To_C1SS0_I3_Static_Members() => ToClass<TC1SS0_I3_Static_Members>(false);
        [Fact]
        public void To_C1SS0_I4_StaticNullable_Members() => ToClass<TC1SS0_I4_StaticNullable_Members>(false);
        #endregion
        #region To C1SNS0
        [Fact]
        public void To_C1SNS0_I0_Members() => ToClass<TC1SNS0_I0_Members>(false);
        [Fact]
        public void To_C1SNS0_I1_Nullable_Members() => ToClass<TC1SNS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C1SNS0_I2_Literal_Members() => ToClass<TC1SNS0_I2_Literal_Members>(true);
        [Fact]
        public void To_C1SNS0_I3_Static_Members() => ToClass<TC1SNS0_I3_Static_Members>(false);
        [Fact]
        public void To_C1SNS0_I4_StaticNullable_Members() => ToClass<TC1SNS0_I4_StaticNullable_Members>(false);
        #endregion
        #region To S1SC0
        [Fact]
        public void To_S1SC0_I0_Members() => ToStruct<TS1SC0_I0_Members>(false);
        [Fact]
        public void To_S1SC0_I1_Nullable_Members() => ToStruct<TS1SC0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S1SC0_I2_Literal_Members() => ToStruct<TS1SC0_I2_Literal_Members>(true);
        [Fact]
        public void To_S1SC0_I3_Readonly_Members() => ToStruct<TS1SC0_I3_Readonly_Members>(true);
        [Fact]
        public void To_S1SC0_I4_Static_Members() => ToStruct<TS1SC0_I4_Static_Members>(false);
        [Fact]
        public void To_S1SC0_I5_StaticNullable_Members() => ToStruct<TS1SC0_I5_StaticNullable_Members>(false);
        #endregion
        #region To NS1SC0
        [Fact]
        public void To_NS1SC0_I0_Members() => ToNullableStruct<TS1SC0_I0_Members>(false);
        [Fact]
        public void To_NS1SC0_I1_Nullable_Members() => ToNullableStruct<TS1SC0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS1SC0_I2_Literal_Members() => ToNullableStruct<TS1SC0_I2_Literal_Members>(true);
        [Fact]
        public void To_NS1SC0_I3_Readonly_Members() => ToNullableStruct<TS1SC0_I3_Readonly_Members>(true);
        [Fact]
        public void To_NS1SC0_I4_Static_Members() => ToNullableStruct<TS1SC0_I4_Static_Members>(false);
        [Fact]
        public void To_NS1SC0_I5_StaticNullable_Members() => ToNullableStruct<TS1SC0_I5_StaticNullable_Members>(false);
        #endregion
        #region To S1SS0
        [Fact]
        public void To_S1SS0_I0_Members() => ToStruct<TS1SS0_I0_Members>(false);
        [Fact]
        public void To_S1SS0_I1_Nullable_Members() => ToStruct<TS1SS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S1SS0_I2_Literal_Members() => ToStruct<TS1SS0_I2_Literal_Members>(true);
        [Fact]
        public void To_S1SS0_I3_Static_Members() => ToStruct<TS1SS0_I3_Static_Members>(false);
        [Fact]
        public void To_S1SS0_I4_StaticNullable_Members() => ToStruct<TS1SS0_I4_StaticNullable_Members>(false);
        #endregion
        #region To NS1SS0
        [Fact]
        public void To_NS1SS0_I0_Members() => ToNullableStruct<TS1SS0_I0_Members>(false);
        [Fact]
        public void To_NS1SS0_I1_Nullable_Members() => ToNullableStruct<TS1SS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS1SS0_I2_Literal_Members() => ToNullableStruct<TS1SS0_I2_Literal_Members>(true);
        [Fact]
        public void To_NS1SS0_I3_Static_Members() => ToNullableStruct<TS1SS0_I3_Static_Members>(false);
        [Fact]
        public void To_NS1SS0_I4_StaticNullable_Members() => ToNullableStruct<TS1SS0_I4_StaticNullable_Members>(false);
        #endregion
        #region To S1SNS0
        [Fact]
        public void To_S1SNS0_I0_Members() => ToStruct<TS1SNS0_I0_Members>(false);
        [Fact]
        public void To_S1SNS0_I1_Nullable_Members() => ToStruct<TS1SNS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S1SNS0_I2_Literal_Members() => ToStruct<TS1SNS0_I2_Literal_Members>(true);
        [Fact]
        public void To_S1SNS0_I3_Static_Members() => ToStruct<TS1SNS0_I3_Static_Members>(false);
        [Fact]
        public void To_S1SNS0_I4_StaticNullable_Members() => ToStruct<TS1SNS0_I4_StaticNullable_Members>(false);
        #endregion
        #region To NS1SNS0
        [Fact]
        public void To_NS1SNS0_I0_Members() => ToNullableStruct<TS1SNS0_I0_Members>(false);
        [Fact]
        public void To_NS1SNS0_I1_Nullable_Members() => ToNullableStruct<TS1SNS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS1SNS0_I2_Literal_Members() => ToNullableStruct<TS1SNS0_I2_Literal_Members>(true);
        [Fact]
        public void To_NS1SNS0_I3_Static_Members() => ToNullableStruct<TS1SNS0_I3_Static_Members>(false);
        [Fact]
        public void To_NS1SNS0_I4_StaticNullable_Members() => ToNullableStruct<TS1SNS0_I4_StaticNullable_Members>(false);
        #endregion
    }
}
