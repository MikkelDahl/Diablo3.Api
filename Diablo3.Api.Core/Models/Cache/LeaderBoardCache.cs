using Diablo3.Api.Core.Extensions;
using Diablo3.Api.Core.Services;

namespace Diablo3.Api.Core.Models.Cache
{
    internal class LeaderBoardCache : CacheBase<CacheKey, LeaderBoard>, ILeaderBoardFetcher
    {
        public LeaderBoardCache(Func<Task<LeaderBoard>> dataFetcher, CacheConfiguration cacheConfiguration) : base(dataFetcher, cacheConfiguration)
        { 
        }

        public LeaderBoard Get(HeroClass heroClass) => Task.Run(() => GetAsync(heroClass)).GetAwaiter().GetResult();
        public LeaderBoard Get(ItemSet set) => Task.Run(() => GetAsync(set)).GetAwaiter().GetResult();

        public Task<LeaderBoard> GetAsync(HeroClass heroClass)
        {
            var key = new CacheKey(heroClass, ItemSet.All);
            return GetAsync(key);
        }

        public Task<LeaderBoard> GetAsync(ItemSet itemSet)
        {
            var heroClass = itemSet.ToHeroClass();
            var key = new CacheKey(heroClass, itemSet);
            return GetAsync(key);
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