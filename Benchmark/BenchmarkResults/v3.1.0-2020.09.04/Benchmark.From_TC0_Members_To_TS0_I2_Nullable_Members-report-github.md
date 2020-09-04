``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |     Error |    StdDev |
|----------------- |------------:|----------:|----------:|
| ExpressMapperMap | 4,005.39 ns | 74.738 ns | 69.910 ns |
|   AgileMapperMap |   541.79 ns |  8.908 ns |  7.439 ns |
|    TinyMapperMap |          NA |        NA |        NA |
|    AutoMapperMap |   531.43 ns | 10.593 ns | 10.878 ns |
|       MapsterMap |   128.23 ns |  0.123 ns |  0.115 ns |
|     AirMapperMap |    85.98 ns |  1.709 ns |  1.679 ns |

Benchmarks with issues:
  From_TC0_Members_To_TS0_I2_Nullable_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
