using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    public interface IFetcher
    {
        Task<LeaderBoard> GetLeaderBoardAsync(PlayerClass playerClass, bool hardcore = false);
    }
}