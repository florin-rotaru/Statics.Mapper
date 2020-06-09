namespace Air.Mapper.Internal
{
    internal class IgnoreOption
    {
        public string DestinationMemberName { get; }

        public IgnoreOption(IMapOption option)
        {
            DestinationMemberName = (string)option.Arguments[0];
        }
    }
}
