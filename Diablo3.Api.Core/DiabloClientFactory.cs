using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Services;
using Serilog;

namespace Diablo3.Api.Core
{
    public class DiabloClientFactory
    {
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        private readonly Credentials credentials;
        private readonly ILogger logger;

        public DiabloClientFactory(Region region, string clientId, string clientSecret)
        {
            credentials = new Credentials(clientId, clientSecret);
            battleNetApiHttpClient = new BattleNetApiHttpClient(credentials, region);
            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IClient Build(ClientConfiguration configuration)
        {
            var dataFetcher = BuildDataFetcher(configuration);
            return new DiabloClient(dataFetcher);
        }

        private ILeaderBoardFetcher BuildDataFetcher(ClientConfiguration configuration)
        {
            var heroFetcher = new HeroFetcher(battleNetApiHttpClient);
            return configuration.CacheConfiguration.Options == CacheOptions.NoCache
                ? new LeaderBoardFetcher(battleNetApiHttpClient, heroFetcher)
                : new CachedLeaderBoardFetcher(new LeaderBoardFetcher(battleNetApiHttpClient, heroFetcher), logger,
                    configuration.CacheConfiguration);
        }
    }
}