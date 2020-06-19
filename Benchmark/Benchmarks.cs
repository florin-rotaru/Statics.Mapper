using Models;

namespace Benchmark
{
    public class From_TC0_Members_To_TC0_I0_Members : BenchmarkFromToN0<TC0_Members, TC0_I0_Members> { }
    public class From_TC0_Members_To_TC0_I1_Members : BenchmarkFromToN0<TC0_Members, TC0_I1_Members> { }
    public class From_TC0_Members_To_TC0_I2_Nullable_Members : BenchmarkFromToN0<TC0_Members, TC0_I2_Nullable_Members> { }

    public class From_TC0_Members_To_TS0_I0_Members : BenchmarkFromToN0<TC0_Members, TS0_I0_Members> { }
    public class From_TC0_Members_To_TS0_I1_Members : BenchmarkFromToN0<TC0_Members, TS0_I1_Members> { }
    public class From_TC0_Members_To_TS0_I2_Nullable_Members : BenchmarkFromToN0<TC0_Members, TS0_I2_Nullable_Members> { }


    public class From_TS0_Members_To_TC0_I0_Members : BenchmarkFromToN0<TS0_Members, TC0_I0_Members> { }
    public class From_TS0_Members_To_TC0_I1_Members : BenchmarkFromToN0<TS0_Members, TC0_I1_Members> { }
    public class From_TS0_Members_To_TC0_I2_Nullable_Members : BenchmarkFromToN0<TS0_Members, TC0_I2_Nullable_Members> { }

    public class From_TS0_Members_To_TS0_I0_Members : BenchmarkFromToN0<TS0_Members, TS0_I0_Members> { }
    public class From_TS0_Members_To_TS0_I1_Members : BenchmarkFromToN0<TS0_Members, TS0_I1_Members> { }
    public class From_TS0_Members_To_TS0_I2_Nullable_Members : BenchmarkFromToN0<TS0_Members, TS0_I2_Nullable_Members> { }


    public class From_TC1_To_TC1_0 : BenchmarkFromToN1<TC1, TC1_0> { }
    public class From_TC1_To_TS1_0 : BenchmarkFromToN1<TC1, TS1_0> { }

    public class From_TS1_To_TC1_0 : BenchmarkFromToN1<TS1, TC1_0> { }
    public class From_TS1_To_TS1_0 : BenchmarkFromToN1<TS1, TS1_0> { }
}
