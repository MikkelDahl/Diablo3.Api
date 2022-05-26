using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Services;

namespace Diablo3.Api.Core
{
    public interface IClient
    {
        Task<ICollection<LeaderBoard>> GetAllLeaderBoardsAsync();
        Task<ICollection<LeaderBoard>> GetAllHardcoreLeaderBoardsAsync();
        Task<LeaderBoard> GetLeaderBoardForClassAsync(HeroClass heroClass);
        Task<LeaderBoard> GetHardcoreLeaderBoardForClassAsync(HeroClass heroClass);
        Task<LeaderBoard> GetLeaderBoardForItemSetAsync(ItemSet set);
        Task<LeaderBoard> GetHardcoreLeaderBoardForItemSetAsync(ItemSet set);
        Task<Item> GetItemAsync(string itemName);
        Task<ICollection<Item>> GetAllItemsAsync();
        IHeroFetcher Characters { get; }

        int GetCurrentSeason();
    }
}