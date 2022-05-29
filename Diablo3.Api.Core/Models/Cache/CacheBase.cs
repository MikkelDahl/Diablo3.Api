namespace Diablo3.Api.Core.Models.Cache
{
    internal abstract class CacheBase<TKey, TValue> where TKey : notnull
    {
        private readonly Dictionary<TKey, (TValue, DateTime)> cache;
        private readonly Func<Task<TValue>> dataFetcher;
        private readonly CacheConfiguration cacheConfiguration;

        protected CacheBase(Func<Task<TValue>> dataFetcher, CacheConfiguration cacheConfiguration)
        {
            this.dataFetcher = dataFetcher ?? throw new ArgumentNullException(nameof(dataFetcher));
            this.cacheConfiguration = cacheConfiguration ?? throw new ArgumentNullException(nameof(cacheConfiguration));
            cache = new Dictionary<TKey, (TValue, DateTime)>();
        }

        protected async Task<TValue> GetAsync(TKey key)
        {
            if (cache.TryGetValue(key, out (TValue Data, DateTime CacheExpiration) value))
                if (value.CacheExpiration > DateTime.UtcNow)
                    return value.Data;

            var cacheExpiration = DateTime.UtcNow + cacheConfiguration.CacheTtl;
            value = (await dataFetcher(), cacheExpiration);
      
            cache[key] = value;
            return value.Data;
        }
    }
}