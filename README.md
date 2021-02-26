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

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1082 (1909/November2018Update/19H2)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.402
  [Host] : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|           Method |       Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |-----------:|----------:|----------:|-------:|------:|------:|----------:|
| ExpressMapperMap | 2,263.7 ns |  45.18 ns |  57.14 ns | 0.9499 |     - |     - |    3985 B |
|   AgileMapperMap | 1,047.8 ns |   5.91 ns |   5.53 ns | 0.8221 |     - |     - |    3441 B |
|    TinyMapperMap | 3,977.6 ns |  13.73 ns |  12.85 ns | 0.8240 |     - |     - |    3465 B |
|    AutoMapperMap | 2,178.8 ns |  23.08 ns |  20.46 ns | 0.7706 |     - |     - |    3225 B |
|       MapsterMap |   736.0 ns |   8.18 ns |   7.65 ns | 0.7362 |     - |     - |    3080 B |
|     AirMapperMap |   668.6 ns |   3.00 ns |   2.66 ns | 0.7362 |     - |     - |    3080 B |
|    FastMapperMap | 5,666.5 ns | 103.45 ns | 127.05 ns | 1.9608 |     - |     - |    8218 B |
| ValueInjecterMap | 4,784.0 ns |  17.14 ns |  15.19 ns | 0.1984 |     - |     - |     840 B |
|    SafeMapperMap | 1,276.1 ns |  16.25 ns |  13.57 ns | 0.9174 |     - |     - |    3840 B |

### More benchmark results
https://github.com/florin-rotaru/NetMappers.Benchmarks/tree/master/NetMappers.Benchmarks/BenchmarksResults/2021.02.25

### Benchmark results summary
|Library             |Passed                  |Failed                  
|--------------------|------------------------|------------------------
|ExpressMapper       |16                      |0                       
|AgileMapper         |16                      |0                       
|TinyMapper          |4                       |12                      
|AutoMapper          |16                      |0                       
|Mapster             |16                      |0                       
|AirMapper           |16                      |0                       
|HigLaboObjectMapper |13                      |3                       
|FastMapper          |1                       |15                      
|ValueInjecter       |8                       |8                       
|PowerMapper         |12                      |4                       
|SafeMapper          |8                       |8    

- passed: no exception thrown nor differences between source and destination members
- failed: exception thrown or differences found between source and destination members
  - [exceptions](https://github.com/florin-rotaru/NetMappers.Benchmarks/blob/master/NetMappers.Benchmarks/BenchmarksResults/2021.02.25/Failed.Exceptions.md)
  - [diffs](https://github.com/florin-rotaru/NetMappers.Benchmarks/blob/master/NetMappers.Benchmarks/BenchmarksResults/2021.02.25/Failed.Diffs.md)
