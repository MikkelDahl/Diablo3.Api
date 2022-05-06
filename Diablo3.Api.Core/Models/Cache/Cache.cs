namespace Diablo3.Api.Core.Models.Cache
{
    public class Cache<TKey, TValue> : ICache<TKey, TValue> where TKey : notnull
    {
        private readonly Dictionary<TKey, (TValue Data, DateTime CacheExpiration)> cachedData;
        private readonly CacheConfiguration cacheConfiguration;
    
        public Cache(CacheConfiguration cacheConfiguration)
        {
            this.cacheConfiguration = cacheConfiguration ?? throw new ArgumentNullException(nameof(cacheConfiguration));
            cachedData = new Dictionary<TKey, (TValue Data, DateTime CacheExpiration)>();
        }
    
        public TValue Get(TKey key)
        {
            if (cachedData.ContainsKey(key) && DateTime.UtcNow < cachedData[key].CacheExpiration) 
                return cachedData[key].Data;

            return default;
        }

        public async Task<TValue> GetAsync(TKey key)
        {
            if (cachedData.ContainsKey(key) && DateTime.UtcNow < cachedData[key].CacheExpiration) 
                return cachedData[key].Data;

            return default;
        }

        public Task SetAsync(TKey key, TValue value)
        {
            cachedData[key] = (value, DateTime.UtcNow + cacheConfiguration.CacheTtl);;
            return Task.FromResult(false);
        }
    }
}