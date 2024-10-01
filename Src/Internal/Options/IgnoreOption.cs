using System.Collections.Generic;
using System.Linq;

namespace Statics.Mapper.Internal.Options
{
    internal class IgnoreOption(IEnumerable<string> destinationMemberNames)
    {
        public List<string> DestinationMemberNames { get; } = destinationMemberNames.ToList();

        public IgnoreOption(IMapperOptionArguments option) : this(option.Arguments.Select(s => (string)s).ToList()) { }

        public IMapperOptionArguments AsMapOptionArguments() =>
            new MapperOptionArguments(nameof(IgnoreOption), [.. DestinationMemberNames]);
    }
}
