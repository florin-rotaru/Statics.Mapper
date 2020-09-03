``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.15063.2106 (1703/CreatorsUpdate/Redstone2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2648436 Hz, Resolution=377.5813 ns, Timer=TSC
.NET Core SDK=3.1.401
  [Host] : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|         Method |      Mean |     Error |    StdDev |
|--------------- |----------:|----------:|----------:|
| ObjectToString | 36.554 ns | 0.7092 ns | 0.6965 ns |
|        GetName |  7.530 ns | 0.2316 ns | 0.2275 ns |
