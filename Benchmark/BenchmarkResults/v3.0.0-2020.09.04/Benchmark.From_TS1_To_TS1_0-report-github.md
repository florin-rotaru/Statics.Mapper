``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |       Mean |    Error |   StdDev |
|----------------- |-----------:|---------:|---------:|
| ExpressMapperMap |   592.3 ns |  9.42 ns |  8.81 ns |
|   AgileMapperMap | 1,341.4 ns | 24.33 ns | 22.76 ns |
|    AutoMapperMap | 1,615.0 ns |  2.69 ns |  2.38 ns |
|       MapsterMap |   962.2 ns |  9.00 ns |  8.42 ns |
|     AirMapperMap |   234.7 ns |  4.19 ns |  3.92 ns |
