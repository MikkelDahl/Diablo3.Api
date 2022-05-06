using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Services;
using Serilog;

namespace Diablo3.Api.Core
{
    public class DiabloClient : IClient
    {
        private readonly IHeroFetcher heroFetcher;
        private readonly ClientConfiguration clientConfiguration;
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        private readonly ILogger logger;
        private readonly int currentSeason;

        public DiabloClient(IHeroFetcher heroFetcher, ClientConfiguration clientConfiguration, IBattleNetApiHttpClient battleNetApiHttpClient, ILogger logger, int currentSeason)
        {
            this.heroFetcher = heroFetcher ?? throw new ArgumentNullException(nameof(heroFetcher)); 
            this.clientConfiguration = clientConfiguration;
            this.battleNetApiHttpClient = battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.currentSeason = currentSeason;

            if (clientConfiguration.CacheConfiguration.Options == CacheOptions.Preload) 
                InitializeCache().Wait();
        }

        public async Task<ICollection<LeaderBoard>> GetAllAsync()
        {
            var dataFetchingTasks = new List<Task<LeaderBoard>>()
            {
                GetLeaderBoardForClassAsync(HeroClass.Barbarian),
                GetLeaderBoardForClassAsync(HeroClass.Crusader),
                GetLeaderBoardForClassAsync(HeroClass.DH),
                GetLeaderBoardForClassAsync(HeroClass.Monk),
                GetLeaderBoardForClassAsync(HeroClass.Necromancer),
                GetLeaderBoardForClassAsync(HeroClass.WitchDoctor),
                GetLeaderBoardForClassAsync(HeroClass.Wizard)
            };

            await Task.WhenAll(dataFetchingTasks);
            return dataFetchingTasks.Select(t => t.Result).ToList();
        }

        public async Task<ICollection<LeaderBoard>> GetAllHardcoreAsync()
        {
            var dataFetchingTasks = new List<Task<LeaderBoard>>()
            {
                GetHardcoreLeaderBoardForClassAsync(HeroClass.Barbarian),
                GetHardcoreLeaderBoardForClassAsync(HeroClass.Crusader),
                GetHardcoreLeaderBoardForClassAsync(HeroClass.DH),
                GetHardcoreLeaderBoardForClassAsync(HeroClass.Monk),
                GetHardcoreLeaderBoardForClassAsync(HeroClass.Necromancer),
                GetHardcoreLeaderBoardForClassAsync(HeroClass.WitchDoctor),
                GetHardcoreLeaderBoardForClassAsync(HeroClass.Wizard)
            };

            await Task.WhenAll(dataFetchingTasks);
            return dataFetchingTasks.Select(t => t.Result).ToList();
        }

        public async Task<LeaderBoard> GetLeaderBoardForClassAsync(HeroClass heroClass)
        {
            var dataFetcherFactory = new DataFetcherFactory(clientConfiguration.CacheConfiguration, battleNetApiHttpClient, false);
            var leaderBoardFetcher = dataFetcherFactory.Build();
            return await leaderBoardFetcher.GetLeaderBoardAsync(heroClass);
        }

        public async Task<LeaderBoard> GetHardcoreLeaderBoardForClassAsync(HeroClass heroClass)
        {
            var dataFetcherFactory = new DataFetcherFactory(clientConfiguration.CacheConfiguration, battleNetApiHttpClient, true);
            var leaderBoardFetcher = dataFetcherFactory.Build();
            return await leaderBoardFetcher.GetLeaderBoardAsync(heroClass);
        }

        public Hero GetHero(int id, string battleTag) => heroFetcher.Get(id, battleTag);

        public async Task<Hero> GetHeroAsync(int id, string battleTag) => await heroFetcher.GetAsync(id, battleTag);

        public int GetCurrentSeason() => currentSeason;

        private async Task InitializeCache()
        {
            var dataFetcherFactory = new DataFetcherFactory(clientConfiguration.CacheConfiguration, battleNetApiHttpClient, false);
            var leaderBoardFetcher = dataFetcherFactory.Build();
            for (var i = 0; i < 7; i++)
            {
                var heroClass = (HeroClass)i;
                
                logger.Information($"Caching for all {heroClass} sets");
                var _ = await leaderBoardFetcher.GetLeaderBoardAsync(heroClass);
                var allItemSets = ItemSetConverter.GetForClass(heroClass);
                foreach (var itemSet in allItemSets)
                {
                    logger.Information($"Caching for set: {itemSet}");
                    try
                    {
                        var normalDataForItemSet = await leaderBoardFetcher.GetLeaderBoardForItemSetAsync(itemSet);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + $" Skipping cache load for {heroClass}/{itemSet}.");
                    }
                }
                
                logger.Information($"Finished initializing cache for {heroClass}");
            }
        }
    }
}