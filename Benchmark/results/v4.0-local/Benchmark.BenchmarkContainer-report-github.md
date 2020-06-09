``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.15063.2106 (1703/CreatorsUpdate/Redstone2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2648442 Hz, Resolution=377.5805 ns, Timer=TSC
.NET Core SDK=2.2.100
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), X64 RyuJIT  [AttachedDebugger]

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|                         Method |       Mean |    Error |   StdDev |
|------------------------------- |-----------:|---------:|---------:|
|           MapWithManualMapping |   619.8 ns |  9.70 ns |  9.07 ns |
|           MapWithExpressMapper | 1,935.6 ns | 33.56 ns | 29.75 ns |
|             MapWithAgileMapper | 1,343.2 ns | 20.30 ns | 18.99 ns |
|              MapWithTinyMapper | 1,664.0 ns | 16.88 ns | 14.96 ns |
|              MapWithAutoMapper | 2,015.5 ns | 35.41 ns | 33.12 ns |
|                 MapWithMapster |   743.7 ns |  8.25 ns |  7.71 ns |
|             MapWithStaticsMapper |   430.7 ns |  4.00 ns |  3.74 ns |
| MapWithStaticsMapperCompiledFunc |   427.2 ns |  4.25 ns |  3.55 ns |
