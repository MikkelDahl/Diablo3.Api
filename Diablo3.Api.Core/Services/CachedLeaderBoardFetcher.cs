using System.Collections.Concurrent;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;
using Serilog;

namespace Diablo3.Api.Core.Services
{
    internal class CachedLeaderBoardFetcher : ILeaderBoardFetcher
    {
        private readonly LeaderBoardFetcher leaderBoardFetcher;
        private readonly CacheConfiguration cacheConfiguration;
        private readonly ILogger logger;
        private readonly ConcurrentDictionary<CacheKey, (LeaderBoard Data, DateTime CacheExpiration)> cachedData;

        public CachedLeaderBoardFetcher(LeaderBoardFetcher leaderBoardFetcher, ILogger logger, CacheConfiguration cacheConfiguration)
        {
            this.leaderBoardFetcher = leaderBoardFetcher ?? throw new ArgumentNullException(nameof(leaderBoardFetcher));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.cacheConfiguration = cacheConfiguration ?? throw new ArgumentNullException(nameof(cacheConfiguration));
            cachedData = new ConcurrentDictionary<CacheKey, (LeaderBoard Data, DateTime CacheExpiration)>();

            if (cacheConfiguration.Options == CacheOptions.Preload)
                InitializeCache().Wait();
        }

        public async Task<LeaderBoard> GetLeaderBoardAsync(HeroClass heroClass, bool hardcore)
        {
            var cacheKey = new CacheKey(heroClass, ItemSet.All, hardcore);
            if (cachedData.ContainsKey(cacheKey) && DateTime.UtcNow < cachedData[cacheKey].CacheExpiration) 
                return cachedData[cacheKey].Data;

            var data = await leaderBoardFetcher.GetLeaderBoardAsync(heroClass);
            cachedData[cacheKey] = (data, DateTime.UtcNow + cacheConfiguration.CacheTtl);

            return data;
        }

        public async Task<LeaderBoard> GetLeaderBoardForItemSetAsync(ItemSet itemSet, bool hardcore = false)
        {
            var heroClass = ItemSetConverter.GetConvertedHeroClass(itemSet);
            var cacheKey = new CacheKey(heroClass, itemSet, hardcore);
            if (cachedData.ContainsKey(cacheKey) && DateTime.UtcNow < cachedData[cacheKey].CacheExpiration) 
                return cachedData[cacheKey].Data;

            var data = await leaderBoardFetcher.GetLeaderBoardForItemSetAsync(itemSet, hardcore);
            cachedData[cacheKey] = (data, DateTime.UtcNow + cacheConfiguration.CacheTtl);

            return data;
        }

        public int GetCurrentSeason() => leaderBoardFetcher.GetCurrentSeason();

        private async Task InitializeCache()
        {
            logger.Information($"Cache initialization started");
            for (var i = 0; i < 7; i++)
            {
                var playerClass = (HeroClass)i;
                var allItemSets = ItemSetConverter.GetForClass(playerClass);
                foreach (var itemSet in allItemSets)
                {
                    logger.Information($"Caching for set: {itemSet}");
                    try
                    {
                        var normalDataForItemSet = await leaderBoardFetcher.GetLeaderBoardForItemSetAsync(itemSet);
                        var hcDataForItemSet = await leaderBoardFetcher.GetLeaderBoardForItemSetAsync(itemSet, true);
                        var normalDataForItemSetCacheKey = new CacheKey(playerClass, itemSet, false);
                        var hcDataForItemSetCacheKey = new CacheKey(playerClass,  itemSet, true);
                        cachedData[normalDataForItemSetCacheKey] = (normalDataForItemSet, DateTime.UtcNow + cacheConfiguration.CacheTtl);
                        cachedData[hcDataForItemSetCacheKey] = (hcDataForItemSet, DateTime.UtcNow + cacheConfiguration.CacheTtl);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + $" Skipping cache load for {playerClass}/{itemSet}.");
                    }
                }

                logger.Information($"Finished initializing cache for {playerClass}.");
            }
            
            logger.Information($"Cache initialization finished. Expiration: {DateTime.UtcNow + cacheConfiguration.CacheTtl} UTC");
        }
    }
}