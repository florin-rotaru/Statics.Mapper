# What is Air.Mapper?

Is a simple and fast open source mapping library which allows to map one type members into another.

# Features

- maps members based on conventions (public properties with the same names and same/derived/convertible types)
- compiles func / action delegates mappers

### Future work
  - More functions available

# Example of usage

```csharp
var account = Fixture.Create<Models.Account>();
Mapper.Map(account, out Models.AccountDto accountDto);
```

