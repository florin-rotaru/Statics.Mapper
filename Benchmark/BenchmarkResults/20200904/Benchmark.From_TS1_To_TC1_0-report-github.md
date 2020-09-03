``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |       Mean |    Error |   StdDev |
|----------------- |-----------:|---------:|---------:|
| ExpressMapperMap |   535.0 ns |  7.16 ns |  6.69 ns |
|   AgileMapperMap | 1,225.0 ns |  4.17 ns |  3.90 ns |
|    AutoMapperMap | 1,454.1 ns | 23.15 ns | 21.65 ns |
|       MapsterMap |   889.8 ns |  9.80 ns |  8.69 ns |
|     AirMapperMap |   168.8 ns |  1.58 ns |  1.48 ns |
