using Microsoft.Extensions.Caching.Memory;

namespace Diablo3.Api.Core.Models.Cache;

internal class LeaderBoardCache : ICache<CacheKey, LeaderBoard>
{
    private readonly IMemoryCache cache;

    public LeaderBoardCache(IMemoryCache cache)
    {
        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<LeaderBoard> GetAsync(CacheKey key)
    {
        if (cache.TryGetValue(key, out LeaderBoard value))
            return value;

        value = await fetcher();
        cache.Set(key, value);
        return value;
    }

    public async Task SetAsync()
    {
        
    }
}