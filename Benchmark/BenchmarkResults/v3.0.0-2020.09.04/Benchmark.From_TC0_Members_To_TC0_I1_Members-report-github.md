``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |      Mean |    Error |   StdDev |
|----------------- |----------:|---------:|---------:|
| ExpressMapperMap | 203.16 ns | 4.008 ns | 5.748 ns |
|   AgileMapperMap | 239.27 ns | 4.784 ns | 4.699 ns |
|    TinyMapperMap | 131.47 ns | 2.201 ns | 1.951 ns |
|    AutoMapperMap | 187.47 ns | 2.373 ns | 1.982 ns |
|       MapsterMap |  71.03 ns | 0.964 ns | 0.854 ns |
|     AirMapperMap |  34.19 ns | 0.517 ns | 0.484 ns |
