using System;

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

        public static decimal GetTokenAssetValue(this string asset)
        {
            return Convert.ToDecimal(asset.Split(' ')[0]);
        }

        public static string GetTokenAssetType(this string asset)
        {
            return asset.Split(' ')[1];
        }
    }
}
