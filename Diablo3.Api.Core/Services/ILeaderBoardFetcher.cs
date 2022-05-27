using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    public interface ILeaderBoardFetcher
    {
        LeaderBoard Get(HeroClass heroClass);
        LeaderBoard Get(ItemSet set);
        Task<LeaderBoard> GetAsync(HeroClass heroClass);
        Task<LeaderBoard> GetAsync(ItemSet set);
        Task<ICollection<LeaderBoard>> GetAllAsync();
    }
}