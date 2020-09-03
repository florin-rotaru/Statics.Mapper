namespace Air.Mapper.Internal
{
    internal class MemberOption
    {
        public string Member { get; set; }
        public string Option { get; set; }
        
        public MemberOption() { }
        public MemberOption(string member, string option)
        {
            Member = member;
            Option = option;
        }
    }
}
