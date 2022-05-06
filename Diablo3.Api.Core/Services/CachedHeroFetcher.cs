using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;

namespace Diablo3.Api.Core.Services
{
    internal class CachedHeroFetcher : IHeroFetcher
    {
        private readonly IHeroFetcher heroFetcher;
        private readonly ICache<int, Hero> cache;


        public CachedHeroFetcher(IHeroFetcher heroFetcher, ICache<int, Hero> cache)
        {
            this.heroFetcher = heroFetcher ?? throw new ArgumentNullException(nameof(heroFetcher));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public Hero Get(int id, string battleTag)
        {
            var cachedData = cache.Get(id);
            if (cachedData is not null)
                return cachedData;

            var hero = heroFetcher.Get(id, battleTag);
            cache.SetAsync(id, hero).RunSynchronously();

            return hero;
        }

        public async Task<Hero> GetAsync(int id, string battleTag)
        {
            var cachedData = await cache.GetAsync(id);
            if (cachedData is not null)
                return cachedData;

            var hero = await heroFetcher.GetAsync(id, battleTag);
            await cache.SetAsync(id, hero);

            return hero;
        }
    }
}