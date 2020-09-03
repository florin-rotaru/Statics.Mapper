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

        public bool UseArrayCopyTo { get; }

        public LocalBuilder SourceLocal { get; set; }
        public LocalBuilder DestinationLocal { get; set; }

        public LocalBuilder LoopIndex { get; set; }
        public LocalBuilder LoopLength { get; set; }

        public LocalBuilder SourceLocalEnumerator { get; set; }

        public LocalBuilder MapperMapMethodLocal { get; set; }


        public CollectionInfo(Type sourceType, Type destinationType)
        {
            SourceType = sourceType;
            DestinationType = destinationType;
            SourceArgument = Collections.GetIEnumerableArgument(SourceType);
            DestinationArgument = Collections.GetIEnumerableArgument(DestinationType);
            UseArrayCopyTo = sourceType.IsArray &&
                sourceType == destinationType &&
                (
                    SourceArgument.IsValueType ||
                    Reflection.TypeInfo.IsBuiltIn(SourceArgument)
                );
        }

        public CollectionInfo(DestinationNode destinationNode, DestinationNodeMember destinationNodeMember) :
            this(destinationNodeMember.SourceNodeMember.Type, destinationNodeMember.Info.Type)
        {
            DestinationNode = destinationNode;
            DestinationNodeMember = destinationNodeMember;
        }
    }
}
