using System;
using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Test.Builders;

public class LeaderBoardEntryBuilder
{
    private Player player;
    private RiftInformation riftInformation;
    
    private LeaderBoardEntryBuilder(Player player, RiftInformation riftInformation)
    {
        this.player = player;
        this.riftInformation = riftInformation;
    }

    public static LeaderBoardEntryBuilder WithDefaultValues() =>
        new LeaderBoardEntryBuilder(
            new Player("TestName", "#1234", PlayerClass.Barbarian, 1000),
            new RiftInformation(100, TimeSpan.FromMinutes(10), DateTime.UtcNow, ItemSet.Raekor)
        );
    
    public LeaderBoardEntryBuilder WithRiftInformation(RiftInformation riftInformation) =>
        new LeaderBoardEntryBuilder(player, riftInformation);
    
    public LeaderBoardEntryBuilder WithPlayer(Player player) =>
        new LeaderBoardEntryBuilder(player, riftInformation);

    public LeaderBoardEntry Build() => new LeaderBoardEntry(player, riftInformation);
}