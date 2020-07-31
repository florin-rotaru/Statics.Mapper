using System;
using System.Reflection.Emit;

namespace Air.Mapper.Internal
{
    internal class CollectionInfo
    {
        public DestinationNode DestinationNode { get; }
        public DestinationNodeMember DestinationNodeMember { get; }
        public Type SourceType { get; }
        public Type DestinationType { get; }

        public Type SourceArgument { get; }
        public Type DestinationArgument { get; }

        public LocalBuilder SourceLocal { get; set; }
        public LocalBuilder DestinationLocal { get; set; }

        public LocalBuilder LoopIndex { get; set; }
        public LocalBuilder LoopLength { get; set; }

        public LocalBuilder MapperMapMethodLocal { get; set; }

        public CollectionInfo(DestinationNode destinationNode, DestinationNodeMember destinationNodeMember)
        {
            DestinationNode = destinationNode;
            DestinationNodeMember = destinationNodeMember;
            SourceType = DestinationNodeMember.SourceNodeMember.Type;
            DestinationType = DestinationNodeMember.Info.Type;
            SourceArgument = Collections.GetIEnumerableArgument(SourceType);
            DestinationArgument = Collections.GetIEnumerableArgument(DestinationType);
        }
    }
}
