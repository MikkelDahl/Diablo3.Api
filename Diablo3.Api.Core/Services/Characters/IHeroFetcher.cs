using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services.Characters
{
    public interface IHeroFetcher
    {
        Hero Get(int id, string battleTag);
        Task<Hero> GetAsync(int id, string battleTag);
    }
}