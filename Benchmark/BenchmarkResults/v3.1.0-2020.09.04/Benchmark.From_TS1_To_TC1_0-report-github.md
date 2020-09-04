``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |       Mean |    Error |   StdDev |     Median |
|----------------- |-----------:|---------:|---------:|-----------:|
| ExpressMapperMap |   512.8 ns | 10.27 ns | 12.23 ns |   513.9 ns |
|   AgileMapperMap | 1,165.6 ns | 14.12 ns | 11.79 ns | 1,167.9 ns |
|    AutoMapperMap | 1,387.9 ns | 26.60 ns | 28.46 ns | 1,408.5 ns |
|       MapsterMap |   855.9 ns | 14.23 ns | 13.31 ns |   856.7 ns |
|     AirMapperMap |   147.6 ns |  2.96 ns |  2.77 ns |   149.0 ns |
