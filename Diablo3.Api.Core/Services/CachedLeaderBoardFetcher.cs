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

        public async Task<LeaderBoard> GetLeaderBoardAsync(HeroClass heroClass)
        {
            var hardcore = leaderBoardFetcher.GetType() == typeof(HardcoreLeaderBoardFetcher);
            var cacheKey = new CacheKey(heroClass, ItemSet.All, hardcore);
            var cachedData = await cache.GetAsync(cacheKey);
            if (cachedData is not null)
                return cachedData;

            var data = await leaderBoardFetcher.GetLeaderBoardAsync(heroClass);
            await cache.SetAsync(cacheKey, data);

            return data;
        }

        public async Task<LeaderBoard> GetLeaderBoardForItemSetAsync(ItemSet itemSet)
        {
            var heroClass = itemSet.ToHeroClass();
            var hardcore = leaderBoardFetcher.GetType() == typeof(HardcoreLeaderBoardFetcher);
            var cacheKey = new CacheKey(heroClass, itemSet, hardcore);
            var cachedData = await cache.GetAsync(cacheKey);
            if (cachedData is not null)
                return cachedData;

            var data = await leaderBoardFetcher.GetLeaderBoardForItemSetAsync(itemSet);
            await cache.SetAsync(cacheKey, data);

            return data;
        }
    }
}