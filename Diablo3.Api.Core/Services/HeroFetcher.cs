using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services;

public class HeroFetcher : IHeroFetcher
{
    private IBattleNetApiHttpClient battleNetApiHttpClient;
    public HeroFetcher(IBattleNetApiHttpClient battleNetApiHttpClient)
    {
        this.battleNetApiHttpClient = battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
    }
    public Hero Get(int id)
    {
        throw new NotImplementedException();
    }

    public Hero GetByBattleTag(string battleTag)
    {
        throw new NotImplementedException();
    }
}