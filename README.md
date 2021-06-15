# What is Statics.Mapper?

Is a simple and fast open source mapping library which allows to map one type members into another.

# Overview

- maps members based on conventions (public properties with the same names and same/derived/convertible types)
- compiles func / action delegates mappers

You can install it via [package manager console](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-powershell)
```
PM> Install-Package Statics.Mapper
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

### Default Map Options
- bool expand = true
- bool useMapperConfig = true

#### Expand Option
When **true** maps all members and node members  
When **false** node members map will be skipped 

#### UseMapperConfig
When **true** MapperConfig options are applied

## Built with performance in mind 
``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19042.1052 (20H2/October2020Update)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=5.0.301
  [Host] : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |       Mean |    Error |   StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |-----------:|---------:|---------:|-------:|------:|------:|----------:|
| ExpressMapperMap | 2,115.6 ns | 20.92 ns | 19.57 ns | 0.9499 |     - |     - |   3,985 B |
|   AgileMapperMap | 1,018.3 ns | 11.28 ns | 10.56 ns | 0.8221 |     - |     - |   3,440 B |
|    TinyMapperMap | 3,985.2 ns | 15.35 ns | 12.82 ns | 0.8240 |     - |     - |   3,464 B |
|    AutoMapperMap | 2,282.1 ns | 14.77 ns | 13.82 ns | 0.7706 |     - |     - |   3,224 B |
|       MapsterMap |   723.7 ns | 10.58 ns |  9.38 ns | 0.7362 |     - |     - |   3,080 B |
| StaticsMapperMap |   678.2 ns |  9.83 ns |  8.71 ns | 0.7362 |     - |     - |   3,080 B |
|    FastMapperMap | 5,712.2 ns | 37.14 ns | 34.74 ns | 1.9608 |     - |     - |   8,216 B |
| ValueInjecterMap | 4,774.0 ns | 46.09 ns | 43.11 ns | 0.1984 |     - |     - |     840 B |
|    SafeMapperMap | 1,199.8 ns |  5.87 ns |  5.21 ns | 0.9174 |     - |     - |   3,840 B |


### More benchmark results
https://github.com/florin-rotaru/net-mappers-benchmarks/tree/master/net-mappers-benchmarks/BenchmarksResults/2021.06.14

### Benchmark results summary
|Library             |Passed                  |Failed                  
|--------------------|------------------------|------------------------
|ExpressMapper       |17                      |0                       
|AgileMapper         |17                      |0                       
|AutoMapper          |17                      |0                       
|Mapster             |17                      |0                       
|StaticsMapper       |17                      |0                       
|HigLaboObjectMapper |14                      |3                       
|FastMapper          |2                       |15                      
|ValueInjecter       |8                       |9                       
|PowerMapper         |12                      |5                       
|SafeMapper          |9                       |8                       
   

- passed: no exception thrown nor differences between source and destination members
- failed: exception thrown or differences found between source and destination members
  - [exceptions](https://github.com/florin-rotaru/net-mappers-benchmarks/tree/master/net-mappers-benchmarks/BenchmarksResults/2021.06.14/Failed.Exceptions.md)
  - [diffs](https://github.com/florin-rotaru/net-mappers-benchmarks/tree/master/net-mappers-benchmarks/BenchmarksResults/2021.06.14/Failed.Diffs.md)
