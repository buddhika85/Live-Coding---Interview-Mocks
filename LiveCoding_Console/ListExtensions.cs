using static System.Console;

namespace LiveCoding_Console
{
    internal static class ListExtensions
    {
        public static void DisplayAsString<T>(this IEnumerable<T> list, char separator = ',') 
            => WriteLine($"{string.Join(separator, list)}");
    }
}
