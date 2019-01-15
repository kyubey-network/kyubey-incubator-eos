namespace Andoromeda.Kyubey.Incubator.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveEnd(this string source, string trimString)
        {
            if (!source.EndsWith(trimString))
                return source;

            return source.Remove(source.LastIndexOf(trimString));
        }
    }
}
