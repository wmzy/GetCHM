namespace WMZY.Util
{
    // todo: 总是会调用，应移除
    public static class StringExtensions
    {
        public static string NullOrEmptyDefault(this string source, string @default)
        {
            return string.IsNullOrEmpty(source) ? @default : source;
        }

        public static string NullOrWhiteSpaceDefault(this string source, string @default)
        {
            return string.IsNullOrWhiteSpace(source) ? @default : source;
        }
    }
}
