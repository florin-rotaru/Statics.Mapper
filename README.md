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
  .MapAs(d => d.InterfaceMember, typeof(Implementation))
  .Map(s => s.SourceMember, d => d.DestinationMember));

var mapFunction = Mapper<TSource, IDestination>.CompileFunc(o => o
  .MapAs(d => d, typeof(Destination)));
```
or
```csharp
MapperConfig<TSource, TDestination>.SetOptions(o => o
  .Ignore(i => i.DestinationMember)
  .MapAs(d => d.InterfaceMember, typeof(Implementation))
  .Map(s => s.SourceMember, d => d.DestinationMember));

MapperConfig<TSource, IDestination>.SetOptions(o => o
  .MapAs(d => d, typeof(Destination)));
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

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1348 (21H1/May2021Update)
11th Gen Intel Core i5-1135G7 2.40GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.100
  [Host] : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |       Mean |    Error |   StdDev |  Gen 0 | Allocated |
|----------------- |-----------:|---------:|---------:|-------:|----------:|
| ExpressMapperMap | 1,313.3 ns |  5.86 ns |  4.58 ns | 0.9518 |   3,985 B |
|   AgileMapperMap |   682.0 ns |  2.87 ns |  2.40 ns | 0.8221 |   3,440 B |
|    TinyMapperMap | 2,428.9 ns |  9.61 ns |  8.99 ns | 0.8278 |   3,464 B |
|    AutoMapperMap | 1,323.2 ns |  6.13 ns |  5.12 ns | 0.7706 |   3,224 B |
|       MapsterMap |   550.4 ns |  4.23 ns |  3.96 ns | 0.7362 |   3,080 B |
| StaticsMapperMap |   508.6 ns |  5.15 ns |  4.82 ns | 0.7362 |   3,080 B |
|    FastMapperMap | 3,574.4 ns | 13.41 ns | 12.55 ns | 1.9646 |   8,216 B |
| ValueInjecterMap | 2,266.0 ns | 23.66 ns | 22.13 ns | 0.1373 |     584 B |


### More benchmark results
https://github.com/florin-rotaru/net-mappers-benchmarks/tree/master/net-mappers-benchmarks/BenchmarksResults/2021.11.21

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
   

- passed: no exception thrown nor differences between source and destination members
- failed: exception thrown or differences found between source and destination members
  - [exceptions](https://github.com/florin-rotaru/net-mappers-benchmarks/tree/master/net-mappers-benchmarks/BenchmarksResults/2021.11.21/Failed.Exceptions.md)
  - [diffs](https://github.com/florin-rotaru/net-mappers-benchmarks/tree/master/net-mappers-benchmarks/BenchmarksResults/2021.11.21/Failed.Diffs.md)
