using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core
{
    public interface IClient
    {
        Task<ICollection<LeaderBoard>> GetAllLeaderBoardsAsync();
        Task<ICollection<LeaderBoard>> GetAllHardcoreLeaderBoardsAsync();
        Task<LeaderBoard> GetLeaderBoardForClassAsync(HeroClass heroClass);
        Task<LeaderBoard> GetHardcoreLeaderBoardForClassAsync(HeroClass heroClass);
        Hero GetHero(int id, string battleTag);
        Task<Hero> GetHeroAsync(int id, string battleTag);
        Task<Item> GetItemAsync(string itemName);
        Task<ICollection<Item>> GetAllItemsAsync();

        int GetCurrentSeason();
    }
}