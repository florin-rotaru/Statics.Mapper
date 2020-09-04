``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |      Mean |    Error |   StdDev |
|----------------- |----------:|---------:|---------:|
| ExpressMapperMap | 213.40 ns | 3.841 ns | 3.593 ns |
|   AgileMapperMap | 226.02 ns | 3.782 ns | 3.538 ns |
|    TinyMapperMap | 124.15 ns | 2.511 ns | 2.348 ns |
|    AutoMapperMap | 174.00 ns | 3.069 ns | 2.870 ns |
|       MapsterMap |  64.15 ns | 1.251 ns | 1.170 ns |
|     AirMapperMap |  29.54 ns | 0.658 ns | 0.615 ns |
