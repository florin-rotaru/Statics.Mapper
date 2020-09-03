``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |     Error |    StdDev |
|----------------- |------------:|----------:|----------:|
| ExpressMapperMap | 2,298.00 ns | 41.398 ns | 38.724 ns |
|   AgileMapperMap |   387.12 ns |  4.928 ns |  4.369 ns |
|    TinyMapperMap |          NA |        NA |        NA |
|    AutoMapperMap |   221.15 ns |  0.444 ns |  0.393 ns |
|       MapsterMap |    68.25 ns |  0.971 ns |  0.811 ns |
|     AirMapperMap |    28.80 ns |  0.451 ns |  0.422 ns |

Benchmarks with issues:
  From_TC0_Members_To_TS0_I0_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
