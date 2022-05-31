using System.Threading.Tasks;
using Diablo3.Api.Core.Exceptions;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.DTOs;
using Diablo3.Api.Core.Services;
using Diablo3.Api.Core.Services.Characters;
using Moq;
using NUnit.Framework;

namespace Diablo3.Api.Core.Test.Services
{
    [TestFixture]
    public class HeroFetcherTest
    {
        private Mock<IBattleNetApiHttpClient> battleNetApiHttpClientMock = new();
    
        [TearDown]
        public void TearDown()
        {
            battleNetApiHttpClientMock = new Mock<IBattleNetApiHttpClient>();
        }

        [Test]
        public void Constructor_throws_on_null_args()
        {
            TestHelper.AssertConstructorThrowsOnNullArgs<HeroFetcher>();
        }

        [Test]
        public async Task GetAsync_build_hero_from_dto()
        {
            var battleTag = "test#tag";
            var heroDto = new HeroDto
            {
                Id = 1,
                Class = "barbarian",
                HighestSoloRiftCompleted = 50,
                Name = "test",
                ParagonLevel = 100
            };
        
            battleNetApiHttpClientMock.Setup(a => a.GetCurrentRegion()).Returns(Region.EU);
            battleNetApiHttpClientMock.Setup(a => a.GetBnetApiResponseAsync<HeroDto>(It.IsAny<string>()))
                .ReturnsAsync(heroDto);

            var hero = await Sut().GetAsync(1, battleTag);
        
            Assert.Multiple(() =>
            {
                Assert.That(hero.Id, Is.EqualTo(heroDto.Id));
                Assert.That(hero.BattleTag, Is.EqualTo(battleTag));
                Assert.That(hero.Name, Is.EqualTo(heroDto.Name));
                Assert.That(hero.Paragon, Is.EqualTo(heroDto.ParagonLevel));
                Assert.That(hero.HighestGreaterRiftCompleted, Is.EqualTo(heroDto.HighestSoloRiftCompleted));
            });
        }
        
        [Test]
        public void GetAsync_rethrows_HeroNotFoundException_on_error()
        {
            battleNetApiHttpClientMock.Setup(a => a.GetCurrentRegion()).Returns(Region.EU);
            battleNetApiHttpClientMock.Setup(a => a.GetBnetApiResponseAsync<HeroDto>(It.IsAny<string>()))!
                .ReturnsAsync((HeroDto)null);

            Assert.ThrowsAsync<HeroNotFoundException>(async () => await Sut().GetAsync(1,  "test#tag"));
        }

        private HeroFetcher Sut() => new(battleNetApiHttpClientMock.Object);
    }
}