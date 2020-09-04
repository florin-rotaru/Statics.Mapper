``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |       Mean |    Error |   StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |-----------:|---------:|---------:|-------:|------:|------:|----------:|
| ExpressMapperMap | 2,145.4 ns | 42.41 ns | 39.67 ns | 0.9499 |     - |     - |   3.89 KB |
|   AgileMapperMap | 1,114.7 ns | 22.20 ns | 21.80 ns | 0.8221 |     - |     - |   3.36 KB |
|    TinyMapperMap | 4,000.0 ns | 10.79 ns | 10.09 ns | 0.8240 |     - |     - |   3.38 KB |
|    AutoMapperMap | 2,320.4 ns |  9.46 ns |  8.85 ns | 0.7706 |     - |     - |   3.15 KB |
|       MapsterMap |   813.6 ns | 11.51 ns | 10.77 ns | 0.7362 |     - |     - |   3.01 KB |
|     AirMapperMap |   760.3 ns | 10.18 ns |  9.52 ns | 0.7362 |     - |     - |   3.01 KB |
