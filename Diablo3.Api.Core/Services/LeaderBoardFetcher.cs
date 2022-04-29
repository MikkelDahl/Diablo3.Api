using System.Data;
using Diablo3.Api.Core.Extensions;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.DTOs;

namespace Diablo3.Api.Core.Services
{
    internal class LeaderBoardFetcher : ILeaderBoardFetcher
    {
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;

        public LeaderBoardFetcher(IBattleNetApiHttpClient battleNetApiHttpClient)
        {
            this.battleNetApiHttpClient = battleNetApiHttpClient ??
                                          throw new ArgumentNullException(nameof(battleNetApiHttpClient));
        }

        public async Task<LeaderBoard> GetLeaderBoardAsync(HeroClass heroClass, bool hardcore = false)
        {
            var currentSeason = await GetCurrentSeasonAsync();
            var requests = new List<string>();
            for (var i = 0; i < 6; i++)
                requests.Add(CreateGetRequest(heroClass, i, hardcore, currentSeason));

            var requestTasks = requests.Select(GetDataObjectAsync).ToList();
            await Task.WhenAll(requestTasks);

            return BuildLeaderBoard(requestTasks.Select(t => t.Result).ToList());
        }

        public async Task<int> GetCurrentSeasonAsync()
        {
            const string request =
                "https://eu.api.blizzard.com/data/d3/season/?access_token=USf56m8BU5LNl13XSnu7x8c0EMNwprwNCB";
            var seasonDataObject = await battleNetApiHttpClient.GetBnetApiResponseAsync<SeasonDataObject>(request);

            return seasonDataObject.current_season;
        }

        private static LeaderBoard BuildLeaderBoard(IReadOnlyList<LeaderBoardDataObject> leaderBoards)
        {
            var leaderBoardEntries = new List<LeaderBoardEntry>();
            for (var i = 0; i < 6; i++)
            {
                var heroes = leaderBoards[i].row
                    .Select(GetLadderHero)
                    .ToList();

                var itemSet = ItemSetConverter.GetConvertedSet(heroes[0].HeroClass, i);
                var riftInfo = leaderBoards[i].row.Select(a => new RiftInformation(a.data[1].number,
                    TimeSpan.FromMilliseconds(a.data[2].timestamp), DateTime.Now, itemSet)).ToList();
                var entries = heroes.Select((p, index) => new LeaderBoardEntry(p, riftInfo[index])).ToList();
                if (!entries.All(e => e.Verify()))
                    throw new ConstraintException("RiftInformation is inconsistent with Hero data.");

                leaderBoardEntries.AddRange(entries);
            }

            var trimmedEntries = leaderBoardEntries
                .OrderByDescending(e => e.RiftInformation.Level)
                .Take(1000)
                .ToList();

            return new LeaderBoard(trimmedEntries);
        }

        private static LadderHero GetLadderHero(LeaderBoardEntryObject entry)
        {
            var dto = new LadderHeroDto
            {
                Id = GetHeroId(entry.player[0].data),
                BattleTag = entry.player[0].data[0].String,
                Class = entry.player[0].data[2].String,
                ParagonLevel = entry.player[0].data[5].number,
                RiftLevel = entry.data[1].number
            };

            return dto.ToLadderHero();
        }

        private static int GetHeroId(IReadOnlyList<Data> data)
        {
            return data.Count < 8
                ? data[6].number
                : data[8].number;
        }

        private async Task<LeaderBoardDataObject> GetDataObjectAsync(string request) =>
            await battleNetApiHttpClient.GetBnetApiResponseAsync<LeaderBoardDataObject>(request);

        private string CreateGetRequest(HeroClass heroClass, int setItemIndex, bool isHardcore, int season)
        {
            var region = battleNetApiHttpClient.GetCurrentRegion();
            var setIndex = setItemIndex > 0 ? $"set{setItemIndex}" : "noset";
            var hardcore = isHardcore ? "hardcore-" : "";
            return
                $"https://{region.ToString().ToLower()}.api.blizzard.com/data/d3/season/{season}/leaderboard/rift-{hardcore}{heroClass.ToString().ToLower()}-{setIndex}?access_token=USSBRq1wybH5l8pk8Yy7ojhUJQX2yOOGZQ";
        }
    }
}