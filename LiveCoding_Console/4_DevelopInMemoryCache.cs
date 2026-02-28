using System.Collections.Concurrent;
using static System.Console;

namespace LiveCoding_Console
{
    // DevelopInMemoryCache
    internal class _4_DevelopInMemoryCache
    {
        public static void Test()
        {
            CustomInMemoryCache<int?>.Cache("key1", 1, 5);       // 5 seconds cached
            WriteLine($"Key1: {CustomInMemoryCache<int?>.GetByKey("KEY1")}");
            WriteLine($"Key2: {CustomInMemoryCache<bool?>.GetByKey("key2")} - This should not exist");


            WriteLine("Waiting 5 seconds to expire key 1");
            Thread.Sleep(5000);
            WriteLine($"After 5 seconds expiry - Key1: {CustomInMemoryCache<int?>.GetByKey("KEY1")}");
            WriteLine($"After 5 seconds expiry - Key1: {CustomInMemoryCache<int?>.GetByKey("KEY1")}");


            WriteLine("\nNext Test....\n");

            CustomInMemoryCache<Customer?>.Cache("key-john", new Customer("John", 20), 5);
            WriteLine($"key-john: {CustomInMemoryCache<Customer?>.GetByKey("key-john")}");
            CustomInMemoryCache<Customer?>.Cache("key-john", new Customer("Smith", 20), 5);
            WriteLine($"Replaced with smith - key-john: {CustomInMemoryCache<Customer?>.GetByKey("key-john")}");

            CustomInMemoryCache<Customer?>.EvictCache("key-john");
            WriteLine($"After evicted - key-john: {CustomInMemoryCache<Customer?>.GetByKey("key-john")} - This should not exist");
        }
    }

    public record Customer(string Name, int Age);


    #region origin_implementation

    // implemenation 
    public record CachedValue<T>(T ValueCached, DateTime CachedTime, int CacheTimeInSeconds = 60);


    public static class CustomInMemoryCache<T>
    {
        private static Dictionary<string, CachedValue<T>> MyCache { get; set; } = [];

        public static void Cache(string key, T valueToCache, int cacheTimeInSeconds = 60)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException($"{nameof(key)} cannot be null or empty");

            // it will either add or replace
            MyCache[key.ToLower()] = new CachedValue<T>(valueToCache, DateTime.UtcNow, cacheTimeInSeconds);
        }

        public static void EvictCache(string key)
        {
            key = key.ToLower();
            if (MyCache.TryGetValue(key, out CachedValue<T>? value))
            {
                if (value is not null)
                {
                    MyCache.Remove(key);
                }
            }
        }

     
        public static T? GetByKey(string key)
        {
            if (MyCache.TryGetValue(key, out CachedValue<T>? value))
            {
                if (DateTime.UtcNow.Subtract(value.CachedTime).TotalSeconds < value.CacheTimeInSeconds)
                    return value.ValueCached;
                else
                    MyCache.Remove(key);        // Too old - remove
            }

            return default(T);
        }
    }


    #endregion origin_implementation


    #region improved_implemenation

    public record CachValue<T>(T Value, DateTime CachedAt, int TtlSeconds);

    public static class InMemoryCache<T>
    {
        // •	ConcurrentDictionary → thread safe
        // •	StringComparer.OrdinalIgnoreCase → no need to lowercase keys
        private static readonly ConcurrentDictionary<string, CachValue<T>> _cache =
            new(StringComparer.OrdinalIgnoreCase);

        public static void Set(string key, T value, int ttlSeconds = 60)
        {
            _cache[key] = new CachValue<T>(value, DateTime.UtcNow, ttlSeconds);
        }

        public static T? Get(string key)
        {
            if (_cache.TryGetValue(key, out var entry))
            {
                var age = DateTime.UtcNow - entry.CachedAt;

                if (age.TotalSeconds < entry.TtlSeconds)
                    return entry.Value;

                _cache.TryRemove(key, out _);
            }

            return default;
        }

        public static void Remove(string key)
        {
            _cache.TryRemove(key, out _);
        }
    }
    #endregion improved_implemenation
}
