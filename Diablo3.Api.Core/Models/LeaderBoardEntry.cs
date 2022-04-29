namespace Diablo3.Api.Core.Models
{
    public class LeaderBoardEntry
    {
        public LeaderBoardEntry(LadderHero ladderHero, RiftInformation riftInformation)
        {
            LadderHero = ladderHero;
            RiftInformation = riftInformation;
        }

        public LadderHero LadderHero { get; }
        public RiftInformation RiftInformation { get; }
        
    }
}