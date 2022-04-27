using System;
using System.Linq;
using System.Threading.Tasks;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.DTOs;
using Diablo3.Api.Core.Services;
using Diablo3.Api.Core.Test.DtoBuilders;
using Moq;
using NUnit.Framework;

namespace Diablo3.Api.Core.Test;

public class DataFetcherTest
{
    private Mock<IBattleNetApiHttpClient> battleNetApiHttpClientMock;
    private DataFetcher sut;
    
    [SetUp]
    public void Setup()
    {
        battleNetApiHttpClientMock = new Mock<IBattleNetApiHttpClient>();
        battleNetApiHttpClientMock
            .Setup(a => a.GetBnetApiResponseAsync<LeaderBoardDataObject>(It.IsAny<string>()))
            .ReturnsAsync(LeaderBoardDataObjectBuilder.WithDefaultValues().Build());
        
        sut = new DataFetcher(battleNetApiHttpClientMock.Object);
    }
    
    [Test]
    public void Constructor_throws_on_null_params()
    {
        Assert.Throws<ArgumentNullException>(() => new DataFetcher(null));
    }

    [Test]
    public async Task GetLeaderBoardAsync_returns_entries_ordered_by_rift_level()
    {
        var leaderBoard = await sut.GetLeaderBoardAsync(PlayerClass.Barbarian);
        Assert.IsTrue(leaderBoard.Entries.All(e => e.RiftInformation.Level <= leaderBoard.Entries.First().RiftInformation.Level));
    }
    
    [Test]
    public async Task GetLeaderBoardAsync_returns_entries_secondary_ordered_by_clear_time()
    {
        
    }
}