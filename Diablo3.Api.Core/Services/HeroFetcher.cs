using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services;

public class HeroFetcher : IHeroFetcher
{
    private IBattleNetApiHttpClient battleNetApiHttpClient;

    public HeroFetcher(IBattleNetApiHttpClient battleNetApiHttpClient)
    {
        this.battleNetApiHttpClient =
            battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
    }

    public Hero Get(int id, string battleTag)
    {
        return Task.Run(() => GetAsync(id, battleTag)).GetAwaiter().GetResult();
    }

    public Task<Hero> GetAsync(int id, string battleTag)
    {
        var request = CreateGetRequest(id, battleTag);
        
    }
    
    private string CreateGetRequest(int id, string battleTag)
    {
        var region = battleNetApiHttpClient.GetCurrentRegion();
        return $"https://{region.ToString().ToLower()}.api.blizzard.com/data/d3/profile/{battleTag}/hero/{id}?access_token=USSBRq1wybH5l8pk8Yy7ojhUJQX2yOOGZQ";
    }
}