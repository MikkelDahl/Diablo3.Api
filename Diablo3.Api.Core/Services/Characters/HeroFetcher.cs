using Diablo3.Api.Core.Exceptions;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.DTOs;

namespace Diablo3.Api.Core.Services.Characters
{
    internal class HeroFetcher : IHeroFetcher
    {
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;

        public HeroFetcher(IBattleNetApiHttpClient battleNetApiHttpClient)
        {
            this.battleNetApiHttpClient =
                battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
        }

        public Hero Get(int id, string battleTag)
        {
            return Task.Run(() => GetAsync(id, battleTag)).GetAwaiter().GetResult();
        }

        public async Task<Hero> GetAsync(int id, string battleTag)
        {
            var request = CreateGetRequest(id, battleTag);
            try
            {
                var heroDto = await battleNetApiHttpClient.GetBnetApiResponseAsync<HeroDto>(request);
                return heroDto.ToHero(battleTag);
            }
            catch (Exception e)
            {
                throw new HeroNotFoundException(id, battleTag);
            }
        }
    
        private string CreateGetRequest(int id, string battleTag)
        {
            var region = battleNetApiHttpClient.GetCurrentRegion();
            var battleTagName = battleTag.Split("#", StringSplitOptions.RemoveEmptyEntries);
            return $"https://{region.ToString().ToLower()}.api.blizzard.com/d3/profile/{battleTagName[0]}%23{battleTagName[1]}/hero/{id}?access_token=USSBRq1wybH5l8pk8Yy7ojhUJQX2yOOGZQ";
        }
    }
}