using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    internal class NormalLeaderBoardFetcher : LeaderBoardFetcher
    {
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        private readonly int currentSeason;

        public NormalLeaderBoardFetcher(IBattleNetApiHttpClient battleNetApiHttpClient, int currentSeason) : base(battleNetApiHttpClient)
        {
            this.battleNetApiHttpClient = battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
            this.currentSeason = currentSeason;
        }
        
        protected override string CreateGetRequest(HeroClass heroClass, int setItemIndex)
        {
            var heroClassParam = heroClass == HeroClass.DemonHunter && setItemIndex < 0 
                ? "dh" 
                : heroClass.ToString().ToLower();
            
            var region = battleNetApiHttpClient.GetCurrentRegion();
            var setIndex = setItemIndex > 0
                ? $"-set{setItemIndex}"
                : setItemIndex == 0
                    ? "-noset"
                    : "";

            return
                $"https://{region.ToString().ToLower()}.api.blizzard.com/data/d3/season/{currentSeason}/leaderboard/rift-{heroClassParam}{setIndex}?access_token=USSBRq1wybH5l8pk8Yy7ojhUJQX2yOOGZQ";
        }
    }
}