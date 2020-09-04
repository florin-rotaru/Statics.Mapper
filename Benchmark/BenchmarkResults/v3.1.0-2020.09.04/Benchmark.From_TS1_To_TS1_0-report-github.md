``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |       Mean |    Error |   StdDev |
|----------------- |-----------:|---------:|---------:|
| ExpressMapperMap |   556.8 ns | 10.79 ns | 11.08 ns |
|   AgileMapperMap | 1,296.9 ns | 25.51 ns | 25.05 ns |
|    AutoMapperMap | 1,566.5 ns | 30.99 ns | 38.06 ns |
|       MapsterMap |   932.6 ns | 18.11 ns | 22.91 ns |
|     AirMapperMap |   227.5 ns |  4.16 ns |  3.89 ns |
