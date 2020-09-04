``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |     Error |    StdDev |
|----------------- |------------:|----------:|----------:|
| ExpressMapperMap | 3,413.78 ns | 25.757 ns | 21.509 ns |
|   AgileMapperMap |   473.68 ns |  6.610 ns |  6.183 ns |
|    TinyMapperMap |          NA |        NA |        NA |
|    AutoMapperMap |   243.67 ns |  2.272 ns |  1.774 ns |
|       MapsterMap |    85.99 ns |  0.171 ns |  0.160 ns |
|     AirMapperMap |    39.17 ns |  0.377 ns |  0.353 ns |

Benchmarks with issues:
  From_TC0_Members_To_TS0_I1_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
