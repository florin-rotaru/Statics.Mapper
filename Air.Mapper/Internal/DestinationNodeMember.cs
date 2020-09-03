using System.Reflection.Emit;

namespace Air.Mapper.Internal
{
    internal class DestinationNodeMember
    {
        public Reflection.MemberInfo Info { get; set; }
        public SourceNode SourceNode { get; set; }
        public Reflection.MemberInfo SourceNodeMember { get; set; }
        public bool IsCollection { get; set; }
        public LocalBuilder Local { get; set; }
        public bool Map { get; set; }
        public Status Status { get; set; }

        public DestinationNodeMember(Reflection.MemberInfo memberInfo)
        {
            Info = memberInfo;
        }
    }
}
