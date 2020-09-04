``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |     Error |    StdDev |
|----------------- |------------:|----------:|----------:|
| ExpressMapperMap | 3,999.58 ns | 11.044 ns | 10.331 ns |
|   AgileMapperMap |   668.96 ns | 12.263 ns | 11.471 ns |
|    TinyMapperMap |          NA |        NA |        NA |
|    AutoMapperMap |   511.69 ns |  1.310 ns |  1.161 ns |
|       MapsterMap |   117.49 ns |  1.760 ns |  1.646 ns |
|     AirMapperMap |    52.63 ns |  1.080 ns |  1.863 ns |

Benchmarks with issues:
  From_TS0_Members_To_TS0_I1_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
