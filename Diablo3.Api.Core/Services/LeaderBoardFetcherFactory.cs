using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;

namespace Diablo3.Api.Core.Services
{
    public class LeaderBoardFetcherFactory
    {
        private readonly ISeasonIformationFetcher seasonIformationFetcher;
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        private readonly CacheConfiguration cacheConfiguration;
        public LeaderBoardFetcherFactory(CacheConfiguration cacheConfiguration, IBattleNetApiHttpClient battleNetApiHttpClient)
        {
            this.cacheConfiguration = cacheConfiguration;
            this.battleNetApiHttpClient = battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
            seasonIformationFetcher = new SeasonIformationFetcher(battleNetApiHttpClient);
        }

        public ILeaderBoardFetcher Build()
        {
            var actualFetcher = BuildLeaderBoardFetcher(false);
            var cache = new Cache<CacheKey, LeaderBoard>(cacheConfiguration);
            return cacheConfiguration.Options == CacheOptions.NoCache
                ? actualFetcher
                : new CachedLeaderBoardFetcher(actualFetcher, cache);
        }
        
        public ILeaderBoardFetcher BuildHardcore()
        {
            var actualFetcher = BuildLeaderBoardFetcher(true);
            var cache = new Cache<CacheKey, LeaderBoard>(cacheConfiguration);
            return cacheConfiguration.Options == CacheOptions.NoCache
                ? actualFetcher
                : new CachedLeaderBoardFetcher(actualFetcher, cache);
        }

        private ILeaderBoardFetcher BuildLeaderBoardFetcher(bool hardcore)
        {
            var currentSeason = seasonIformationFetcher.GetCurrentSeasonAsync().Result;
            return hardcore
                ? new HardcoreLeaderBoardFetcher(battleNetApiHttpClient, currentSeason)
                : new NormalLeaderBoardFetcher(battleNetApiHttpClient, currentSeason);
        }
    }
}