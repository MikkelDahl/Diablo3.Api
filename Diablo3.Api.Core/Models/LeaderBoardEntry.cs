namespace Diablo3.Api.Core.Models;

public class LeaderBoardEntry
{
    public LeaderBoardEntry(Player player, RiftInformation riftInformation)
    {
        Player = player;
        RiftInformation = riftInformation;
    }

    public Player Player { get; }
    public RiftInformation RiftInformation { get; }
}