using System.Collections.Concurrent;
using Diablo3.Api.Core.Models;
using Serilog;

namespace Diablo3.Api.Core.Services
{
    internal class CachedDataFetcher : IFetcher
    {
        private readonly DataFetcher dataFetcher;
        private readonly CacheConfiguration cacheConfiguration;
        private readonly ILogger logger;
        private readonly ConcurrentDictionary<CacheKey, (LeaderBoard Data, DateTime CacheExpiration)> cachedData;

        public CachedDataFetcher(DataFetcher dataFetcher, ILogger logger, CacheConfiguration cacheConfiguration)
        {
            this.dataFetcher = dataFetcher ?? throw new ArgumentNullException(nameof(dataFetcher));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.cacheConfiguration = cacheConfiguration;
            cachedData = new ConcurrentDictionary<CacheKey, (LeaderBoard Data, DateTime CacheExpiration)>();

            if (cacheConfiguration.Options == CacheOptions.Preload)
                Task.WaitAny(InitializeCache());
        }

        public async Task<LeaderBoard> GetLeaderBoardAsync(PlayerClass playerClass, bool hardcore)
        {
            var cacheKey = new CacheKey(playerClass, hardcore);
            if (cachedData.ContainsKey(cacheKey) && DateTime.UtcNow < cachedData[cacheKey].CacheExpiration) 
                return cachedData[cacheKey].Data;

            var data = await dataFetcher.GetLeaderBoardAsync(playerClass);
            cachedData[cacheKey] = (data, DateTime.UtcNow + cacheConfiguration.CacheTtl);

            return data;
        }

        private async Task InitializeCache()
        {
            logger.Information($"Cache initialization started");
            for (int i = 0; i < 6; i++)
            {
                var playerClass = (PlayerClass)i;
                var normalData = await dataFetcher.GetLeaderBoardAsync(playerClass);
                var hcData = await dataFetcher.GetLeaderBoardAsync(playerClass, true);
                
                var normalCacheKey = new CacheKey(playerClass, false);
                var hcCacheKey = new CacheKey(playerClass, true);
                cachedData[normalCacheKey] = (normalData, DateTime.UtcNow + cacheConfiguration.CacheTtl);
                cachedData[hcCacheKey] = (hcData, DateTime.UtcNow + cacheConfiguration.CacheTtl);
                logger.Information($"Loaded {playerClass} into cache.");
            }
            
            logger.Information($"Cache initialization finished. Expiration: {DateTime.UtcNow + cacheConfiguration.CacheTtl} UTC");
        }
    }
}