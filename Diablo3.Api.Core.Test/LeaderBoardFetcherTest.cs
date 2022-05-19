using System;
using System.Collections.Generic;
using Diablo3.Api.Core.Models.DTOs;
using Diablo3.Api.Core.Services;
using Moq;
using NUnit.Framework;

namespace Diablo3.Api.Core.Test;

[TestFixture]
public class LeaderBoardFetcherTest
{
    private Mock<IBattleNetApiHttpClient> battleNetApiHttpClientMock;

    [TearDown]
    public void TearDown()
    {
        battleNetApiHttpClientMock = new Mock<IBattleNetApiHttpClient>();
    }

    [Test]
    public void Constructor_throws_on_null_args()
    {
        Assert.Throws<ArgumentNullException>(() => new NormalLeaderBoardFetcher(null, 1));
    }


    [Test]
    public void GetLeaderBoardAsync_gets_top_players_across_itemsets()
    {
        var playerEntries = new List<PlayerDataObject>
        {

        };
        
        var entries = new List<LeaderBoardEntryObject>()
        {
            new LeaderBoardEntryObject{ data = new List<Data>{ new Data(10, 1000)}, player = playerEntries  }
        }
        var leaderboardDto = new LeaderBoardDataObject
        {
            row = new List<LeaderBoardEntryObject>()
        };
        
        battleNetApiHttpClientMock
            .SetupSequence(a =>
                a.GetBnetApiResponseAsync<LeaderBoardDataObject>(It.IsAny<string>()))
            .ReturnsAsync()
            .ReturnsAsync()
            .ReturnsAsync();
    }
}