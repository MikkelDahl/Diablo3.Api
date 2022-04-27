namespace Diablo3.Api.Core.Models;

public class LeaderBoard
{
    // public LeaderBoard(LeaderBoardDataObject leaderBoardDto)
    // {
    //     var players  = leaderBoardDto.row.SelectMany(a => a.player.Select(p => p.ToPlayer())).ToList();
    //     var riftInfo = leaderBoardDto.row.Select(a => new RiftInformation(a.data[1].number, TimeSpan.FromMilliseconds(a.data[2].timestamp), DateTime.Now)).ToList();
    //     var entries = players.Select((p, i) => new LeaderBoardEntry(p, riftInfo[i])).ToList();
    //     Entries = entries;
    // }

    public LeaderBoard(List<LeaderBoardEntry> entries)
    {
        Entries = entries;
    }
    
    public List<LeaderBoardEntry> Entries { get; }
}