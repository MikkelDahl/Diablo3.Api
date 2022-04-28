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
        private Mock<IFetcher> dataFetcherMock = null!;

        [SetUp]
        public void Setup()
        {
            dataFetcherMock = new Mock<IFetcher>();
            sut = new DiabloClient(dataFetcherMock.Object);
        
            SetupDataFetcherMock();
        }

        [Test]
        public void Constructor_throws_on_null_args()
        {
            Assert.Throws<ArgumentNullException>(() => new DiabloClient(null));
        }

        [Test]
        public async Task GetForClassAsync_calls_fetcher_once()
        {
            var _ = await sut.GetForClassAsync(PlayerClass.Barbarian);
            dataFetcherMock.Verify(a => a.GetLeaderBoardAsync(It.IsAny<PlayerClass>(), It.IsAny<bool>()), Times.Once);
        }
    
        [Test]
        public async Task GetAllAsync_calls_fetcher_once_per_class()
        {
            var _ = await sut.GetAllAsync();
            dataFetcherMock.Verify(a => a.GetLeaderBoardAsync(It.IsAny<PlayerClass>(), It.IsAny<bool>()), Times.Exactly(7));

        }
    
        [Test]
        public async Task GetAllHardcoreAsync_calls_fetcher_once_per_class()
        {
            var _ = await sut.GetAllHardcoreAsync();
            dataFetcherMock.Verify(a => a.GetLeaderBoardAsync(It.IsAny<PlayerClass>(), It.IsAny<bool>()), Times.Exactly(7));

        }
    
        [Test]
        public async Task GetHardcoreForClassAsync_calls_fetcher_with_hardcore_param_set_to_true()
        {
            var _ = await sut.GetHardcoreForClassAsync(PlayerClass.Barbarian);
            dataFetcherMock.Verify(a => a.GetLeaderBoardAsync(It.IsAny<PlayerClass>(), It.Is<bool>(b => b == true)), Times.Once);

        }

        private void SetupDataFetcherMock()
        {
            var leaderBoardEntries = new List<LeaderBoardEntry>();

            dataFetcherMock.Setup(a =>
                    a.GetLeaderBoardAsync(It.IsAny<PlayerClass>(), It.IsAny<bool>()))
                .ReturnsAsync(new LeaderBoard(leaderBoardEntries));
        }
    }
}