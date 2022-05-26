using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Services;
using Serilog;

namespace Diablo3.Api.Core
{
    internal class DiabloClient : IClient
    {
        private readonly ClientConfiguration clientConfiguration;
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        private readonly ILogger logger;
        private readonly int currentSeason;

        internal DiabloClient(ILeaderBoardService leaderBoardFetcher, IHeroFetcher heroFetcher, ClientConfiguration clientConfiguration,
            IBattleNetApiHttpClient battleNetApiHttpClient, ILogger logger, int currentSeason, IItemCache itemCache)
        {
            Characters = heroFetcher ?? throw new ArgumentNullException(nameof(heroFetcher));
            Items = itemCache ?? throw new ArgumentNullException(nameof(itemCache));
            LeaderBoards = leaderBoardFetcher ?? throw new ArgumentNullException(nameof(leaderBoardFetcher));
            this.clientConfiguration = clientConfiguration ?? throw new ArgumentNullException(nameof(clientConfiguration));
            this.battleNetApiHttpClient = battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (currentSeason <= 0) 
                throw new ArgumentOutOfRangeException(nameof(currentSeason));
            
            this.currentSeason = currentSeason;

            if (clientConfiguration.CacheConfiguration.Options == CacheOptions.Preload)
                InitializeCache().Wait();
        }

        public IHeroFetcher Characters { get; }
        public IItemCache Items { get; }
        public ILeaderBoardService LeaderBoards { get; }

        public async Task<ICollection<LeaderBoard>> GetAllLeaderBoardsAsync()
        {
            var dataFetchingTasks = new List<Task<LeaderBoard>>()
            {
                GetLeaderBoardForClassAsync(HeroClass.Barbarian),
                GetLeaderBoardForClassAsync(HeroClass.Crusader),
                GetLeaderBoardForClassAsync(HeroClass.DemonHunter),
                GetLeaderBoardForClassAsync(HeroClass.Monk),
                GetLeaderBoardForClassAsync(HeroClass.Necromancer),
                GetLeaderBoardForClassAsync(HeroClass.WitchDoctor),
                GetLeaderBoardForClassAsync(HeroClass.Wizard)
            };

            await Task.WhenAll(dataFetchingTasks);
            return dataFetchingTasks.Select(t => t.Result).ToList();
        }

        public async Task<ICollection<LeaderBoard>> GetAllHardcoreLeaderBoardsAsync()
        {
            var dataFetchingTasks = new List<Task<LeaderBoard>>()
            {
                GetHardcoreLeaderBoardForClassAsync(HeroClass.Barbarian),
                GetHardcoreLeaderBoardForClassAsync(HeroClass.Crusader),
                GetHardcoreLeaderBoardForClassAsync(HeroClass.DemonHunter),
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
            var leaderBoardFetcherFactory = new LeaderBoardFetcherFactory(clientConfiguration.CacheConfiguration, battleNetApiHttpClient);
            var leaderBoardFetcher = leaderBoardFetcherFactory.Build();
            return await leaderBoardFetcher.GetAsync(heroClass);
        }

        public async Task<LeaderBoard> GetHardcoreLeaderBoardForClassAsync(HeroClass heroClass)
        {
            var leaderBoardFetcherFactory = new LeaderBoardFetcherFactory(clientConfiguration.CacheConfiguration, battleNetApiHttpClient);
            var leaderBoardFetcher = leaderBoardFetcherFactory.BuildHardcore();
            return await leaderBoardFetcher.GetAsync(heroClass);
        }

        public async Task<LeaderBoard> GetLeaderBoardForItemSetAsync(ItemSet set)
        {
            var leaderBoardFetcherFactory = new LeaderBoardFetcherFactory(clientConfiguration.CacheConfiguration, battleNetApiHttpClient);
            var leaderBoardFetcher = leaderBoardFetcherFactory.Build();
            return await leaderBoardFetcher.GetForItemSetAsync(set);
        }

        public async Task<LeaderBoard> GetHardcoreLeaderBoardForItemSetAsync(ItemSet set)
        {
            var leaderBoardFetcherFactory = new LeaderBoardFetcherFactory(clientConfiguration.CacheConfiguration, battleNetApiHttpClient);
            var leaderBoardFetcher = leaderBoardFetcherFactory.BuildHardcore();
            return await leaderBoardFetcher.GetForItemSetAsync(set);
        }

        public int GetCurrentSeason() => currentSeason;

        private async Task InitializeCache()
        {
            var leaderBoardFetcherFactory =
                new LeaderBoardFetcherFactory(clientConfiguration.CacheConfiguration, battleNetApiHttpClient);
            var leaderBoardFetcher = leaderBoardFetcherFactory.Build();
            var hardcoreLeaderBoardFetcher = leaderBoardFetcherFactory.BuildHardcore();
            for (var i = 0; i < 7; i++)
            {
                var heroClass = (HeroClass)i;

                logger.Information($"Caching for all {heroClass} sets");
                var normal = await leaderBoardFetcher.GetAsync(heroClass);
                logger.Information($"Caching for all {heroClass} sets (HC)");
                var hc = await hardcoreLeaderBoardFetcher.GetAsync(heroClass);
                var allItemSets = ItemSetConverter.GetForClass(heroClass);
                foreach (var itemSet in allItemSets)
                {
                    logger.Information($"Caching for set: {itemSet}");
                    try
                    {
                        var normalDataForItemSet = await leaderBoardFetcher.GetForItemSetAsync(itemSet);
                        var hcDataForItemSet = await hardcoreLeaderBoardFetcher.GetForItemSetAsync(itemSet);
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