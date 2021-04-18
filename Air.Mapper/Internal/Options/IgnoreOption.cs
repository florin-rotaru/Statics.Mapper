using System;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper.Internal
{
    internal class IgnoreOption
    {
        public List<string> DestinationMemberNames { get; }

        public IgnoreOption(IEnumerable<string> destinationMemberNames) =>
            DestinationMemberNames = destinationMemberNames.ToList();

        public IgnoreOption(IMapOption option) : this(option.Arguments.Select(s => (string)s).ToList()) { }

        public IMapOption AsMapOption() =>
            new Option(nameof(MapOptions<Type, Type>.Ignore), DestinationMemberNames.ToArray());
    }
}
