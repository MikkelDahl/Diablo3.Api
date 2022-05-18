using System;
using System.Threading.Tasks;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.DTOs;
using Diablo3.Api.Core.Services;
using Moq;
using NUnit.Framework;

namespace Diablo3.Api.Core.Test.Services;

[TestFixture]
public class HeroFetcherTest
{
    private Mock<IBattleNetApiHttpClient> battleNetApiHttpClientMock;
    
    [SetUp]
    public void Setup()
    {
        battleNetApiHttpClientMock = new Mock<IBattleNetApiHttpClient>();
    }

    [Test]
    public void Constructor_throws_on_null_args()
    {
        Assert.Throws<ArgumentNullException>(() => new HeroFetcher(null));
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

    private HeroFetcher Sut() => new(battleNetApiHttpClientMock.Object);
}