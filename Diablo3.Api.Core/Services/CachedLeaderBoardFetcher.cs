using System.Collections.Concurrent;
using Diablo3.Api.Core.Models;
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
            this.cacheConfiguration = cacheConfiguration;
            cachedData = new ConcurrentDictionary<CacheKey, (LeaderBoard Data, DateTime CacheExpiration)>();

            if (cacheConfiguration.Options == CacheOptions.Preload)
                Task.WaitAny(InitializeCache());
        }

        public async Task<LeaderBoard> GetLeaderBoardAsync(HeroClass heroClass, bool hardcore)
        {
            var cacheKey = new CacheKey(heroClass, hardcore);
            if (cachedData.ContainsKey(cacheKey) && DateTime.UtcNow < cachedData[cacheKey].CacheExpiration) 
                return cachedData[cacheKey].Data;

            var data = await leaderBoardFetcher.GetLeaderBoardAsync(heroClass);
            cachedData[cacheKey] = (data, DateTime.UtcNow + cacheConfiguration.CacheTtl);

            return data;
        }

        public async Task<int> GetCurrentSeasonAsync() => await leaderBoardFetcher.GetCurrentSeasonAsync();

        private async Task InitializeCache()
        {
            logger.Information($"Cache initialization started");
            for (int i = 0; i < 6; i++)
            {
                var playerClass = (HeroClass)i;
                var normalData = await leaderBoardFetcher.GetLeaderBoardAsync(playerClass);
                var hcData = await leaderBoardFetcher.GetLeaderBoardAsync(playerClass, true);
                
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