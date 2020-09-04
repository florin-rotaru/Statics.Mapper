``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |       Mean |    Error |   StdDev |     Median |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |-----------:|---------:|---------:|-----------:|-------:|------:|------:|----------:|
| ExpressMapperMap | 2,017.8 ns | 37.52 ns | 60.58 ns | 2,011.7 ns | 0.9499 |     - |     - |   3.89 KB |
|   AgileMapperMap | 1,019.0 ns | 22.21 ns | 63.01 ns |   992.2 ns | 0.8221 |     - |     - |   3.36 KB |
|    TinyMapperMap | 3,895.7 ns | 52.91 ns | 46.91 ns | 3,905.7 ns | 0.8240 |     - |     - |   3.38 KB |
|    AutoMapperMap | 2,311.8 ns | 38.76 ns | 36.26 ns | 2,330.7 ns | 0.7706 |     - |     - |   3.15 KB |
|       MapsterMap |   723.3 ns | 12.46 ns | 11.66 ns |   728.9 ns | 0.7362 |     - |     - |   3.01 KB |
|     AirMapperMap |   673.2 ns |  8.56 ns |  8.01 ns |   673.0 ns | 0.7362 |     - |     - |   3.01 KB |
