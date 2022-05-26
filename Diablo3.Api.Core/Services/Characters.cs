using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services;

public class Characters : ICharacterService
{
    private readonly IHeroFetcher heroFetcher;

    public Characters(IHeroFetcher heroFetcher)
    {
        this.heroFetcher = heroFetcher ?? throw new ArgumentNullException(nameof(heroFetcher));
    }


    public Hero Get(int id, string battleTag) => heroFetcher.Get(id, battleTag);

    public async Task<Hero> GetAsync(int id, string battleTag) => await heroFetcher.GetAsync(id, battleTag);
}