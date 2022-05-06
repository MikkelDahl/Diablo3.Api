using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Services;
using Moq;
using NUnit.Framework;
using Serilog;

namespace Diablo3.Api.Core.Test
{
    [TestFixture]
    public class DiabloClientTest
    {
        private DiabloClient sut = null!;
        private Mock<ILeaderBoardFetcher> leaderBoardFetcherMock = null!;
        private Mock<IBattleNetApiHttpClient> battleNetApiHttpClientMock = null!;
        private Mock<ILogger> loggerMock = null!;
        private Mock<IHeroFetcher> heroFetcherMock = null!;

        [SetUp]
        public void Setup()
        {
            leaderBoardFetcherMock = new Mock<ILeaderBoardFetcher>();
            heroFetcherMock = new Mock<IHeroFetcher>();
            battleNetApiHttpClientMock = new Mock<IBattleNetApiHttpClient>();
            loggerMock = new Mock<ILogger>();
            sut = new DiabloClient(heroFetcherMock.Object,
                new ClientConfiguration(new CacheConfiguration(CacheOptions.Default)),
                battleNetApiHttpClientMock.Object, loggerMock.Object, 1);

            SetupDataFetcherMock();
        }

        [Test]
        public async Task GetForClassAsync_calls_fetcher_once()
        {
            var _ = await sut.GetLeaderBoardForClassAsync(HeroClass.Barbarian);
            leaderBoardFetcherMock.Verify(a => a.GetLeaderBoardAsync(It.IsAny<HeroClass>()),
                Times.Once);
        }

        [Test]
        public async Task GetAllAsync_calls_fetcher_once_per_class()
        {
            var _ = await sut.GetAllAsync();
            leaderBoardFetcherMock.Verify(a => a.GetLeaderBoardAsync(It.IsAny<HeroClass>()),
                Times.Exactly(7));
        }

        [Test]
        public async Task GetAllHardcoreAsync_calls_fetcher_once_per_class()
        {
            var _ = await sut.GetAllHardcoreAsync();
            leaderBoardFetcherMock.Verify(a => a.GetLeaderBoardAsync(It.IsAny<HeroClass>()),
                Times.Exactly(7));
        }

        [Test]
        public async Task GetHardcoreForClassAsync_calls_fetcher_with_hardcore_param_set_to_true()
        {
            var _ = await sut.GetHardcoreLeaderBoardForClassAsync(HeroClass.Barbarian);
            leaderBoardFetcherMock.Verify(
                a => a.GetLeaderBoardAsync(It.IsAny<HeroClass>()), Times.Once);
        }

        private void SetupDataFetcherMock()
        {
            var leaderBoardEntries = new List<LeaderBoardEntry>();

            leaderBoardFetcherMock.Setup(a =>
                    a.GetLeaderBoardAsync(It.IsAny<HeroClass>()))
                .ReturnsAsync(new LeaderBoard(leaderBoardEntries));
        }
    }
}