using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Services;
using Serilog;

namespace Diablo3.Api.Core
{
    public class DiabloClientFactory
    {
        private IBattleNetApiHttpClient battleNetApiHttpClient;
        private Credentials credentials;
        private ILogger logger;

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

        private IFetcher BuildDataFetcher(ClientConfiguration configuration) =>
            configuration.CacheConfiguration.Options == CacheOptions.NoCache
                ? new DataFetcher(battleNetApiHttpClient)
                : new CachedDataFetcher(new DataFetcher(battleNetApiHttpClient), logger, configuration.CacheConfiguration);
    }
}