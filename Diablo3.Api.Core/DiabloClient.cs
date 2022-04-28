using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Services;

namespace Diablo3.Api.Core
{
    public class DiabloClient : IClient
    {
        private readonly ILeaderBoardFetcher dataLeaderBoardFetcher;

        public DiabloClient(ILeaderBoardFetcher dataLeaderBoardFetcher)
        {
            this.dataLeaderBoardFetcher = dataLeaderBoardFetcher ?? throw new ArgumentNullException(nameof(dataLeaderBoardFetcher));
        }

        public async Task<ICollection<LeaderBoard>> GetAllAsync()
        {
            var dataFetchingTasks = new List<Task<LeaderBoard>>()
            {
                GetForClassAsync(HeroClass.Barbarian),
                GetForClassAsync(HeroClass.Crusader),
                GetForClassAsync(HeroClass.DemonHunter),
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
                GetHardcoreForClassAsync(HeroClass.DemonHunter),
                GetHardcoreForClassAsync(HeroClass.Monk),
                GetHardcoreForClassAsync(HeroClass.Necromancer),
                GetHardcoreForClassAsync(HeroClass.WitchDoctor),
                GetHardcoreForClassAsync(HeroClass.Wizard)
            };

            await Task.WhenAll(dataFetchingTasks);
            return dataFetchingTasks.Select(t => t.Result).ToList();
        }

        public async Task<LeaderBoard> GetForClassAsync(HeroClass heroClass) =>
            await dataLeaderBoardFetcher.GetLeaderBoardAsync(heroClass);

        public async Task<LeaderBoard> GetHardcoreForClassAsync(HeroClass heroClass) =>
            await dataLeaderBoardFetcher.GetLeaderBoardAsync(heroClass, true);

        public async Task<int> GetCurrentSeasonAsync() => await dataLeaderBoardFetcher.GetCurrentSeasonAsync();
    }
}
  