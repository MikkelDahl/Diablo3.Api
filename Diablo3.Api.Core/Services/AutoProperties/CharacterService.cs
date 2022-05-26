using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services.AutoProperties;

public class CharacterService : IHeroFetcher
{
    private readonly IHeroFetcher heroFetcher;

    public CharacterService(IHeroFetcher heroFetcher)
    {
        this.heroFetcher = heroFetcher ?? throw new ArgumentNullException(nameof(heroFetcher));
    }


    public Hero Get(int id, string battleTag) => heroFetcher.Get(id, battleTag);

    public async Task<Hero> GetAsync(int id, string battleTag) => await heroFetcher.GetAsync(id, battleTag);
}