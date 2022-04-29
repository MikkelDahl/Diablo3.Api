namespace Diablo3.Api.Core.Models
{
    public class LeaderBoard
    {
        public LeaderBoard(IEnumerable<LeaderBoardEntry> entries)
        {
            Entries = entries.OrderByDescending(e => e.RiftInformation.Level).ToList();
        }
    
        public List<LeaderBoardEntry> Entries { get; }
        public LadderHero GetHighestRankedPlayer() => Entries.First().LadderHero;
        public LadderHero GetHighestParagonPlayer() => Entries
            .OrderByDescending(e => e.LadderHero.Paragon)
            .First()
            .LadderHero;
        
        public ItemSet GetMostPopularSet() => Entries
            .Select(e => e.RiftInformation)
            .GroupBy(a => a.ItemSet)
            .Select(g => new { Set = g.Key, Count = g.Count()})
            .OrderByDescending(s => s.Count)
            .First()
            .Set;
    }
}