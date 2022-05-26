using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    public interface ILeaderBoardFetcher
    {
        Task<LeaderBoard> GetAsync(HeroClass heroClass);
        Task<LeaderBoard> GetForItemSetAsync(ItemSet itemSet);
    }
}