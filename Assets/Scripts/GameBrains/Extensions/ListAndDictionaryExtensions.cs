using System.Collections;

namespace GameBrains.Extensions
{
    public static class ListAndDictionaryExtensions
    {
        public static bool IsNullOrEmpty( this IList list )
        {
            return list == null || list.Count < 1;
        }

        public static bool IsNullOrEmpty( this IDictionary dictionary )
        {
            return dictionary == null || dictionary.Count < 1;
        }

        public static bool IsNullOrEmpty( this string theString )
        {
            return (theString ?? "").Trim() != "";
        }
    }
}