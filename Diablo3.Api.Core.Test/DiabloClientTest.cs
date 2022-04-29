using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Services;
using Moq;
using NUnit.Framework;

namespace Diablo3.Api.Core.Test
{
    [TestFixture]
    public class DiabloClientTest
    {
        private DiabloClient sut = null!;
        private Mock<ILeaderBoardFetcher> leaderBoardFetcherMock = null!;
        private Mock<IHeroFetcher> heroFetcherMock = null!;

        [SetUp]
        public void Setup()
        {
            leaderBoardFetcherMock = new Mock<ILeaderBoardFetcher>();
            heroFetcherMock = new Mock<IHeroFetcher>();
            sut = new DiabloClient(leaderBoardFetcherMock.Object, heroFetcherMock.Object);
        
            SetupDataFetcherMock();
        }

        [Test]
        public void Constructor_throws_on_null_args()
        {
            Assert.Throws<ArgumentNullException>(() => new DiabloClient(null, heroFetcherMock.Object));
            Assert.Throws<ArgumentNullException>(() => new DiabloClient(leaderBoardFetcherMock.Object, null));
        }

        [Test]
        public async Task GetForClassAsync_calls_fetcher_once()
        {
            var _ = await sut.GetForClassAsync(HeroClass.Barbarian);
            leaderBoardFetcherMock.Verify(a => a.GetLeaderBoardAsync(It.IsAny<HeroClass>(), It.IsAny<bool>()), Times.Once);
        }
    
        [Test]
        public async Task GetAllAsync_calls_fetcher_once_per_class()
        {
            var _ = await sut.GetAllAsync();
            leaderBoardFetcherMock.Verify(a => a.GetLeaderBoardAsync(It.IsAny<HeroClass>(), It.IsAny<bool>()), Times.Exactly(7));

        }
    
        [Test]
        public async Task GetAllHardcoreAsync_calls_fetcher_once_per_class()
        {
            var _ = await sut.GetAllHardcoreAsync();
            leaderBoardFetcherMock.Verify(a => a.GetLeaderBoardAsync(It.IsAny<HeroClass>(), It.IsAny<bool>()), Times.Exactly(7));

        }
    
        [Test]
        public async Task GetHardcoreForClassAsync_calls_fetcher_with_hardcore_param_set_to_true()
        {
            var _ = await sut.GetHardcoreForClassAsync(HeroClass.Barbarian);
            leaderBoardFetcherMock.Verify(a => a.GetLeaderBoardAsync(It.IsAny<HeroClass>(), It.Is<bool>(b => b == true)), Times.Once);

        }

        private void SetupDataFetcherMock()
        {
            var leaderBoardEntries = new List<LeaderBoardEntry>();

            leaderBoardFetcherMock.Setup(a =>
                    a.GetLeaderBoardAsync(It.IsAny<HeroClass>(), It.IsAny<bool>()))
                .ReturnsAsync(new LeaderBoard(leaderBoardEntries));
        }
    }
}