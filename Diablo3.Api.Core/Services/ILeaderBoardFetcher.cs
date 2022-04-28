using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    public interface ILeaderBoardFetcher
    {
        Task<LeaderBoard> GetLeaderBoardAsync(HeroClass heroClass, bool hardcore = false);
        Task<int> GetCurrentSeasonAsync();
    }
}