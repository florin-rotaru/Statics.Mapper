``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |     Error |    StdDev |
|----------------- |------------:|----------:|----------:|
| ExpressMapperMap | 4,155.47 ns | 11.949 ns | 11.177 ns |
|   AgileMapperMap |   602.24 ns |  6.313 ns |  5.596 ns |
|    TinyMapperMap |          NA |        NA |        NA |
|    AutoMapperMap |   555.76 ns |  8.444 ns |  7.486 ns |
|       MapsterMap |   130.51 ns |  0.778 ns |  0.690 ns |
|     AirMapperMap |    88.77 ns |  1.193 ns |  1.116 ns |

Benchmarks with issues:
  From_TC0_Members_To_TS0_I2_Nullable_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
