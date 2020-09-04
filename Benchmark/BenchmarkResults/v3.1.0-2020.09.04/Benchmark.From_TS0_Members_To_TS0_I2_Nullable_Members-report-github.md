``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |     Error |    StdDev |
|----------------- |------------:|----------:|----------:|
| ExpressMapperMap | 4,573.13 ns | 86.537 ns | 72.262 ns |
|   AgileMapperMap |   724.37 ns |  7.898 ns |  7.001 ns |
|    TinyMapperMap |          NA |        NA |        NA |
|    AutoMapperMap |   754.95 ns | 14.703 ns | 13.753 ns |
|       MapsterMap |   154.84 ns |  0.233 ns |  0.207 ns |
|     AirMapperMap |    95.43 ns |  0.041 ns |  0.035 ns |

Benchmarks with issues:
  From_TS0_Members_To_TS0_I2_Nullable_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
