using System.Collections.Generic;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Test.Builders;
using NUnit.Framework;

namespace Diablo3.Api.Core.Test.Models;

[TestFixture]
public class LeaderBoardTest
{
    private LeaderBoard sut;
    
    [SetUp]
    public void Setup()
    {
        var riftInformations = new List<RiftInformation>()
        {
            RiftInformationBuilder.WithDefaultValues().WithLevel(20).WithItemSet(ItemSet.Aegis).Build(),
            RiftInformationBuilder.WithDefaultValues().WithLevel(10).WithItemSet(ItemSet.Akkhan).Build(),
            RiftInformationBuilder.WithDefaultValues().WithLevel(50).WithItemSet(ItemSet.Aegis).Build(),
            RiftInformationBuilder.WithDefaultValues().WithLevel(40).WithItemSet(ItemSet.MonkeyKing).Build(),
            RiftInformationBuilder.WithDefaultValues().WithLevel(30).WithItemSet(ItemSet.Inarius).Build()
        };

        var entries = new List<LeaderBoardEntry>
        {
            LeaderBoardEntryBuilder.WithDefaultValues().WithRiftInformation(riftInformations[0]).Build(),
            LeaderBoardEntryBuilder.WithDefaultValues().WithRiftInformation(riftInformations[1]).Build(),
            LeaderBoardEntryBuilder.WithDefaultValues()
                .WithPlayer(new Player("highestRankedPlayer", "", PlayerClass.Barbarian, 1000))
                .WithRiftInformation(riftInformations[2])
                .Build(),
            LeaderBoardEntryBuilder.WithDefaultValues()
                .WithPlayer(new Player("highestParagonPlayer", "", PlayerClass.Barbarian, 2000))
                .WithRiftInformation(riftInformations[3]).Build(),
            LeaderBoardEntryBuilder.WithDefaultValues().WithRiftInformation(riftInformations[4]).Build()
        };

        sut = new LeaderBoard(entries);
    }

    [Test]
    public void GetHighestRankedPlayer_returns_player_with_highest_rift_level_clear()
    {
        Assert.That(sut.GetHighestRankedPlayer().Name, Is.EqualTo("highestRankedPlayer"));
    }

    [Test]
    public void GetHighestParagonPlayer_returns_player_with_highest_paragon_level()
    {
        Assert.That(sut.GetHighestParagonPlayer().Name, Is.EqualTo("highestParagonPlayer"));
    }

    [Test]
    public void GetMostPopularSet_returns_most_used_set()
    {
        Assert.That(sut.GetMostPopularSet(), Is.EqualTo(ItemSet.Aegis));
    }
}