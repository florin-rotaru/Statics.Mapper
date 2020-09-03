``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |     Error |    StdDev |
|----------------- |------------:|----------:|----------:|
| ExpressMapperMap | 4,704.32 ns | 63.768 ns | 53.249 ns |
|   AgileMapperMap |   778.03 ns | 12.219 ns | 11.429 ns |
|    TinyMapperMap |          NA |        NA |        NA |
|    AutoMapperMap |   806.37 ns |  1.689 ns |  1.580 ns |
|       MapsterMap |   158.38 ns |  2.491 ns |  2.330 ns |
|     AirMapperMap |    95.27 ns |  1.751 ns |  1.638 ns |

Benchmarks with issues:
  From_TS0_Members_To_TS0_I2_Nullable_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
