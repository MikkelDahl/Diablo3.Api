using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Services;

namespace Diablo3.Api.Core;

public class DiabloClient : IClient
{
    private readonly IFetcher dataFetcher;
    private readonly Credentials credentials;

    public DiabloClient(IFetcher dataFetcher)
    {
        this.dataFetcher = dataFetcher ?? throw new ArgumentNullException(nameof(dataFetcher));
    }

    public async Task<ICollection<LeaderBoard>> GetAllAsync()
    {
        var dataFetchingTasks = new List<Task<LeaderBoard>>()
        {
            GetForClassAsync(PlayerClass.Barbarian),
            GetForClassAsync(PlayerClass.DemonHunter),
            GetForClassAsync(PlayerClass.Monk),
            GetForClassAsync(PlayerClass.Necromancer),
            GetForClassAsync(PlayerClass.WitchDoctor),
            GetForClassAsync(PlayerClass.Wizard)
        };

        await Task.WhenAll(dataFetchingTasks);
        return dataFetchingTasks.Select(t => t.Result).ToList();
    }

    public async Task<LeaderBoard> GetForClassAsync(PlayerClass playerClass) =>
        await dataFetcher.GetLeaderBoardAsync(playerClass);

    public async Task<LeaderBoard> GetHardcoreForClassAsync(PlayerClass playerClass) =>
        await dataFetcher.GetLeaderBoardAsync(playerClass, true);
}
  