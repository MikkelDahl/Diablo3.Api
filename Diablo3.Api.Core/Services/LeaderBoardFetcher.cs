using System.Data;
using Diablo3.Api.Core.Extensions;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.DTOs;

namespace Diablo3.Api.Core.Services
{
    internal abstract class LeaderBoardFetcher : ILeaderBoardFetcher
    {
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;

        protected LeaderBoardFetcher(IBattleNetApiHttpClient battleNetApiHttpClient)
        {
            this.battleNetApiHttpClient = battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
        }

        public async Task<LeaderBoard> GetLeaderBoardAsync(HeroClass heroClass)
        {
            var items = ItemSetConverter.GetForClass(heroClass);
            var allEntries = new List<LeaderBoardEntry>();

            foreach (var item in items)
            {
                var itemSetLeaderBoard = await GetLeaderBoardForItemSetAsync(item);
                allEntries.AddRange(itemSetLeaderBoard.Entries);
            }

            return await BuildLeaderBoard(allEntries);
        }
    
        public async Task<LeaderBoard> GetLeaderBoardForItemSetAsync(ItemSet itemSet)
        {
            var heroClass = ItemSetConverter.GetConvertedHeroClass(itemSet);
            var setIndex = ItemSetConverter.GetConvertedIndex(itemSet);
            var request = CreateGetRequest(heroClass, setIndex);
            var leaderBoardDto = await GetDataObjectAsync(request);

            return BuildLeaderBoardWithItemSet(leaderBoardDto, itemSet);
        }
    
        private static Task<LeaderBoard> BuildLeaderBoard(IReadOnlyCollection<LeaderBoardEntry> entries)
        {
            if (!entries.All(e => e.Verify()))
                throw new ConstraintException("RiftInformation is inconsistent with Hero data.");


            var trimmedEntries = entries
                .OrderByDescending(e => e.RiftInformation.Level)
                .Take(1000)
                .ToList();

            return Task.FromResult(new LeaderBoard(trimmedEntries));
        }
    
        private static LeaderBoard BuildLeaderBoardWithItemSet(LeaderBoardDataObject leaderBoardDto, ItemSet itemSet)
        {
            var leaderBoardEntries = new List<LeaderBoardEntry>();
            var heroes = leaderBoardDto.row
                .Select(GetLadderHero)
                .ToList();

            var riftInfo = leaderBoardDto.row.Select(a => new RiftInformation(a.data[1].number,
                TimeSpan.FromMilliseconds(a.data[2].timestamp), DateTime.MinValue.AddMilliseconds(a.data[3].timestamp), itemSet)).ToList();
            var entries = heroes.Select((p, index) => new LeaderBoardEntry(p, riftInfo[index])).ToList();
            if (!entries.All(e => e.Verify()))
                throw new ConstraintException("RiftInformation is inconsistent with Hero data.");

            leaderBoardEntries.AddRange(entries);

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

        private static int GetHeroId(IReadOnlyList<Data> data) =>
            data.Count < 8
                ? data[6].number
                : data[8].number;
    
        private async Task<LeaderBoardDataObject> GetDataObjectAsync(string request) =>
            await battleNetApiHttpClient.GetBnetApiResponseAsync<LeaderBoardDataObject>(request);

        protected abstract string CreateGetRequest(HeroClass heroClass, int setItemIndex);
    }
}