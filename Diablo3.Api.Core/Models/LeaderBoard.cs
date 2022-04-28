namespace Diablo3.Api.Core.Models
{
    public class LeaderBoard
    {
        public LeaderBoard(IEnumerable<LeaderBoardEntry> entries)
        {
            Entries = entries.OrderByDescending(e => e.RiftInformation.Level).ToList();
        }
    
        public List<LeaderBoardEntry> Entries { get; }
        public Hero GetHighestRankedPlayer() => Entries.First().Hero;
        public Hero GetHighestParagonPlayer() => Entries
            .OrderByDescending(e => e.Hero.Paragon)
            .First()
            .Hero;
        
        public ItemSet GetMostPopularSet() => Entries
            .Select(e => e.RiftInformation)
            .GroupBy(a => a.ItemSet)
            .Select(g => new { Set = g.Key, Count = g.Count()})
            .OrderByDescending(s => s.Count)
            .First()
            .Set;
    }
}