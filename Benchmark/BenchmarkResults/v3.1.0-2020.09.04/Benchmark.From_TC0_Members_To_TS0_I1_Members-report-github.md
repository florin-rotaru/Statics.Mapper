``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |    Error |   StdDev |
|----------------- |------------:|---------:|---------:|
| ExpressMapperMap | 3,325.05 ns | 5.376 ns | 4.489 ns |
|   AgileMapperMap |   430.62 ns | 0.449 ns | 0.398 ns |
|    TinyMapperMap |          NA |       NA |       NA |
|    AutoMapperMap |   236.44 ns | 4.371 ns | 4.089 ns |
|       MapsterMap |    83.97 ns | 1.661 ns | 1.631 ns |
|     AirMapperMap |    38.58 ns | 0.039 ns | 0.036 ns |

Benchmarks with issues:
  From_TC0_Members_To_TS0_I1_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
