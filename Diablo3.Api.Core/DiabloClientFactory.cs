using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;
using Diablo3.Api.Core.Services;
using Diablo3.Api.Core.Services.AutoProperties;
using Serilog;

namespace Diablo3.Api.Core
{
    public class DiabloClientFactory
    {
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        private readonly ISeasonIformationFetcher seasonIformationFetcher;
        private readonly ClientConfiguration configuration;
        private readonly ILogger logger;

        public DiabloClientFactory(Region region, string clientId, string clientSecret, ClientConfiguration? configuration = null)
        {
            this.configuration = configuration ?? DefaultClientConfiguration.GetConfiguration();
            var credentials = new Credentials(clientId, clientSecret);
            battleNetApiHttpClient = new BattleNetApiHttpClient(credentials, region);
            seasonIformationFetcher = new SeasonIformationFetcher(battleNetApiHttpClient);
            
            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IClient Build()
        {
            var heroFetcher = BuildHeroFetcher();
            var characterService = new CharacterService(heroFetcher);
            var currentSeason = seasonIformationFetcher.GetCurrentSeasonAsync().Result;
            var itemFetcher = new ItemFetcher(battleNetApiHttpClient);
            var itemCache = new ItemCache(itemFetcher, new Cache<string, ICollection<Item>>(new CacheConfiguration(CacheOptions.Default, 86400)));
            return new DiabloClient(characterService, configuration, battleNetApiHttpClient, logger, currentSeason, itemCache);
        }

        private IHeroFetcher BuildHeroFetcher()
        {
            var heroFetcher = new HeroFetcher(battleNetApiHttpClient); 
            var cache = new Cache<int, Hero>(configuration.CacheConfiguration);
            return configuration.CacheConfiguration.Options == CacheOptions.NoCache
                ? heroFetcher
                : new CachedHeroFetcher(heroFetcher, cache);
        }
    }
}