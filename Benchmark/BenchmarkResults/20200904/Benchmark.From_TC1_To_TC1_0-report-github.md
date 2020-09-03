``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |     Mean |   Error |  StdDev |
|----------------- |---------:|--------:|--------:|
| ExpressMapperMap | 492.1 ns | 6.20 ns | 5.80 ns |
|   AgileMapperMap | 506.1 ns | 9.00 ns | 8.42 ns |
|    AutoMapperMap | 824.2 ns | 9.83 ns | 9.19 ns |
|       MapsterMap | 154.3 ns | 2.65 ns | 2.48 ns |
|     AirMapperMap | 111.2 ns | 1.62 ns | 1.51 ns |
