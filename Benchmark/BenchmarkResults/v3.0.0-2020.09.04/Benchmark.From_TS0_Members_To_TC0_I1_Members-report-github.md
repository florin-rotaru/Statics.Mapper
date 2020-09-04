``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |      Mean |     Error |    StdDev |
|----------------- |----------:|----------:|----------:|
| ExpressMapperMap | 889.15 ns | 11.138 ns | 10.419 ns |
|   AgileMapperMap | 556.24 ns |  5.464 ns |  4.563 ns |
|    TinyMapperMap |        NA |        NA |        NA |
|    AutoMapperMap | 473.16 ns |  5.979 ns |  5.300 ns |
|       MapsterMap | 120.65 ns |  2.064 ns |  1.931 ns |
|     AirMapperMap |  63.07 ns |  0.852 ns |  0.755 ns |

Benchmarks with issues:
  From_TS0_Members_To_TC0_I1_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
