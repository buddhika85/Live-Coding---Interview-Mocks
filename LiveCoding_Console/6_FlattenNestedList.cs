

namespace LiveCoding_Console
{
    internal class _6_FlattenNestedList
    {
        public static void Test()
        {
            IEnumerable<IEnumerable<int>> unflattned =
            [
                [1, 2],
                [3, 4]
            ];

            var flattened = FlattenList(unflattned);
            flattened?.DisplayAsString();
        }

        public static IEnumerable<T> FlattenList<T>(IEnumerable<IEnumerable<T>> unflattned) 
            => unflattned.SelectMany(x => x);
    }
}
