using BenchmarkDotNet.Running;
using System;

namespace Benchmarks
{
    public class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<Collections>();
            BenchmarkRunner.Run<CollectionsToArray>();
            BenchmarkRunner.Run<EnumToString>();

            //BenchmarkRunner.Run<From_Account_To_AccountDto>();
            //BenchmarkRunner.Run<From_TC0_Members_To_TC0_I0_Members>();
            //BenchmarkRunner.Run<From_TC0_Members_To_TC0_I1_Members>();
            //BenchmarkRunner.Run<From_TC0_Members_To_TC0_I2_Nullable_Members>();

            //BenchmarkRunner.Run<From_TC0_Members_To_TS0_I0_Members>();
            //BenchmarkRunner.Run<From_TC0_Members_To_TS0_I1_Members>();
            //BenchmarkRunner.Run<From_TC0_Members_To_TS0_I2_Nullable_Members>();

            //BenchmarkRunner.Run<From_TS0_Members_To_TC0_I0_Members>();
            //BenchmarkRunner.Run<From_TS0_Members_To_TC0_I1_Members>();
            //BenchmarkRunner.Run<From_TS0_Members_To_TC0_I2_Nullable_Members>();

            //BenchmarkRunner.Run<From_TS0_Members_To_TS0_I0_Members>();
            //BenchmarkRunner.Run<From_TS0_Members_To_TS0_I1_Members>();
            //BenchmarkRunner.Run<From_TS0_Members_To_TS0_I2_Nullable_Members>();

            //BenchmarkRunner.Run<From_TC1_To_TC1_0>();
            //BenchmarkRunner.Run<From_TC1_To_TS1_0>();

            //BenchmarkRunner.Run<From_TS1_To_TC1_0>();
            //BenchmarkRunner.Run<From_TS1_To_TS1_0>();

            Console.ReadLine();
        }
    }
}
