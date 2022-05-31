using System.Diagnostics;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;
using Diablo3.Api.Core.Services;
using Diablo3.Api.Core.Services.Characters;
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
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IClient Build()
        {
            var leaderBoardFetchers = Task.Run(BuildLeaderBoardFetchersAsync).GetAwaiter().GetResult();
            var itemCache = Task.Run(BuildItemFetcherAsync).GetAwaiter().GetResult();
            var heroFetcher =  Task.Run(BuildHeroFetcher).GetAwaiter().GetResult();
            var accountFetcher =  Task.Run(BuildAccountFetcher).GetAwaiter().GetResult();
            
            return new DiabloClient(leaderBoardFetchers, heroFetcher, itemCache, accountFetcher);
        }
        
        public async Task<IClient> BuildAsync()
        {
            var leaderBoardFetchersTask = BuildLeaderBoardFetchersAsync();
            var itemCacheTask = BuildItemFetcherAsync();
            var accountFetcherTask = BuildAccountFetcher();
            var heroFetcherTask = BuildHeroFetcher();

            await Task.WhenAll(leaderBoardFetchersTask, itemCacheTask, accountFetcherTask, heroFetcherTask);
            
            return new DiabloClient(leaderBoardFetchersTask.Result, heroFetcherTask.Result, itemCacheTask.Result, accountFetcherTask.Result);
        }

        private Task<IHeroFetcher> BuildHeroFetcher()
        {
            var heroFetcher = new HeroFetcher(battleNetApiHttpClient); 
            var cache = new Cache<int, Hero>(configuration.CacheConfiguration);
            return configuration.CacheConfiguration.Options == CacheOptions.NoCache
                ? Task.FromResult<IHeroFetcher>(heroFetcher)
                : Task.FromResult<IHeroFetcher>(new CachedHeroFetcher(heroFetcher, cache));
        }
        
        private Task<IAccountFetcher> BuildAccountFetcher()
        {
            var accountFetcher = new AccountFetcher(battleNetApiHttpClient); 
            var cache = new Cache<string, Account>(configuration.CacheConfiguration);
            return configuration.CacheConfiguration.Options == CacheOptions.NoCache
                ? Task.FromResult<IAccountFetcher>(accountFetcher)
                : Task.FromResult<IAccountFetcher>(new CachedAccountFetcher(accountFetcher, cache));
        }

        private async Task<ILeaderBoardService> BuildLeaderBoardFetchersAsync()
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

        private Task<(Cache<CacheKey, LeaderBoard> Normal, Cache<CacheKey, LeaderBoard> HC)> GetColdCaches() => 
            Task.FromResult((new Cache<CacheKey, LeaderBoard>(configuration.CacheConfiguration), new Cache<CacheKey, LeaderBoard>(configuration.CacheConfiguration)));

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
                logger.Information($"Caching {heroClass} sets");
                var normalTask = normalFetcher.GetAsync(heroClass);
                var hcTask = hcFetcher.GetAsync(heroClass);
                var fetchingTasks = new List<Task<LeaderBoard>> { normalTask, hcTask };
                await Task.WhenAll(fetchingTasks);

                await normalCache.SetAsync(new CacheKey(heroClass, ItemSet.All), normalTask.Result);
                await hcCache.SetAsync(new CacheKey(heroClass, ItemSet.All), hcTask.Result);
                
                var allItemSets = ItemSetConverter.GetForClass(heroClass);
                Parallel.ForEach(allItemSets, async set =>
                {
                    try
                    {
                        var normalDataForItemSet = await normalFetcher.GetAsync(set);
                        var hcDataForItemSet = await hcFetcher.GetAsync(set);
                        await normalCache.SetAsync(new CacheKey(heroClass, set), normalDataForItemSet);
                        await hcCache.SetAsync(new CacheKey(heroClass, set), hcDataForItemSet);
                    }
                    catch (Exception e)
                    {
                        logger.Warning("{Message} - Skipping cache preload for {HeroClass}/{Set}", e.Message, heroClass, set);
                    }
                });
            }
            
            watch.Stop();
            logger.Verbose("Full cache initialized in {Elapsed}s", watch.ElapsedMilliseconds / 1000);

            return (normalCache, hcCache);
        }

        private async Task<IItemFetcher> BuildItemFetcherAsync()
        {
            var itemBaseCache = new Cache<string, ICollection<ItemBase>>(configuration.CacheConfiguration);
            var fetcher = new ItemFetcher(battleNetApiHttpClient, logger, itemBaseCache);

            if (configuration.CacheConfiguration.Options == CacheOptions.Preload)
                await fetcher.GetAsync("s");
            
            return configuration.CacheConfiguration.Options == CacheOptions.NoCache 
                ? fetcher
                : new CachedItemFetcher(fetcher, new Cache<string, Item>(configuration.CacheConfiguration));
        }
    }
}