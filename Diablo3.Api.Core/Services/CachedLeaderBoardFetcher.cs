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

        public LeaderBoard Get(HeroClass heroClass) => Task.Run(() => GetAsync(heroClass)).GetAwaiter().GetResult();
        public LeaderBoard Get(ItemSet set) => Task.Run(() => GetAsync(set)).GetAwaiter().GetResult();

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

        public async Task<LeaderBoard> GetAsync(ItemSet itemSet)
        {
            var heroClass = itemSet.ToHeroClass();
            var cacheKey = new CacheKey(heroClass, itemSet);
            var cachedData = await cache.GetAsync(cacheKey);
            if (cachedData is not null)
                return cachedData;

            var data = await leaderBoardFetcher.GetAsync(itemSet);
            await cache.SetAsync(cacheKey, data);

            return data;
        }

        public async Task<ICollection<LeaderBoard>> GetAllAsync()
        {
            var dataFetchingTasks = new List<Task<LeaderBoard>>()
            {
                GetAsync(HeroClass.Barbarian),
                GetAsync(HeroClass.Crusader),
                GetAsync(HeroClass.DemonHunter),
                GetAsync(HeroClass.Monk),
                GetAsync(HeroClass.Necromancer),
                GetAsync(HeroClass.WitchDoctor),
                GetAsync(HeroClass.Wizard)
            };

            await Task.WhenAll(dataFetchingTasks);
            return dataFetchingTasks.Select(t => t.Result).ToList();
        }
    }
}