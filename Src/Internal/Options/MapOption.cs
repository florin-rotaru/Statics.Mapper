namespace Statics.Mapper.Internal.Options
{
    internal class MapOption(
        string sourceMemberName,
        string destinationMemberName,
        bool expand,
        bool useMapperConfig)
    {
        public string SourceMemberName { get; } = sourceMemberName;
        public string DestinationMemberName { get; } = destinationMemberName;
        public bool Expand { get; } = expand;
        public bool UseMapperConfig { get; } = useMapperConfig;

        public MapOption(IMapperOptionArguments option) : this(
            (string)option.Arguments[0],
            (string)option.Arguments[1],
            (bool)option.Arguments[2],
            (bool)option.Arguments[3])
        { }

        public IMapperOptionArguments AsMapOptionArguments() =>
            new MapperOptionArguments(
                nameof(MapOption),
                [
                    SourceMemberName,
                    DestinationMemberName,
                    Expand,
                    UseMapperConfig
                ]);
    }
}
