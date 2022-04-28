using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Extensions
{
    public static class LeaderBoardExtensions
    {
        public static bool Verify(this LeaderBoardEntry entry) => entry.RiftInformation.Level == entry.Hero.HighestGreaterRiftCompleted;
    }
}