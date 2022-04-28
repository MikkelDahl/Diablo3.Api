namespace Diablo3.Api.Core.Models
{
    public class LeaderBoardEntry
    {
        public LeaderBoardEntry(Hero hero, RiftInformation riftInformation)
        {
            Hero = hero;
            RiftInformation = riftInformation;
        }

        public Hero Hero { get; }
        public RiftInformation RiftInformation { get; }
        
    }
}