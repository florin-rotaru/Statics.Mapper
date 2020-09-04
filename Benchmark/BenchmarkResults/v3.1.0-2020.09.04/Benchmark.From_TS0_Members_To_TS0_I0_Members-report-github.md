``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |     Error |    StdDev |
|----------------- |------------:|----------:|----------:|
| ExpressMapperMap | 2,858.13 ns | 38.902 ns | 36.389 ns |
|   AgileMapperMap |   540.82 ns | 10.250 ns |  9.588 ns |
|    TinyMapperMap |          NA |        NA |        NA |
|    AutoMapperMap |   402.25 ns |  6.523 ns |  6.102 ns |
|       MapsterMap |   101.62 ns |  1.587 ns |  1.485 ns |
|     AirMapperMap |    43.72 ns |  0.905 ns |  0.968 ns |

Benchmarks with issues:
  From_TS0_Members_To_TS0_I0_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
