using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;
using Serilog;

namespace Diablo3.Api.Core.Services
{
    public class DataFetcherFactory
    {
        private readonly ISeasonIformationFetcher seasonIformationFetcher;
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        private readonly CacheConfiguration cacheConfiguration;
        private readonly bool hardcore;
        private readonly ILogger logger;

        public DataFetcherFactory(CacheConfiguration cacheConfiguration, IBattleNetApiHttpClient battleNetApiHttpClient, bool hardcore)
        {
            this.cacheConfiguration = cacheConfiguration;
            this.hardcore = hardcore;
            this.battleNetApiHttpClient = battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
            seasonIformationFetcher = new SeasonIformationFetcher(battleNetApiHttpClient);
            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public ILeaderBoardFetcher Build()
        {
        
            var actualFetcher = BuildLeaderBoardFetcher();
            var cache = new Cache<CacheKey, LeaderBoard>(cacheConfiguration);
            return cacheConfiguration.Options == CacheOptions.NoCache
                ? actualFetcher
                : new CachedLeaderBoardFetcher(actualFetcher, logger, cache);
        }

        private ILeaderBoardFetcher BuildLeaderBoardFetcher()
        {
            var currentSeason = seasonIformationFetcher.GetCurrentSeasonAsync().Result;
            return hardcore
                ? new HardcoreLeaderBoardFetcher(battleNetApiHttpClient, currentSeason)
                : new LeaderBoardFetcher(battleNetApiHttpClient, currentSeason);
        }
    }
}