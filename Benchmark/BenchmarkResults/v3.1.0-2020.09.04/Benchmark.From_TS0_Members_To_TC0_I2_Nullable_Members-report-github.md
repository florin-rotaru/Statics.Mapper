``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |      Mean |     Error |    StdDev |
|----------------- |----------:|----------:|----------:|
| ExpressMapperMap | 959.89 ns | 18.961 ns | 20.289 ns |
|   AgileMapperMap | 562.63 ns | 11.237 ns | 11.540 ns |
|    TinyMapperMap |        NA |        NA |        NA |
|    AutoMapperMap | 687.38 ns | 12.336 ns | 11.539 ns |
|       MapsterMap | 137.90 ns |  2.842 ns |  2.792 ns |
|     AirMapperMap |  99.66 ns |  1.859 ns |  1.739 ns |

Benchmarks with issues:
  From_TS0_Members_To_TC0_I2_Nullable_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
