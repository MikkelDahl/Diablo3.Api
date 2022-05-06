using Diablo3.Api.Core.Extensions;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;
using Serilog;

namespace Diablo3.Api.Core.Services
{
    internal class CachedLeaderBoardFetcher : ILeaderBoardFetcher
    {
        private readonly ILeaderBoardFetcher leaderBoardFetcher;
        private readonly ICache<CacheKey, LeaderBoard> cache;

        public CachedLeaderBoardFetcher(ILeaderBoardFetcher leaderBoardFetcher, ILogger logger, ICache<CacheKey, LeaderBoard> cache)
        {
            this.leaderBoardFetcher = leaderBoardFetcher ?? throw new ArgumentNullException(nameof(leaderBoardFetcher));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<LeaderBoard> GetLeaderBoardAsync(HeroClass heroClass)
        {
            var cacheKey = new CacheKey(heroClass, ItemSet.All);
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
            var cacheKey = new CacheKey(heroClass, itemSet);
            var cachedData = await cache.GetAsync(cacheKey);
            if (cachedData is not null)
                return cachedData;

            var data = await leaderBoardFetcher.GetLeaderBoardForItemSetAsync(itemSet);
            await cache.SetAsync(cacheKey, data);

            return data;
        }

        // private async Task InitializeCache()
        // {
        //     logger.Information($"Cache initialization started");
        //     for (var i = 0; i < 7; i++)
        //     {
        //         var playerClass = (HeroClass)i;
        //         var allItemSets = ItemSetConverter.GetForClass(playerClass);
        //         foreach (var itemSet in allItemSets)
        //         {
        //             logger.Information($"Caching for set: {itemSet}");
        //             try
        //             {
        //                 var normalDataForItemSet = await leaderBoardFetcher.GetLeaderBoardForItemSetAsync(itemSet);
        //                 var hcDataForItemSet = await leaderBoardFetcher.GetLeaderBoardForItemSetAsync(itemSet);
        //                 var normalDataForItemSetCacheKey = new CacheKey(playerClass, itemSet);
        //                 var hcDataForItemSetCacheKey = new CacheKey(playerClass,  itemSet);
        //                 cachedData[normalDataForItemSetCacheKey] = (normalDataForItemSet, DateTime.UtcNow + cacheConfiguration.CacheTtl);
        //                 cachedData[hcDataForItemSetCacheKey] = (hcDataForItemSet, DateTime.UtcNow + cacheConfiguration.CacheTtl);
        //             }
        //             catch (Exception e)
        //             {
        //                 Console.WriteLine(e.Message + $" Skipping cache load for {playerClass}/{itemSet}.");
        //             }
        //         }
        //
        //         logger.Information($"Finished initializing cache for {playerClass}.");
        //     }
        //     
        //     logger.Information($"Cache initialization finished. Expiration: {DateTime.UtcNow + cacheConfiguration.CacheTtl} UTC");
        // }
    }
}