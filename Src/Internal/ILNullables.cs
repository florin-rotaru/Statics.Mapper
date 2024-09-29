namespace Statics.Mapper.Internal
{
    internal static class ILNullables
    {
        public static T GetValueOrDefault<T>(T? source)
            where T : struct =>
            source.GetValueOrDefault();

        public static T? GetDefaultValue<T>()
            where T : struct =>
            default;
    }
}
