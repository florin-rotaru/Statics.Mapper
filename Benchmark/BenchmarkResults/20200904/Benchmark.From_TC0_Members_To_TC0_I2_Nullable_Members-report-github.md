``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |     Error |    StdDev |
|----------------- |------------:|----------:|----------:|
| ExpressMapperMap |   354.77 ns |  2.718 ns |  2.122 ns |
|   AgileMapperMap |   298.16 ns |  3.844 ns |  3.596 ns |
|    TinyMapperMap | 1,177.03 ns | 12.880 ns | 11.418 ns |
|    AutoMapperMap |   454.81 ns |  1.421 ns |  1.187 ns |
|       MapsterMap |   105.38 ns |  1.055 ns |  0.987 ns |
|     AirMapperMap |    65.74 ns |  1.156 ns |  1.081 ns |
