using Xunit;
using Xunit.Abstractions;

namespace Models.Internal
{
    public class FromTo_N1_NonStatic_Members<S> : FromTo_N1<S> where S : new()
    {
        public FromTo_N1_NonStatic_Members(ITestOutputHelper console) : base(console) { }

        #region To C1C0
        [Fact]
        public void To_C1C0_I0_Members() => ToClass<TC1C0_I0_Members>(false);
        [Fact]
        public void To_C1C0_I1_Nullable_Members() => ToClass<TC1C0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C1C0_I2_Literal_Members() => ToClass<TC1C0_I2_Literal_Members>(true);
        [Fact]
        public void To_C1C0_I3_Readonly_Members() => ToClass<TC1C0_I3_Readonly_Members>(true);
        #endregion
        #region To C1S0
        [Fact]
        public void To_C1S0_I0_Members() => ToClass<TC1S0_I0_Members>(false);
        [Fact]
        public void To_C1S0_I1_Nullable_Members() => ToClass<TC1S0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C1S0_I2_Literal_Members() => ToClass<TC1S0_I2_Literal_Members>(true);
        #endregion
        #region To C1NS0
        [Fact]
        public void To_C1NS0_I0_Members() => ToClass<TC1NS0_I0_Members>(false);
        [Fact]
        public void To_C1NS0_I1_Nullable_Members() => ToClass<TC1NS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C1NS0_I2_Literal_Members() => ToClass<TC1NS0_I2_Literal_Members>(true);
        #endregion
        #region To S1C0
        [Fact]
        public void To_S1C0_I0_Members() => ToStruct<TS1C0_I0_Members>(false);
        [Fact]
        public void To_S1C0_I1_Nullable_Members() => ToStruct<TS1C0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S1C0_I2_Literal_Members() => ToStruct<TS1C0_I2_Literal_Members>(true);
        [Fact]
        public void To_S1C0_I3_Readonly_Members() => ToStruct<TS1C0_I3_Readonly_Members>(true);
        #endregion
        #region To NS1C0
        [Fact]
        public void To_NS1C0_I0_Members() => ToNullableStruct<TS1C0_I0_Members>(false);
        [Fact]
        public void To_NS1C0_I1_Nullable_Members() => ToNullableStruct<TS1C0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS1C0_I2_Literal_Members() => ToNullableStruct<TS1C0_I2_Literal_Members>(true);
        [Fact]
        public void To_NS1C0_I3_Readonly_Members() => ToNullableStruct<TS1C0_I3_Readonly_Members>(true);
        #endregion
        #region To S1S0
        [Fact]
        public void To_S1S0_I0_Members() => ToStruct<TS1S0_I0_Members>(false);
        [Fact]
        public void To_S1S0_I1_Nullable_Members() => ToStruct<TS1S0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S1S0_I2_Literal_Members() => ToStruct<TS1S0_I2_Literal_Members>(true);
        #endregion
        #region To NS1S0
        [Fact]
        public void To_NS1S0_I0_Members() => ToNullableStruct<TS1S0_I0_Members>(false);
        [Fact]
        public void To_NS1S0_I1_Nullable_Members() => ToNullableStruct<TS1S0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS1S0_I2_Literal_Members() => ToNullableStruct<TS1S0_I2_Literal_Members>(true);
        #endregion
        #region To S1NS0
        [Fact]
        public void To_S1NS0_I0_Members() => ToStruct<TS1NS0_I0_Members>(false);
        [Fact]
        public void To_S1NS0_I1_Nullable_Members() => ToStruct<TS1NS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S1NS0_I2_Literal_Members() => ToStruct<TS1NS0_I2_Literal_Members>(true);
        #endregion
        #region To NS1NS0
        [Fact]
        public void To_NS1NS0_I0_Members() => ToNullableStruct<TS1NS0_I0_Members>(false);
        [Fact]
        public void To_NS1NS0_I1_Nullable_Members() => ToNullableStruct<TS1NS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS1NS0_I2_Literal_Members() => ToNullableStruct<TS1NS0_I2_Literal_Members>(true);
        #endregion
        #region To C1SC0
        #endregion
        #region To C1SS0
        #endregion
        #region To C1SNS0
        #endregion
        #region To S1SC0
        #endregion
        #region To NS1SC0
        #endregion
        #region To S1SS0
        #endregion
        #region To NS1SS0
        #endregion
        #region To S1SNS0
        #endregion
        #region To NS1SNS0
        #endregion
    }
}
