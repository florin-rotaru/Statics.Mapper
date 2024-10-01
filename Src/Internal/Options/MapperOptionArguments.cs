namespace Statics.Mapper.Internal.Options
{
    internal class MapperOptionArguments(string name, object?[] arguments) : IMapperOptionArguments
    {
        public string Name { get; } = name;

        public object?[] Arguments { get; } = arguments;
    }
}
