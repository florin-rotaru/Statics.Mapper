using System;

namespace Statics.Mapper.Internal.Options
{
    internal class MapAsOption(
        string destinationMemberName,
        Type implementation)
    {
        public string DestinationMemberName { get; } = destinationMemberName;
        public Type Implementation { get; } = implementation;

        public MapAsOption(IMapperOptionArguments option) : this(
            (string)option.Arguments[0],
            (Type)option.Arguments[1])
        { }

        public IMapperOptionArguments AsMapOptionArguments() =>
            new MapperOptionArguments(
                nameof(MapAsOption),
                [
                    DestinationMemberName,
                    Implementation
                ]);
    }
}
