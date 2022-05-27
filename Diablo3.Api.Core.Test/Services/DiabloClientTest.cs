using System;
using Diablo3.Api.Core.Services;
using Moq;
using NUnit.Framework;

namespace Diablo3.Api.Core.Test.Services
{
    [TestFixture]
    public class DiabloClientTest
    {
        private Mock<IHeroFetcher> heroFetcherMock;
        private Mock<ILeaderBoardService> leaderBoardServiceMock;
        private Mock<IItemCache> itemCacheMock;

        [SetUp]
        public void Setup()
        {
            leaderBoardServiceMock = new Mock<ILeaderBoardService>(); 
            heroFetcherMock = new Mock<IHeroFetcher>();
            itemCacheMock = new Mock<IItemCache>();
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Constructor_throws_if_currentSeason_is_out_of_valid_range(int season)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DiabloClient(leaderBoardServiceMock.Object,
                heroFetcherMock.Object, itemCacheMock.Object));
        }


        private DiabloClient Sut() =>
            new(leaderBoardServiceMock.Object, heroFetcherMock.Object, itemCacheMock.Object);
    }
}