using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    public interface IHeroFetcher
    {
        Hero Get(int id);
        Hero GetByBattleTag(string battleTag);
    }
}