``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |      Mean |    Error |   StdDev |
|----------------- |----------:|---------:|---------:|
| ExpressMapperMap | 151.24 ns | 2.268 ns | 2.121 ns |
|   AgileMapperMap | 205.96 ns | 1.293 ns | 1.146 ns |
|    TinyMapperMap |  50.15 ns | 0.980 ns | 1.742 ns |
|    AutoMapperMap | 167.12 ns | 1.940 ns | 1.720 ns |
|       MapsterMap |  61.86 ns | 0.506 ns | 0.395 ns |
|     AirMapperMap |  24.07 ns | 0.436 ns | 0.364 ns |
