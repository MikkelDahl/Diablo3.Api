namespace Diablo3.Api.Core.Models
{
    public class LeaderBoard
    {
        public LeaderBoard(List<LeaderBoardEntry> entries)
        {
            if (entries.Any(e => e.LadderHero.HeroClass != entries[0].LadderHero.HeroClass))
                throw new ArgumentException("Inconsistent LeaderBoard Entries: All entries must have same Hero Class", nameof(entries));
            
            Entries = entries.OrderByDescending(e => e.RiftInformation.Level).ToList();
        }
    
        public List<LeaderBoardEntry> Entries { get; }
        public LadderHero GetHighestRankedPlayer() => Entries.First().LadderHero;
        public LadderHero GetHighestParagonPlayer() => Entries
            .OrderByDescending(e => e.LadderHero.Paragon)
            .First()
            .LadderHero;
        
        public (ItemSet Set, int Count) GetMostPopularSet()
        {
            var highestCount = Entries
                .Select(e => e.RiftInformation)
                .GroupBy(a => a.ItemSet)
                .Select(g => new { Set = g.Key, Count = g.Count() })
                .OrderByDescending(s => s.Count)
                .First();

            return (highestCount.Set, highestCount.Count);
        }
    }
}