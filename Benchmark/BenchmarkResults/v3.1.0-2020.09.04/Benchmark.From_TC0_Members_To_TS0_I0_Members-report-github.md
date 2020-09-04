``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |     Error |    StdDev |
|----------------- |------------:|----------:|----------:|
| ExpressMapperMap | 2,271.46 ns | 32.915 ns | 30.789 ns |
|   AgileMapperMap |   358.45 ns |  4.165 ns |  3.692 ns |
|    TinyMapperMap |          NA |        NA |        NA |
|    AutoMapperMap |   215.00 ns |  3.998 ns |  3.739 ns |
|       MapsterMap |    67.03 ns |  1.290 ns |  1.077 ns |
|     AirMapperMap |    28.85 ns |  0.591 ns |  0.553 ns |

Benchmarks with issues:
  From_TC0_Members_To_TS0_I0_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
