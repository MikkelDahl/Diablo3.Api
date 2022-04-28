using System;
using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Test.Builders
{
    public class LeaderBoardEntryBuilder
    {
        private Hero hero;
        private RiftInformation riftInformation;
    
        private LeaderBoardEntryBuilder(Hero hero, RiftInformation riftInformation)
        {
            this.hero = hero;
            this.riftInformation = riftInformation;
        }

        public static LeaderBoardEntryBuilder WithDefaultValues() =>
            new LeaderBoardEntryBuilder(
                new Hero(111111, "TestName", "1234", HeroClass.Barbarian, 1000, 100),
                new RiftInformation(100, TimeSpan.FromMinutes(10), DateTime.UtcNow, ItemSet.Raekor)
            );
    
        public LeaderBoardEntryBuilder WithRiftInformation(RiftInformation riftInformation) =>
            new LeaderBoardEntryBuilder(hero, riftInformation);
    
        public LeaderBoardEntryBuilder WithPlayer(Hero hero) =>
            new LeaderBoardEntryBuilder(hero, riftInformation);

        public LeaderBoardEntry Build() => new LeaderBoardEntry(hero, riftInformation);
    }
}