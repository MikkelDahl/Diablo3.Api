using Diablo3.Api.Core.Extensions;
using Diablo3.Api.Core.Services;

namespace Diablo3.Api.Core.Models.Cache;

internal class LeaderBoardCache : CacheBase<CacheKey, LeaderBoard>, ILeaderBoardFetcher
{
    public LeaderBoardCache(Func<Task<LeaderBoard>> dataFetcher, CacheConfiguration cacheConfiguration) : base(dataFetcher, cacheConfiguration)
    { 
    }


    public Task<LeaderBoard> GetLeaderBoardAsync(HeroClass heroClass)
    {
        var key = new CacheKey(heroClass, ItemSet.All);
        return GetAsync(key);
    }

    public Task<LeaderBoard> GetLeaderBoardForItemSetAsync(ItemSet itemSet)
    {
        var heroClass = itemSet.ToHeroClass();
        var key = new CacheKey(heroClass, itemSet);
        return GetAsync(key);
    }
}