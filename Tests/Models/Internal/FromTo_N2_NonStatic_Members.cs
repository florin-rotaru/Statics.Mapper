using Xunit;
using Xunit.Abstractions;

namespace Models.Internal
{
    public class FromTo_N2_NonStatic_Members<S> : FromTo_N2<S> where S : new()
    {
        public FromTo_N2_NonStatic_Members(ITestOutputHelper console) : base(console) { }

        #region To C2C1C0
        [Fact]
        public void To_C2C1C0_I0_Members() => ToClass<TC2C1C0_I0_Members>(false);
        [Fact]
        public void To_C2C1C0_I1_Nullable_Members() => ToClass<TC2C1C0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C2C1C0_I2_Literal_Members() => ToClass<TC2C1C0_I2_Literal_Members>(true);
        [Fact]
        public void To_C2C1C0_I3_Readonly_Members() => ToClass<TC2C1C0_I3_Readonly_Members>(true);
        #endregion
        #region To C2C1S0
        [Fact]
        public void To_C2C1S0_I0_Members() => ToClass<TC2C1S0_I0_Members>(false);
        [Fact]
        public void To_C2C1S0_I1_Nullable_Members() => ToClass<TC2C1S0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C2C1S0_I2_Literal_Members() => ToClass<TC2C1S0_I2_Literal_Members>(true);
        #endregion
        #region To C2C1NS0
        [Fact]
        public void To_C2C1NS0_I0_Members() => ToClass<TC2C1NS0_I0_Members>(false);
        [Fact]
        public void To_C2C1NS0_I1_Nullable_Members() => ToClass<TC2C1NS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C2C1NS0_I2_Literal_Members() => ToClass<TC2C1NS0_I2_Literal_Members>(true);
        #endregion
        #region To C2C1SC0
        #endregion
        #region To C2C1SS0
        #endregion
        #region To C2C1SNS0
        #endregion
        #region To C2S1C0
        [Fact]
        public void To_C2S1C0_I0_Members() => ToClass<TC2S1C0_I0_Members>(false);
        [Fact]
        public void To_C2S1C0_I1_Nullable_Members() => ToClass<TC2S1C0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C2S1C0_I2_Literal_Members() => ToClass<TC2S1C0_I2_Literal_Members>(true);
        [Fact]
        public void To_C2S1C0_I3_Readonly_Members() => ToClass<TC2S1C0_I3_Readonly_Members>(true);
        #endregion
        #region To C2S1S0
        [Fact]
        public void To_C2S1S0_I0_Members() => ToClass<TC2S1S0_I0_Members>(false);
        [Fact]
        public void To_C2S1S0_I1_Nullable_Members() => ToClass<TC2S1S0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C2S1S0_I2_Literal_Members() => ToClass<TC2S1S0_I2_Literal_Members>(true);
        #endregion
        #region To C2S1NS0
        [Fact]
        public void To_C2S1NS0_I0_Members() => ToClass<TC2S1NS0_I0_Members>(false);
        [Fact]
        public void To_C2S1NS0_I1_Nullable_Members() => ToClass<TC2S1NS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C2S1NS0_I2_Literal_Members() => ToClass<TC2S1NS0_I2_Literal_Members>(true);
        #endregion
        #region To C2S1SC0
        #endregion
        #region To C2S1SS0
        #endregion
        #region To C2S1SNS0
        #endregion
        #region To C2NS1C0
        [Fact]
        public void To_C2NS1C0_I0_Members() => ToClass<TC2NS1C0_I0_Members>(false);
        [Fact]
        public void To_C2NS1C0_I1_Nullable_Members() => ToClass<TC2NS1C0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C2NS1C0_I2_Literal_Members() => ToClass<TC2NS1C0_I2_Literal_Members>(true);
        [Fact]
        public void To_C2NS1C0_I3_Readonly_Members() => ToClass<TC2NS1C0_I3_Readonly_Members>(true);
        #endregion
        #region To C2NS1S0
        [Fact]
        public void To_C2NS1S0_I0_Members() => ToClass<TC2NS1S0_I0_Members>(false);
        [Fact]
        public void To_C2NS1S0_I1_Nullable_Members() => ToClass<TC2NS1S0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C2NS1S0_I2_Literal_Members() => ToClass<TC2NS1S0_I2_Literal_Members>(true);
        #endregion
        #region To C2NS1NS0
        [Fact]
        public void To_C2NS1NS0_I0_Members() => ToClass<TC2NS1NS0_I0_Members>(false);
        [Fact]
        public void To_C2NS1NS0_I1_Nullable_Members() => ToClass<TC2NS1NS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_C2NS1NS0_I2_Literal_Members() => ToClass<TC2NS1NS0_I2_Literal_Members>(true);
        #endregion
        #region To C2NS1SC0
        #endregion
        #region To C2NS1SS0
        #endregion
        #region To C2NS1SNS0
        #endregion
        #region To S2C1C0
        [Fact]
        public void To_S2C1C0_I0_Members() => ToStruct<TS2C1C0_I0_Members>(false);
        [Fact]
        public void To_S2C1C0_I1_Nullable_Members() => ToStruct<TS2C1C0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S2C1C0_I2_Literal_Members() => ToStruct<TS2C1C0_I2_Literal_Members>(true);
        [Fact]
        public void To_S2C1C0_I3_Readonly_Members() => ToStruct<TS2C1C0_I3_Readonly_Members>(true);
        #endregion
        #region To NS2C1C0
        [Fact]
        public void To_NS2C1C0_I0_Members() => ToNullableStruct<TS2C1C0_I0_Members>(false);
        [Fact]
        public void To_NS2C1C0_I1_Nullable_Members() => ToNullableStruct<TS2C1C0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS2C1C0_I2_Literal_Members() => ToNullableStruct<TS2C1C0_I2_Literal_Members>(true);
        [Fact]
        public void To_NS2C1C0_I3_Readonly_Members() => ToNullableStruct<TS2C1C0_I3_Readonly_Members>(true);
        #endregion
        #region To S2C1S0
        [Fact]
        public void To_S2C1S0_I0_Members() => ToStruct<TS2C1S0_I0_Members>(false);
        [Fact]
        public void To_S2C1S0_I1_Nullable_Members() => ToStruct<TS2C1S0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S2C1S0_I2_Literal_Members() => ToStruct<TS2C1S0_I2_Literal_Members>(true);
        #endregion
        #region To NS2C1S0
        [Fact]
        public void To_NS2C1S0_I0_Members() => ToNullableStruct<TS2C1S0_I0_Members>(false);
        [Fact]
        public void To_NS2C1S0_I1_Nullable_Members() => ToNullableStruct<TS2C1S0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS2C1S0_I2_Literal_Members() => ToNullableStruct<TS2C1S0_I2_Literal_Members>(true);
        #endregion
        #region To S2C1NS0
        [Fact]
        public void To_S2C1NS0_I0_Members() => ToStruct<TS2C1NS0_I0_Members>(false);
        [Fact]
        public void To_S2C1NS0_I1_Nullable_Members() => ToStruct<TS2C1NS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S2C1NS0_I2_Literal_Members() => ToStruct<TS2C1NS0_I2_Literal_Members>(true);
        #endregion
        #region To NS2C1NS0
        [Fact]
        public void To_NS2C1NS0_I0_Members() => ToNullableStruct<TS2C1NS0_I0_Members>(false);
        [Fact]
        public void To_NS2C1NS0_I1_Nullable_Members() => ToNullableStruct<TS2C1NS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS2C1NS0_I2_Literal_Members() => ToNullableStruct<TS2C1NS0_I2_Literal_Members>(true);
        #endregion
        #region To S2C1SC0
        #endregion
        #region To NS2C1SC0
        #endregion
        #region To S2C1SS0
        #endregion
        #region To NS2C1SS0
        #endregion
        #region To S2C1SNS0
        #endregion
        #region To NS2C1SNS0
        #endregion
        #region To S2S1C0
        [Fact]
        public void To_S2S1C0_I0_Members() => ToStruct<TS2S1C0_I0_Members>(false);
        [Fact]
        public void To_S2S1C0_I1_Nullable_Members() => ToStruct<TS2S1C0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S2S1C0_I2_Literal_Members() => ToStruct<TS2S1C0_I2_Literal_Members>(true);
        [Fact]
        public void To_S2S1C0_I3_Readonly_Members() => ToStruct<TS2S1C0_I3_Readonly_Members>(true);
        #endregion
        #region To NS2S1C0
        [Fact]
        public void To_NS2S1C0_I0_Members() => ToNullableStruct<TS2S1C0_I0_Members>(false);
        [Fact]
        public void To_NS2S1C0_I1_Nullable_Members() => ToNullableStruct<TS2S1C0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS2S1C0_I2_Literal_Members() => ToNullableStruct<TS2S1C0_I2_Literal_Members>(true);
        [Fact]
        public void To_NS2S1C0_I3_Readonly_Members() => ToNullableStruct<TS2S1C0_I3_Readonly_Members>(true);
        #endregion
        #region To S2S1S0
        [Fact]
        public void To_S2S1S0_I0_Members() => ToStruct<TS2S1S0_I0_Members>(false);
        [Fact]
        public void To_S2S1S0_I1_Nullable_Members() => ToStruct<TS2S1S0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S2S1S0_I2_Literal_Members() => ToStruct<TS2S1S0_I2_Literal_Members>(true);
        #endregion
        #region To NS2S1S0
        [Fact]
        public void To_NS2S1S0_I0_Members() => ToNullableStruct<TS2S1S0_I0_Members>(false);
        [Fact]
        public void To_NS2S1S0_I1_Nullable_Members() => ToNullableStruct<TS2S1S0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS2S1S0_I2_Literal_Members() => ToNullableStruct<TS2S1S0_I2_Literal_Members>(true);
        #endregion
        #region To S2S1NS0
        [Fact]
        public void To_S2S1NS0_I0_Members() => ToStruct<TS2S1NS0_I0_Members>(false);
        [Fact]
        public void To_S2S1NS0_I1_Nullable_Members() => ToStruct<TS2S1NS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S2S1NS0_I2_Literal_Members() => ToStruct<TS2S1NS0_I2_Literal_Members>(true);
        #endregion
        #region To NS2S1NS0
        [Fact]
        public void To_NS2S1NS0_I0_Members() => ToNullableStruct<TS2S1NS0_I0_Members>(false);
        [Fact]
        public void To_NS2S1NS0_I1_Nullable_Members() => ToNullableStruct<TS2S1NS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS2S1NS0_I2_Literal_Members() => ToNullableStruct<TS2S1NS0_I2_Literal_Members>(true);
        #endregion
        #region To S2S1SC0
        #endregion
        #region To NS2S1SC0
        #endregion
        #region To S2S1SS0
        #endregion
        #region To NS2S1SS0
        #endregion
        #region To S2S1SNS0
        #endregion
        #region To NS2S1SNS0
        #endregion
        #region To S2NS1C0
        [Fact]
        public void To_S2NS1C0_I0_Members() => ToStruct<TS2NS1C0_I0_Members>(false);
        [Fact]
        public void To_S2NS1C0_I1_Nullable_Members() => ToStruct<TS2NS1C0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S2NS1C0_I2_Literal_Members() => ToStruct<TS2NS1C0_I2_Literal_Members>(true);
        [Fact]
        public void To_S2NS1C0_I3_Readonly_Members() => ToStruct<TS2NS1C0_I3_Readonly_Members>(true);
        #endregion
        #region To NS2NS1C0
        [Fact]
        public void To_NS2NS1C0_I0_Members() => ToNullableStruct<TS2NS1C0_I0_Members>(false);
        [Fact]
        public void To_NS2NS1C0_I1_Nullable_Members() => ToNullableStruct<TS2NS1C0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS2NS1C0_I2_Literal_Members() => ToNullableStruct<TS2NS1C0_I2_Literal_Members>(true);
        [Fact]
        public void To_NS2NS1C0_I3_Readonly_Members() => ToNullableStruct<TS2NS1C0_I3_Readonly_Members>(true);
        #endregion
        #region To S2NS1S0
        [Fact]
        public void To_S2NS1S0_I0_Members() => ToStruct<TS2NS1S0_I0_Members>(false);
        [Fact]
        public void To_S2NS1S0_I1_Nullable_Members() => ToStruct<TS2NS1S0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S2NS1S0_I2_Literal_Members() => ToStruct<TS2NS1S0_I2_Literal_Members>(true);
        #endregion
        #region To NS2NS1S0
        [Fact]
        public void To_NS2NS1S0_I0_Members() => ToNullableStruct<TS2NS1S0_I0_Members>(false);
        [Fact]
        public void To_NS2NS1S0_I1_Nullable_Members() => ToNullableStruct<TS2NS1S0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS2NS1S0_I2_Literal_Members() => ToNullableStruct<TS2NS1S0_I2_Literal_Members>(true);
        #endregion
        #region To S2NS1NS0
        [Fact]
        public void To_S2NS1NS0_I0_Members() => ToStruct<TS2NS1NS0_I0_Members>(false);
        [Fact]
        public void To_S2NS1NS0_I1_Nullable_Members() => ToStruct<TS2NS1NS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_S2NS1NS0_I2_Literal_Members() => ToStruct<TS2NS1NS0_I2_Literal_Members>(true);
        #endregion
        #region To NS2NS1NS0
        [Fact]
        public void To_NS2NS1NS0_I0_Members() => ToNullableStruct<TS2NS1NS0_I0_Members>(false);
        [Fact]
        public void To_NS2NS1NS0_I1_Nullable_Members() => ToNullableStruct<TS2NS1NS0_I1_Nullable_Members>(false);
        [Fact]
        public void To_NS2NS1NS0_I2_Literal_Members() => ToNullableStruct<TS2NS1NS0_I2_Literal_Members>(true);
        #endregion
        #region To S2NS1SC0
        #endregion
        #region To NS2NS1SC0
        #endregion
        #region To S2NS1SS0
        #endregion
        #region To NS2NS1SS0
        #endregion
        #region To S2NS1SNS0
        #endregion
        #region To NS2NS1SNS0
        #endregion
        #region To C2SC1C0
        #endregion
        #region To C2SC1S0
        #endregion
        #region To C2SC1NS0
        #endregion
        #region To C2SC1SC0
        #endregion
        #region To C2SC1SS0
        #endregion
        #region To C2SC1SNS0
        #endregion
        #region To C2SS1C0
        #endregion
        #region To C2SS1S0
        #endregion
        #region To C2SS1NS0
        #endregion
        #region To C2SS1SC0
        #endregion
        #region To C2SS1SS0
        #endregion
        #region To C2SS1SNS0
        #endregion
        #region To C2SNS1C0
        #endregion
        #region To C2SNS1S0
        #endregion
        #region To C2SNS1NS0
        #endregion
        #region To C2SNS1SC0
        #endregion
        #region To C2SNS1SS0
        #endregion
        #region To C2SNS1SNS0
        #endregion
        #region To S2SC1C0
        #endregion
        #region To NS2SC1C0
        #endregion
        #region To S2SC1S0
        #endregion
        #region To NS2SC1S0
        #endregion
        #region To S2SC1NS0
        #endregion
        #region To NS2SC1NS0
        #endregion
        #region To S2SC1SC0
        #endregion
        #region To NS2SC1SC0
        #endregion
        #region To S2SC1SS0
        #endregion
        #region To NS2SC1SS0
        #endregion
        #region To S2SC1SNS0
        #endregion
        #region To NS2SC1SNS0
        #endregion
        #region To S2SS1C0
        #endregion
        #region To NS2SS1C0
        #endregion
        #region To S2SS1S0
        #endregion
        #region To NS2SS1S0
        #endregion
        #region To S2SS1NS0
        #endregion
        #region To NS2SS1NS0
        #endregion
        #region To S2SS1SC0
        #endregion
        #region To NS2SS1SC0
        #endregion
        #region To S2SS1SS0
        #endregion
        #region To NS2SS1SS0
        #endregion
        #region To S2SS1SNS0
        #endregion
        #region To NS2SS1SNS0
        #endregion
        #region To S2SNS1C0
        #endregion
        #region To NS2SNS1C0
        #endregion
        #region To S2SNS1S0
        #endregion
        #region To NS2SNS1S0
        #endregion
        #region To S2SNS1NS0
        #endregion
        #region To NS2SNS1NS0
        #endregion
        #region To S2SNS1SC0
        #endregion
        #region To NS2SNS1SC0
        #endregion
        #region To S2SNS1SS0
        #endregion
        #region To NS2SNS1SS0
        #endregion
        #region To S2SNS1SNS0
        #endregion
        #region To NS2SNS1SNS0
        #endregion
    }
}
