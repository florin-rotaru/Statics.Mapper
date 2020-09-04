``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |      Mean |     Error |    StdDev |
|----------------- |----------:|----------:|----------:|
| ExpressMapperMap | 834.35 ns | 14.115 ns | 13.203 ns |
|   AgileMapperMap | 509.12 ns | 10.155 ns | 10.429 ns |
|    TinyMapperMap |        NA |        NA |        NA |
|    AutoMapperMap | 434.37 ns |  6.581 ns |  6.156 ns |
|       MapsterMap | 106.29 ns |  1.964 ns |  1.837 ns |
|     AirMapperMap |  57.83 ns |  1.199 ns |  1.121 ns |

Benchmarks with issues:
  From_TS0_Members_To_TC0_I1_Members.TinyMapperMap: InProcess(Toolchain=InProcessEmitToolchain)
