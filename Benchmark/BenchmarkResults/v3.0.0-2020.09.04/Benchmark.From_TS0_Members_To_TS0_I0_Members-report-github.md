``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |     Error |    StdDev |
|----------------- |------------:|----------:|----------:|
| ExpressMapperMap | 2,927.59 ns | 56.744 ns | 53.079 ns |
|   AgileMapperMap |   588.73 ns |  2.124 ns |  1.883 ns |
|    TinyMapperMap |          NA |        NA |        NA |
|    AutoMapperMap |   419.81 ns |  1.119 ns |  0.992 ns |
|       MapsterMap |   108.18 ns |  2.043 ns |  1.706 ns |
|     AirMapperMap |    44.93 ns |  0.133 ns |  0.124 ns |

Benchmarks with issues:
  From_TS0_Members_To_TS0_I0_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
