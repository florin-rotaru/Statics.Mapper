using System;

namespace Air.Mapper.Internal
{
    internal class IgnoreOption
    {
        public string DestinationMemberName { get; }

        public IgnoreOption(string destinationMemberName) =>
            DestinationMemberName = destinationMemberName;

        public IgnoreOption(IMapOption option) : this((string)option.Arguments[0]) { }

        public IMapOption AsMapOption() =>
            new Option(nameof(MapOptions<Type, Type>.Ignore), new object[] { DestinationMemberName });
    }
}
