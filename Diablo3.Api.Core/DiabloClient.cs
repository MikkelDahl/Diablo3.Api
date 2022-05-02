using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Services;

namespace Diablo3.Api.Core
{
    public class DiabloClient : IClient
    {
        private readonly IHeroFetcher heroFetcher;
        private readonly ClientConfiguration clientConfiguration;
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        private readonly int currentSeason;

        public DiabloClient(IHeroFetcher heroFetcher, ClientConfiguration clientConfiguration, IBattleNetApiHttpClient battleNetApiHttpClient, int currentSeason)
        {
            this.heroFetcher = heroFetcher ?? throw new ArgumentNullException(nameof(heroFetcher)); 
            this.clientConfiguration = clientConfiguration;
            this.battleNetApiHttpClient = battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
            this.currentSeason = currentSeason;
        }

        public async Task<ICollection<LeaderBoard>> GetAllAsync()
        {
            var dataFetchingTasks = new List<Task<LeaderBoard>>()
            {
                GetForClassAsync(HeroClass.Barbarian),
                GetForClassAsync(HeroClass.Crusader),
                GetForClassAsync(HeroClass.DH),
                GetForClassAsync(HeroClass.Monk),
                GetForClassAsync(HeroClass.Necromancer),
                GetForClassAsync(HeroClass.WitchDoctor),
                GetForClassAsync(HeroClass.Wizard)
            };

            await Task.WhenAll(dataFetchingTasks);
            return dataFetchingTasks.Select(t => t.Result).ToList();
        }

        public async Task<ICollection<LeaderBoard>> GetAllHardcoreAsync()
        {
            var dataFetchingTasks = new List<Task<LeaderBoard>>()
            {
                GetHardcoreForClassAsync(HeroClass.Barbarian),
                GetHardcoreForClassAsync(HeroClass.Crusader),
                GetHardcoreForClassAsync(HeroClass.DH),
                GetHardcoreForClassAsync(HeroClass.Monk),
                GetHardcoreForClassAsync(HeroClass.Necromancer),
                GetHardcoreForClassAsync(HeroClass.WitchDoctor),
                GetHardcoreForClassAsync(HeroClass.Wizard)
            };

            await Task.WhenAll(dataFetchingTasks);
            return dataFetchingTasks.Select(t => t.Result).ToList();
        }

        public async Task<LeaderBoard> GetForClassAsync(HeroClass heroClass)
        {
            var dataFetcherFactory = new DataFetcherFactory(clientConfiguration.CacheConfiguration, battleNetApiHttpClient, false);
            var leaderBoardFetcher = dataFetcherFactory.Build();
            return await leaderBoardFetcher.GetLeaderBoardAsync(heroClass);
        }

        public async Task<LeaderBoard> GetHardcoreForClassAsync(HeroClass heroClass)
        {
            var dataFetcherFactory = new DataFetcherFactory(clientConfiguration.CacheConfiguration, battleNetApiHttpClient, true);
            var leaderBoardFetcher = dataFetcherFactory.Build();
            return await leaderBoardFetcher.GetLeaderBoardAsync(heroClass);
        }

        public Hero GetHero(int id, string battleTag) => heroFetcher.Get(id, battleTag);

        public async Task<Hero> GetHeroAsync(int id, string battleTag) => await heroFetcher.GetAsync(id, battleTag);

        public int GetCurrentSeason() => currentSeason;
    }
}