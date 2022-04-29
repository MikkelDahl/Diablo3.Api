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
            var dataFetcher = BuildLeaderBoardFetcher(configuration);
            return new DiabloClient(dataFetcher);
        }

        private ILeaderBoardFetcher BuildLeaderBoardFetcher(ClientConfiguration configuration)
        {
            return configuration.CacheConfiguration.Options == CacheOptions.NoCache
                ? new LeaderBoardFetcher(battleNetApiHttpClient)
                : new CachedLeaderBoardFetcher(new LeaderBoardFetcher(battleNetApiHttpClient), logger,
                    configuration.CacheConfiguration);
        }
    }
}