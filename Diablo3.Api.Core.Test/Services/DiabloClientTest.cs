using System;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Services;
using Moq;
using NUnit.Framework;
using Serilog;

namespace Diablo3.Api.Core.Test.Services;

[TestFixture]
public class DiabloClientTest
{
    private Mock<IHeroFetcher> heroFetcherMock;
    private Mock<IItemCache> itemCacheMock;
    private Mock<ILogger> loggerMock;
    private Mock<IBattleNetApiHttpClient> battleNetApiHttpClientMock;
    private const int CurrentSeason = 10;

    [SetUp]
    public void Setup()
    {
        heroFetcherMock = new Mock<IHeroFetcher>();
        itemCacheMock = new Mock<IItemCache>();
        battleNetApiHttpClientMock = new Mock<IBattleNetApiHttpClient>();
        loggerMock = new Mock<ILogger>();
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void Constructor_throws_if_currentSeason_is_out_of_valid_range(int season)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DiabloClient(heroFetcherMock.Object,
            DefaultClientConfiguration.GetConfiguration(), battleNetApiHttpClientMock.Object, loggerMock.Object, season,
            itemCacheMock.Object));
    }


    private DiabloClient Sut() =>
        new(heroFetcherMock.Object, DefaultClientConfiguration.GetConfiguration(),
            battleNetApiHttpClientMock.Object, loggerMock.Object, CurrentSeason, itemCacheMock.Object);
}