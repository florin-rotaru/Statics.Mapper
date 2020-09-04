``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |     Error |    StdDev |
|----------------- |------------:|----------:|----------:|
| ExpressMapperMap | 3,918.39 ns | 43.242 ns | 38.333 ns |
|   AgileMapperMap |   591.53 ns |  9.850 ns |  9.214 ns |
|    TinyMapperMap |          NA |        NA |        NA |
|    AutoMapperMap |   482.50 ns |  9.140 ns |  8.102 ns |
|       MapsterMap |   111.23 ns |  1.531 ns |  1.279 ns |
|     AirMapperMap |    49.56 ns |  0.918 ns |  0.858 ns |

Benchmarks with issues:
  From_TS0_Members_To_TS0_I1_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
