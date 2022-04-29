using System;
using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Test.Builders
{
    public class LeaderBoardEntryBuilder
    {
        private LadderHero ladderHero;
        private RiftInformation riftInformation;
    
        private LeaderBoardEntryBuilder(LadderHero ladderHero, RiftInformation riftInformation)
        {
            this.ladderHero = ladderHero;
            this.riftInformation = riftInformation;
        }

        public static LeaderBoardEntryBuilder WithDefaultValues() =>
            new LeaderBoardEntryBuilder(
                new LadderHero(111111, "1234", HeroClass.Barbarian, 1000, 100),
                new RiftInformation(100, TimeSpan.FromMinutes(10), DateTime.UtcNow, ItemSet.Raekor)
            );
    
        public LeaderBoardEntryBuilder WithRiftInformation(RiftInformation riftInformation) =>
            new LeaderBoardEntryBuilder(ladderHero, riftInformation);
    
        public LeaderBoardEntryBuilder WithPlayer(LadderHero ladderHero) =>
            new LeaderBoardEntryBuilder(ladderHero, riftInformation);

        public LeaderBoardEntry Build() => new LeaderBoardEntry(ladderHero, riftInformation);
    }
}