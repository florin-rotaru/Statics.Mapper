``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |       Mean |   Error |  StdDev |
|----------------- |-----------:|--------:|--------:|
| ExpressMapperMap |   568.9 ns | 6.93 ns | 6.48 ns |
|   AgileMapperMap |   611.5 ns | 7.85 ns | 6.56 ns |
|    AutoMapperMap | 1,039.1 ns | 7.79 ns | 7.28 ns |
|       MapsterMap |   255.1 ns | 4.90 ns | 4.81 ns |
|     AirMapperMap |   192.2 ns | 2.48 ns | 2.32 ns |
