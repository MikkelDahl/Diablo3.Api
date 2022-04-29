using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services;

internal class CachedHeroFetcher : IHeroFetcher
{
    private readonly IHeroFetcher heroFetcher;
    private readonly CacheConfiguration cacheConfiguration;
    private readonly Dictionary<int, (Hero Data, DateTime CacheExpiration)> cachedData;


    public CachedHeroFetcher(IHeroFetcher heroFetcher, CacheConfiguration cacheConfiguration)
    {
        this.heroFetcher = heroFetcher ?? throw new ArgumentNullException(nameof(heroFetcher));
        this.cacheConfiguration = cacheConfiguration ?? throw new ArgumentNullException(nameof(cacheConfiguration));
        cachedData = new Dictionary<int, (Hero Data, DateTime CacheExpiration)>();
    }

    public Hero Get(int id, string battleTag)
    {
        if (cachedData.ContainsKey(id) && DateTime.UtcNow < cachedData[id].CacheExpiration) 
            return cachedData[id].Data;

        var hero = heroFetcher.Get(id, battleTag);
        cachedData[id] = (hero, DateTime.UtcNow + cacheConfiguration.CacheTtl);

        return hero;
    }

    public async Task<Hero> GetAsync(int id, string battleTag)
    {
        if (cachedData.ContainsKey(id) && DateTime.UtcNow < cachedData[id].CacheExpiration) 
            return cachedData[id].Data;

        var hero = await heroFetcher.GetAsync(id, battleTag);
        cachedData[id] = (hero, DateTime.UtcNow + cacheConfiguration.CacheTtl);

        return hero;
    }
}