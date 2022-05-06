namespace Diablo3.Api.Core.Models.Cache
{
    public class Cache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly Dictionary<TKey, (TValue Data, DateTime CacheExpiration)> data;
        private readonly CacheConfiguration cacheConfiguration;
    
        public Cache(CacheConfiguration cacheConfiguration)
        {
            this.cacheConfiguration = cacheConfiguration ?? throw new ArgumentNullException(nameof(cacheConfiguration));
            data = new Dictionary<TKey, (TValue Data, DateTime CacheExpiration)>();
        }
    
        public TValue Get(TKey key)
        {
            if (data.ContainsKey(key) && DateTime.UtcNow < data[key].CacheExpiration) 
                return data[key].Data;

            return default;
        }

        public async Task<TValue> GetAsync(TKey key)
        {
            if (data.ContainsKey(key) && DateTime.UtcNow < data[key].CacheExpiration) 
                return data[key].Data;

            return default;
        }

        public Task SetAsync(TKey key, TValue value)
        {
            data[key] = (value, DateTime.UtcNow + cacheConfiguration.CacheTtl);;
            return Task.FromResult(false);
        }
    }
}