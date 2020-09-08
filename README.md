# What is Air.Mapper?

Is a simple and fast open source mapping library which allows to map one type members into another.

# Overview

- maps members based on conventions (public properties with the same names and same/derived/convertible types)
- compiles func / action delegates mappers

You can install it via [package manager console](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-powershell)
```
PM> Install-Package Air.Mapper
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
mapAction(source, ref destination);
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
|           Method |       Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |-----------:|----------:|----------:|-------:|------:|------:|----------:|
| ExpressMapperMap | 2,001.0 ns |  39.16 ns |  34.72 ns | 0.9499 |     - |     - |   3.89 KB |
|   AgileMapperMap | 1,056.9 ns |  20.86 ns |  37.61 ns | 0.8221 |     - |     - |   3.36 KB |
|    TinyMapperMap | 4,052.6 ns |  59.84 ns | 104.80 ns | 0.8240 |     - |     - |   3.38 KB |
|    AutoMapperMap | 2,209.0 ns |  44.10 ns |  50.78 ns | 0.7706 |     - |     - |   3.15 KB |
|       MapsterMap |   729.4 ns |  14.50 ns |  13.57 ns | 0.7362 |     - |     - |   3.01 KB |
|     AirMapperMap |   668.4 ns |  12.21 ns |  11.42 ns | 0.7362 |     - |     - |   3.01 KB |
| HigLaboMapperMap |   525.0 ns |  10.27 ns |  11.42 ns | 0.3977 |     - |     - |   1.63 KB |
|    FastMapperMap | 5,545.8 ns | 108.21 ns | 101.22 ns | 1.9608 |     - |     - |   8.02 KB |

in the above results the HigLaboMapperMap missed some member mappings

### More benchmark results
https://github.com/florin-rotaru/NetMappers.Benchmarks/tree/master/NetMappers.Benchmarks/BenchmarksResults/2020.09.06

### Benchmark results summary
|Library                 |Passed      |Failed
|----------------------- |----------- |-----------
|ExpressMapper           |16          |0
|AgileMapper             |16          |0
|TinyMapper              |4           |12
|AutoMapper              |16          |0
|Mapster                 |16          |0
|AirMapper               |16          |0
|HigLaboObjectMapper     |13          |3
|FastMapper              |1           |15

- passed: no exception thrown nor differences between source and destination members
- failed: exception thrown or differences found between source and destination members
  - [exceptions](https://github.com/florin-rotaru/NetMappers.Benchmarks/blob/master/NetMappers.Benchmarks/BenchmarksResults/2020.09.06/Failed.Exceptions.md)
  - [diffs](https://github.com/florin-rotaru/NetMappers.Benchmarks/blob/master/NetMappers.Benchmarks/BenchmarksResults/2020.09.06/Failed.Diffs.md)
