using System;

namespace Statics.Mapper.Internal
{
    internal class MapOption
    {
        public string SourceMemberName { get; }
        public string DestinationMemberName { get; }
        public bool Expand { get; }
        public bool UseMapperConfig { get; }

        public MapOption(
            string sourceMemberName,
            string destinationMemberName,
            bool expand,
            bool useMapperConfig)
        {
            SourceMemberName = sourceMemberName;
            DestinationMemberName = destinationMemberName;
            Expand = expand;
            UseMapperConfig = useMapperConfig;
        }

        public MapOption(IMapOption option) : this(
            (string)option.Arguments[0],
            (string)option.Arguments[1],
            (bool)option.Arguments[2],
            (bool)option.Arguments[3])
        { }

        public IMapOption AsMapOption() =>
            new Option(
                nameof(MapOptions<Type, Type>.Map),
                new object[] {
                    SourceMemberName,
                    DestinationMemberName,
                    Expand,
                    UseMapperConfig
                });
    }
}
