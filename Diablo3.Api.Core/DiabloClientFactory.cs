using System.Diagnostics;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;
using Diablo3.Api.Core.Services;
using Serilog;

namespace Diablo3.Api.Core
{
    public class DiabloClientFactory
    {
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        private readonly ClientConfiguration configuration;
        private readonly int currentSeason;
        private readonly ILogger logger;

        public DiabloClientFactory(Region region, string clientId, string clientSecret, ClientConfiguration? configuration = null)
        {
            this.configuration = configuration ?? DefaultClientConfiguration.GetConfiguration();
            var credentials = new Credentials(clientId, clientSecret);
            battleNetApiHttpClient = new BattleNetApiHttpClient(credentials, region);
            var seasonIformationFetcher = new SeasonIformationFetcher(battleNetApiHttpClient);
            currentSeason = seasonIformationFetcher.GetCurrentSeasonAsync().GetAwaiter().GetResult();
            
            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IClient Build()
        {
            var leaderBoardFetchers = Task.Run(BuildLeaderBoardFetchers).GetAwaiter().GetResult();
            var heroFetcher = BuildHeroFetcher();
            var itemFetcher = new ItemFetcher(battleNetApiHttpClient);
            var itemCache = new ItemCache(itemFetcher, new Cache<string, ICollection<Item>>(new CacheConfiguration(CacheOptions.Default, 86400)));
            return new DiabloClient(leaderBoardFetchers, heroFetcher, itemCache);
        }

        private IHeroFetcher BuildHeroFetcher()
        {
            var heroFetcher = new HeroFetcher(battleNetApiHttpClient); 
            var cache = new Cache<int, Hero>(configuration.CacheConfiguration);
            return configuration.CacheConfiguration.Options == CacheOptions.NoCache
                ? heroFetcher
                : new CachedHeroFetcher(heroFetcher, cache);
        }
        
        private async Task<ILeaderBoardService> BuildLeaderBoardFetchers()
        {
            var normalFetcher = new NormalLeaderBoardFetcher(battleNetApiHttpClient, currentSeason); 
            var hcFetcher = new HardcoreLeaderBoardFetcher(battleNetApiHttpClient, currentSeason);
            var (normalCache, hcCache) = configuration.CacheConfiguration.Options == CacheOptions.Preload
                ? await GetWarmedUpCaches()
                : await GetColdCaches();

            return configuration.CacheConfiguration.Options == CacheOptions.NoCache
                ? new LeaderBoardService(normalFetcher, hcFetcher)
                : new LeaderBoardService(new CachedLeaderBoardFetcher(normalFetcher, normalCache),
                    new CachedLeaderBoardFetcher(hcFetcher, hcCache));
        }

        private async Task<(Cache<CacheKey, LeaderBoard> Normal, Cache<CacheKey, LeaderBoard> HC)> GetColdCaches() => 
            (new Cache<CacheKey, LeaderBoard>(configuration.CacheConfiguration), new Cache<CacheKey, LeaderBoard>(configuration.CacheConfiguration));

        private async Task<(Cache<CacheKey, LeaderBoard> Normal, Cache<CacheKey, LeaderBoard> HC)> GetWarmedUpCaches()
        {
            var normalCache = new Cache<CacheKey, LeaderBoard>(configuration.CacheConfiguration);
            var hcCache = new Cache<CacheKey, LeaderBoard>(configuration.CacheConfiguration);

            var normalFetcher = new NormalLeaderBoardFetcher(battleNetApiHttpClient, currentSeason); 
            var hcFetcher = new HardcoreLeaderBoardFetcher(battleNetApiHttpClient, currentSeason);

            var watch = new Stopwatch();
            watch.Start();
            for (var i = 0; i < 7; i++)
            {
                var heroClass = (HeroClass)i;
                logger.Information($"Caching for all {heroClass} sets");
                var normalTask = normalFetcher.GetAsync(heroClass);
                var hcTask = hcFetcher.GetAsync(heroClass);
                var fetchingTasks = new List<Task<LeaderBoard>> { normalTask, hcTask };
                await Task.WhenAll(fetchingTasks);

                await normalCache.SetAsync(new CacheKey(heroClass, ItemSet.All), normalTask.Result);
                await hcCache.SetAsync(new CacheKey(heroClass, ItemSet.All), hcTask.Result);
                
                var allItemSets = ItemSetConverter.GetForClass(heroClass);
                Parallel.ForEach(allItemSets, async itemSet =>
                {
                    logger.Information($"Caching for set: {itemSet}");
                    try
                    {
                        var normalDataForItemSet = await normalFetcher.GetAsync(itemSet);
                        var hcDataForItemSet = await hcFetcher.GetAsync(itemSet);
                        await normalCache.SetAsync(new CacheKey(heroClass, itemSet), normalDataForItemSet);
                        await hcCache.SetAsync(new CacheKey(heroClass, itemSet), hcDataForItemSet);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + $" Skipping cache load for {heroClass}/{itemSet}.");
                    }
                });
                
                watch.Stop();
                logger.Information($"Finished initializing cache for {heroClass} in {watch.ElapsedMilliseconds / 1000}s");
            }

            return (normalCache, hcCache);
        }
    }
}