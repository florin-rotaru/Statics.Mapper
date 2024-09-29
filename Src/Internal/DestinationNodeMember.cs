using System.Reflection.Emit;

namespace Statics.Mapper.Internal
{
    internal class DestinationNodeMember(MapperTypeMemberInfo memberInfo)
    {
        public MapperTypeMemberInfo Info { get; set; } = memberInfo;
        public SourceNode? SourceNode { get; set; }
        public MapperTypeMemberInfo? SourceNodeMember { get; set; }
        public bool IsCollection { get; set; }
        public LocalBuilder? Local { get; set; }
        public bool Map { get; set; }
        public Status Status { get; set; }
    }
}
