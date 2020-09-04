``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |      Mean |    Error |   StdDev |
|----------------- |----------:|---------:|---------:|
| ExpressMapperMap | 462.84 ns | 7.740 ns | 7.240 ns |
|   AgileMapperMap | 447.04 ns | 7.962 ns | 9.168 ns |
|    AutoMapperMap | 806.92 ns | 1.481 ns | 1.313 ns |
|       MapsterMap | 136.88 ns | 2.814 ns | 2.764 ns |
|     AirMapperMap |  99.67 ns | 1.980 ns | 1.945 ns |
