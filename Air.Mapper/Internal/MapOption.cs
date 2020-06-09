namespace Air.Mapper.Internal
{
    internal class MapOption
    {
        public string SourceMemberName { get; }
        public string DestinationMemberName { get; }
        public bool Expand { get; }

        public MapOption(IMapOption option)
        {
            SourceMemberName = (string)option.Arguments[0];
            DestinationMemberName = (string)option.Arguments[1];
            Expand = (bool)option.Arguments[2];
        }
    }
}
