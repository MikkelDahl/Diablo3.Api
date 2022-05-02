using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core
{
    public interface IClient
    {
        Task<ICollection<LeaderBoard>> GetAllAsync();
        Task<ICollection<LeaderBoard>> GetAllHardcoreAsync();
        Task<LeaderBoard> GetForClassAsync(HeroClass heroClass);
        Task<LeaderBoard> GetHardcoreForClassAsync(HeroClass heroClass);
        Hero GetHero(int id, string battleTag);
        Task<Hero> GetHeroAsync(int id, string battleTag);

        int GetCurrentSeason();
    }
}