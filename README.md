# What is Air.Mapper?

Is a simple and fast open source mapping library which allows to map one type members into another.

# Overview

- maps members based on conventions (public properties with the same names and same/derived/convertible types)
- compiles func / action delegates mappers

You can install it via [package manager console](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-powershell)
```
PM> Install-Package Mapster
```

## Basic usage

Create a new object
```csharp
var destination = Mapper<TSource, TDestination>.Map(source);
```

Update object
```csharp
Mapper<TSource, TDestination>.Map(source, ref destination)
```

Create a map function
```csharp
var mapFunction = Mapper<TSource, TDestination>.CompileFunc();
var destination = mapFunction(source);
```

Create a map action
```csharp
var mapAction = Mapper<TSource, TDestination>.CompileActionRef();
var destination = mapAction(source, ref destination);
```

Setting map option
```csharp
var mapFunction = Mapper<TSource, TDestination>.CompileFunc(o => o
  .Ignore(i => i.DestinationMember)
  .Map(s => s.SourceMember, d => d.DestinationMember));
```
or
```csharp
MapperConfig<TSource, TDestination>.SetOptions(o => o
  .Ignore(i => i.DestinationMember)
  .Map(s => s.SourceMember, d => d.DestinationMember));
```

## Built with performance in mind 
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

More [benchmark results](https://github.com/florin-rotaru/Air.Mapper/tree/master/Benchmark/BenchmarkResults/20200904)
