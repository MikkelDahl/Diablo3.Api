using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.DTOs;
using Diablo3.Api.Core.Services;
using Moq;
using NUnit.Framework;

namespace Diablo3.Api.Core.Test
{
    [TestFixture]
    public class LeaderBoardFetcherTest
    {
        private Mock<IBattleNetApiHttpClient> battleNetApiHttpClientMock;
        private LeaderBoardFetcher sut;

        [SetUp]
        public void Setup()
        {
            battleNetApiHttpClientMock = new Mock<IBattleNetApiHttpClient>();
        }

        [Test]
        public void Constructor_throws_on_null_args()
        {
            Assert.Throws<ArgumentNullException>(() => new NormalLeaderBoardFetcher(null, 1));
        }


        [Test]
        public async Task GetLeaderBoardAsync_gets_top_players_across_itemsets()
        {
            var leaderboardDto1 = GetleaderBoardDto("a", 10, 100);
            var leaderboardDto2 = GetleaderBoardDto("b", 11, 20);
            var leaderboardDto3 = GetleaderBoardDto("c", 12, 150);

            battleNetApiHttpClientMock
                .SetupSequence(a =>
                    a.GetBnetApiResponseAsync<LeaderBoardDataObject>(It.IsAny<string>()))
                .ReturnsAsync(leaderboardDto1)
                .ReturnsAsync(leaderboardDto2)
                .ReturnsAsync(leaderboardDto3)
                .ReturnsAsync(leaderboardDto1)
                .ReturnsAsync(leaderboardDto2)
                .ReturnsAsync(leaderboardDto3)
                .ReturnsAsync(leaderboardDto2);

            sut = new NormalLeaderBoardFetcher(battleNetApiHttpClientMock.Object, 1);
            var actual = await sut.GetLeaderBoardAsync(HeroClass.Barbarian);

            Assert.Multiple(() =>
            {
                Assert.That(actual.GetHighestRankedPlayer().RiftLevel, Is.EqualTo(150));
                Assert.That(actual.Entries.Last().LadderHero.RiftLevel, Is.EqualTo(10));
                Assert.That(actual.Entries.First().RiftInformation.ItemSet, Is.EqualTo(ItemSet.MoTE));
                Assert.That(actual.Entries.Last().RiftInformation.ItemSet, Is.EqualTo(ItemSet.WhirlWind));
            });
        }

        private static LeaderBoardDataObject GetleaderBoardDto(string idOffset, int minRiftLevel, int maxRiftLevel)
        {
            var data1 = new Data { id = idOffset + "123", number = minRiftLevel, String = "test", timestamp = 10000 };
            var data2 = new Data { id = idOffset + "1234", number = maxRiftLevel, String = "wat", timestamp = 10000 };
            var firstPlayerEntries = new List<PlayerDataObject>
            {
                new()
                {
                    data = new List<Data>
                    {
                        data1,
                        data1,
                        data1,
                        data1,
                        data1,
                        data1,
                        data1,
                        data1,
                        data1,
                        data1,
                    }
                }
            };

            var secondPlayerEntries = new List<PlayerDataObject>
            {
                new()
                {
                    data = new List<Data>
                    {
                        data2,
                        data2,
                        data2,
                        data2,
                        data2,
                        data2,
                        data2,
                        data2,
                        data2,
                        data2
                    }
                }
            };

            var entries = new List<LeaderBoardEntryObject>()
            {
                new LeaderBoardEntryObject
                    { data = new List<Data> { data1, data1, data1, data1 }, player = firstPlayerEntries },
                new LeaderBoardEntryObject
                    { data = new List<Data> { data2, data2, data2, data2 }, player = secondPlayerEntries }
            };

            return new LeaderBoardDataObject { row = entries };
        }
    }
}