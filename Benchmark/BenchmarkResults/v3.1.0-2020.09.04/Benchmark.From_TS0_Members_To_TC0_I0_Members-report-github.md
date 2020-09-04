``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |      Mean |    Error |    StdDev |
|----------------- |----------:|---------:|----------:|
| ExpressMapperMap | 789.80 ns | 9.985 ns |  9.340 ns |
|   AgileMapperMap | 494.97 ns | 9.615 ns | 10.288 ns |
|    TinyMapperMap |        NA |       NA |        NA |
|    AutoMapperMap | 356.70 ns | 6.300 ns |  5.893 ns |
|       MapsterMap |  98.28 ns | 1.495 ns |  1.398 ns |
|     AirMapperMap |  53.22 ns | 1.068 ns |  0.999 ns |

Benchmarks with issues:
  From_TS0_Members_To_TC0_I0_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
