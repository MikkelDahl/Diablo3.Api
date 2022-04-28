using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.DTOs;

namespace Diablo3.Api.Core.Services
{
    internal class DataFetcher : IFetcher
    {
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        public DataFetcher(IBattleNetApiHttpClient battleNetApiHttpClient)
        {
            this.battleNetApiHttpClient = battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
        }
        
        public async Task<LeaderBoard> GetLeaderBoardAsync(PlayerClass playerClass, bool hardcore = false)
        {
            var currentSeason = await GetCurrentSeasonAsync();
            var requests = new List<string>();
            for (var i = 0; i < 6; i++) 
                requests.Add(CreateGetRequest(playerClass, i, hardcore, currentSeason));
            
            var requestTasks = requests.Select(GetDataObjectAsync).ToList();
            await Task.WhenAll(requestTasks);
            
            return BuildLeaderBoard(requestTasks.Select(t => t.Result).ToList());
        }

        public async Task<int> GetCurrentSeasonAsync()
        {
            const string request = "https://eu.api.blizzard.com/data/d3/season/?access_token=USf56m8BU5LNl13XSnu7x8c0EMNwprwNCB";
            var seasonDataObject = await battleNetApiHttpClient.GetBnetApiResponseAsync<SeasonDataObject>(request);

            return seasonDataObject.current_season;
        }

        private static LeaderBoard BuildLeaderBoard(IReadOnlyList<LeaderBoardDataObject> leaderBoards)
        {
            var leaderBoardEntries = new List<LeaderBoardEntry>();
            for (var i = 0; i < 6; i++)
            {
                var players  = leaderBoards[i].row.SelectMany(a => a.player.Select(p => p.ToPlayer())).ToList();
                var itemSet = ItemSetConverter.GetConvertedSet(players[0].PlayerClass, i);
                var riftInfo =  leaderBoards[i].row.Select(a => new RiftInformation(a.data[1].number, TimeSpan.FromMilliseconds(a.data[2].timestamp), DateTime.Now, itemSet)).ToList();
                var entries = players.Select((p, index) => new LeaderBoardEntry(p, riftInfo[index])).ToList();
                leaderBoardEntries.AddRange(entries);
            }

            var trimmedEntries = leaderBoardEntries
                .OrderByDescending(e => e.RiftInformation.Level)
                .Take(1000)
                .ToList();
            
            return new LeaderBoard(trimmedEntries);
        }
        
        

        private async Task<LeaderBoardDataObject> GetDataObjectAsync(string request) => await battleNetApiHttpClient.GetBnetApiResponseAsync<LeaderBoardDataObject>(request);

        private string CreateGetRequest(PlayerClass playerClass, int setItemIndex, bool isHardcore, int season)
        {
            var region = battleNetApiHttpClient.GetCurrentRegion();
            var setIndex = setItemIndex > 0 ? $"set{setItemIndex}" : "noset";
            var hardcore = isHardcore ? "hardcore-" : "";
            return $"https://{region.ToString().ToLower()}.api.blizzard.com/data/d3/season/{season}/leaderboard/rift-{hardcore}{playerClass.ToString().ToLower()}-{setIndex}?access_token=USSBRq1wybH5l8pk8Yy7ojhUJQX2yOOGZQ";
        }
    }
}