namespace Air.Mapper.Internal
{
    internal class Option : IMapOption
    {
        public string Name { get; }

        public object[] Arguments { get; }

        public Option(string name, object[] arguments)
        {
            Name = name;
            Arguments = arguments;
        }
    }
}
