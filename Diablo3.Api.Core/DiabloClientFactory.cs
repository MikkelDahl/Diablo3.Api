using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;
using Diablo3.Api.Core.Services;
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
            var leaderBoardFetchers = BuildLeaderBoardFetchers();
            var heroFetcher = BuildHeroFetcher();
            var currentSeason = seasonIformationFetcher.GetCurrentSeasonAsync().Result;
            var itemFetcher = new ItemFetcher(battleNetApiHttpClient);
            var itemCache = new ItemCache(itemFetcher, new Cache<string, ICollection<Item>>(new CacheConfiguration(CacheOptions.Default, 86400)));
            return new DiabloClient(leaderBoardFetchers, heroFetcher, configuration, battleNetApiHttpClient, logger, currentSeason, itemCache);
        }

        private IHeroFetcher BuildHeroFetcher()
        {
            var heroFetcher = new HeroFetcher(battleNetApiHttpClient); 
            var cache = new Cache<int, Hero>(configuration.CacheConfiguration);
            return configuration.CacheConfiguration.Options == CacheOptions.NoCache
                ? heroFetcher
                : new CachedHeroFetcher(heroFetcher, cache);
        }
        
        private ILeaderBoardService BuildLeaderBoardFetchers()
        {
            var currentSeason = seasonIformationFetcher.GetCurrentSeasonAsync().Result;
            var normalFetcher = new NormalLeaderBoardFetcher(battleNetApiHttpClient, currentSeason); 
            var hcFetcher = new HardcoreLeaderBoardFetcher(battleNetApiHttpClient, currentSeason); 
            var cache = new Cache<CacheKey, LeaderBoard>(configuration.CacheConfiguration);
            var hcCache = new Cache<CacheKey, LeaderBoard>(configuration.CacheConfiguration);
            
            return configuration.CacheConfiguration.Options == CacheOptions.NoCache
                ? new LeaderBoardService(normalFetcher, hcFetcher)
                : new LeaderBoardService(new CachedLeaderBoardFetcher(normalFetcher, cache),
                    new CachedLeaderBoardFetcher(hcFetcher, hcCache));
        }
    }
}