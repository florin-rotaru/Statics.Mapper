``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |       Mean |    Error |   StdDev |
|----------------- |-----------:|---------:|---------:|
| ExpressMapperMap | 1,058.2 ns |  3.50 ns |  3.28 ns |
|   AgileMapperMap |   622.4 ns | 10.71 ns | 10.02 ns |
|    TinyMapperMap |         NA |       NA |       NA |
|    AutoMapperMap |   702.8 ns |  9.32 ns |  8.72 ns |
|       MapsterMap |   152.9 ns |  3.13 ns |  4.87 ns |
|     AirMapperMap |   102.5 ns |  1.43 ns |  1.34 ns |

Benchmarks with issues:
  From_TS0_Members_To_TC0_I2_Nullable_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
