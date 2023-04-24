using System.Linq;

namespace MattPruett.MUDLite
{
    internal static class ClassExtensions
    {
        internal static string TrimLower(this string value)
        {
            return value.Trim().ToLower();
        }

        internal static string[] ToArray(this string value)
        {
            // Shouldn't ever be null, but it can't hurt to check it when we check if it's empty.
            return string.IsNullOrEmpty(value)
                ? new string[0]
                : value.Split();
        }

        internal static string[] ToArray(this string value, bool lowerCase)
        {

            return string.IsNullOrEmpty(value)
                ? new string[0]
                :  lowerCase

                    ? value
                        .Split()
                        .Select(str => str.TrimLower())
                        .ToArray()

                    : value.ToArray();
        }
    }
}
