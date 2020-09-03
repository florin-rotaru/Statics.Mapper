``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |      Mean |    Error |   StdDev |
|----------------- |----------:|---------:|---------:|
| ExpressMapperMap | 876.99 ns | 9.508 ns | 8.428 ns |
|   AgileMapperMap | 537.31 ns | 1.944 ns | 1.818 ns |
|    TinyMapperMap |        NA |       NA |       NA |
|    AutoMapperMap | 366.99 ns | 3.244 ns | 3.034 ns |
|       MapsterMap | 106.43 ns | 1.175 ns | 1.041 ns |
|     AirMapperMap |  58.19 ns | 0.659 ns | 0.616 ns |

Benchmarks with issues:
  From_TS0_Members_To_TC0_I0_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
