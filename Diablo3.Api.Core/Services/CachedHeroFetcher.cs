using Diablo3.Api.Core.Models;
using Serilog;

namespace Diablo3.Api.Core.Services;

public class CachedHeroFetcher : IHeroFetcher
{
    private readonly IHeroFetcher heroFetcher;
    private readonly CacheConfiguration cacheConfiguration;
    private readonly ILogger logger;
    private readonly Dictionary<int, (Hero Data, DateTime CacheExpiration)> cachedData;


    public CachedHeroFetcher(IHeroFetcher heroFetcher, ILogger logger, CacheConfiguration cacheConfiguration)
    {
        this.heroFetcher = heroFetcher ?? throw new ArgumentNullException(nameof(heroFetcher));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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