namespace Diablo3.Api.Core.Models
{
    public class LeaderBoard
    {
        public LeaderBoard(IEnumerable<LeaderBoardEntry> entries)
        {
            Entries = entries.OrderByDescending(e => e.RiftInformation.Level).ToList();
        }
    
        public List<LeaderBoardEntry> Entries { get; }
        public Player GetHighestRankedPlayer() => Entries.First().Player;
        public Player GetHighestParagonPlayer() => Entries
            .OrderByDescending(e => e.Player.Paragon)
            .First()
            .Player;
        
        public ItemSet GetMostPopularSet() => Entries
            .Select(e => e.RiftInformation)
            .GroupBy(a => a.ItemSet)
            .Select(g => new { Set = g.Key, Count = g.Count()})
            .OrderByDescending(s => s.Count)
            .First()
            .Set;
    }
}