using Air.Mapper;
using AutoFixture;
using Benchmark.Benchmarks;
using BenchmarkDotNet.Running;
using Mapster;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Benchmark
{
    public class Program
    {
        private static void Main(string[] args)
        {


            //new From_Account_To_AccountDto();

            //BenchmarkRunner.Run<IterateIEnumerable>();

            //BenchmarkRunner.Run<CollectionsToArray>();

            BenchmarkRunner.Run<From_Account_To_AccountDto>();

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


            //RunBenchmark();
            Console.ReadLine();
        }


        private static void WriteLine(string method, Stopwatch stopwatch) =>
         Console.WriteLine("{0,-18};{1,-32}",
             stopwatch.Elapsed,
             method);


        public static void RunBenchmark()
        {
            var entries = 102400;
            var source = new List<TC0_I0_Members>();
            for (int i = 0; i < entries; i++)
                source.Add(new Fixture().Create<TC0_I0_Members>());

            var destination = new TC0_I0_Members[entries];


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            destination = source.Adapt<TC0_I0_Members[]>();
            stopwatch.Stop();
            WriteLine("MapsterAdapt", stopwatch);

            int runs = 0;
            while (runs < 2)
            {
                Console.WriteLine($" =======  Run: {runs}; Actions: {source.Count};  =======");
                stopwatch.Restart();
                for (int i = 0; i < source.Count; i++)
                    destination[i] = Mapper<TC0_I0_Members, TC0_I0_Members>.Map(source[i]);
                stopwatch.Stop();
                WriteLine("AirMapper", stopwatch);

                stopwatch.Restart();
                for (int i = 0; i < source.Count; i++)
                    destination[i] = source[i].Adapt<TC0_I0_Members>();
                stopwatch.Stop();
                WriteLine("MapsterMap", stopwatch);

                stopwatch.Restart();
                var airMapperMap = Mapper<TC0_I0_Members, TC0_I0_Members>.CompiledFunc;
                for (int i = 0; i < source.Count; i++)
                    destination[i] = airMapperMap(source[i]);
                stopwatch.Stop();
                WriteLine("AirMapperCompiledFunc", stopwatch);

                runs += 1;
            }
        }

    }
}
