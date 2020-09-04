``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |      Mean |    Error |   StdDev |
|----------------- |----------:|---------:|---------:|
| ExpressMapperMap | 167.49 ns | 2.357 ns | 2.205 ns |
|   AgileMapperMap | 232.78 ns | 2.432 ns | 1.899 ns |
|    TinyMapperMap |  52.07 ns | 0.741 ns | 0.619 ns |
|    AutoMapperMap | 180.22 ns | 3.580 ns | 3.348 ns |
|       MapsterMap |  62.70 ns | 0.677 ns | 0.529 ns |
|     AirMapperMap |  27.52 ns | 0.636 ns | 0.653 ns |
