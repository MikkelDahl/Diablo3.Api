using Diablo3.Api.Core.Extensions;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;

namespace Diablo3.Api.Core.Services
{
    internal class CachedLeaderBoardFetcher : ILeaderBoardFetcher
    {
        private readonly ILeaderBoardFetcher leaderBoardFetcher;
        private readonly ICache<CacheKey, LeaderBoard> cache;

        public CachedLeaderBoardFetcher(ILeaderBoardFetcher leaderBoardFetcher, ICache<CacheKey, LeaderBoard> cache)
        {
            this.leaderBoardFetcher = leaderBoardFetcher ?? throw new ArgumentNullException(nameof(leaderBoardFetcher));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<LeaderBoard> GetAsync(HeroClass heroClass)
        {
            var cacheKey = new CacheKey(heroClass, ItemSet.All);
            var cachedData = await cache.GetAsync(cacheKey);
            if (cachedData is not null)
                return cachedData;

            var data = await leaderBoardFetcher.GetAsync(heroClass);
            await cache.SetAsync(cacheKey, data);

            return data;
        }

        public async Task<LeaderBoard> GetForItemSetAsync(ItemSet itemSet)
        {
            var heroClass = itemSet.ToHeroClass();
            var cacheKey = new CacheKey(heroClass, itemSet);
            var cachedData = await cache.GetAsync(cacheKey);
            Console.WriteLine($"Cached data: {cachedData?.Entries.First().LadderHero.BattleTag}");
            if (cachedData is not null)
                return cachedData;

            var data = await leaderBoardFetcher.GetForItemSetAsync(itemSet);
            await cache.SetAsync(cacheKey, data);

            return data;
        }
    }
}