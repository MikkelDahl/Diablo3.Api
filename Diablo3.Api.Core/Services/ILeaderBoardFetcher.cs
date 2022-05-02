using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    public interface ILeaderBoardFetcher
    {
        Task<LeaderBoard> GetLeaderBoardAsync(HeroClass heroClass, bool hardcore = false);
        Task<LeaderBoard> GetLeaderBoardForItemSetAsync(ItemSet itemSet, bool hardcore = false);
        int GetCurrentSeason();
    }
}