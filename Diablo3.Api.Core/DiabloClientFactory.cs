using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Services;
using Serilog;

namespace Diablo3.Api.Core
{
    public class DiabloClientFactory
    {
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        private readonly ISeasonIformationFetcher seasonIformationFetcher;
        private readonly Credentials credentials;
        private readonly ILogger logger;

        public DiabloClientFactory(Region region, string clientId, string clientSecret)
        {
            credentials = new Credentials(clientId, clientSecret);
            battleNetApiHttpClient = new BattleNetApiHttpClient(credentials, region);
            seasonIformationFetcher = new SeasonIformationFetcher(battleNetApiHttpClient);
            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IClient Build(ClientConfiguration configuration)
        {
            var leaderBoardFetcher = BuildLeaderBoardFetcher(configuration);
            var heroFetcher = BuildHeroFetcher(configuration);
            return new DiabloClient(leaderBoardFetcher, heroFetcher);
        }

        private ILeaderBoardFetcher BuildLeaderBoardFetcher(ClientConfiguration configuration)
        {
            var currentSeason = seasonIformationFetcher.GetCurrentSeasonAsync().Result;
            var leaderBoardFetcher = new LeaderBoardFetcher(battleNetApiHttpClient, currentSeason);
            return configuration.CacheConfiguration.Options == CacheOptions.NoCache
                ? leaderBoardFetcher
                : new CachedLeaderBoardFetcher(leaderBoardFetcher, logger,
                    configuration.CacheConfiguration);
        }
        
        private IHeroFetcher BuildHeroFetcher(ClientConfiguration configuration)
        {
            var heroFetcher = new HeroFetcher(battleNetApiHttpClient); 
            return configuration.CacheConfiguration.Options == CacheOptions.NoCache
                ? heroFetcher
                : new CachedHeroFetcher(heroFetcher, configuration.CacheConfiguration);
        }
    }
}