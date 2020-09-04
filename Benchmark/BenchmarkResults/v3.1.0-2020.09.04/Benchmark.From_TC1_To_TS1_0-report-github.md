``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |       Mean |    Error |   StdDev |
|----------------- |-----------:|---------:|---------:|
| ExpressMapperMap |   506.1 ns |  9.94 ns | 10.21 ns |
|   AgileMapperMap |   551.5 ns | 10.48 ns |  9.81 ns |
|    AutoMapperMap | 1,005.3 ns | 19.10 ns | 17.87 ns |
|       MapsterMap |   234.8 ns |  4.76 ns |  4.68 ns |
|     AirMapperMap |   182.2 ns |  3.59 ns |  3.36 ns |
