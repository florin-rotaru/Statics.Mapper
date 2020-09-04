``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |        Mean |     Error |    StdDev |
|----------------- |------------:|----------:|----------:|
| ExpressMapperMap |   330.85 ns |  5.587 ns |  5.227 ns |
|   AgileMapperMap |   261.36 ns |  4.829 ns |  4.517 ns |
|    TinyMapperMap | 1,039.87 ns | 19.306 ns | 15.073 ns |
|    AutoMapperMap |   427.02 ns |  7.337 ns |  6.126 ns |
|       MapsterMap |    93.65 ns |  1.840 ns |  1.721 ns |
|     AirMapperMap |    58.65 ns |  1.104 ns |  2.255 ns |
