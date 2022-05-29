namespace Diablo3.Api.Core.Services
{
    public class LeaderBoardService : ILeaderBoardService
    {

        public LeaderBoardService(ILeaderBoardFetcher normalLeaderBoardFetcher, ILeaderBoardFetcher hardcoreLeaderBoardFetcher)
        {
            Normal = normalLeaderBoardFetcher ?? throw new ArgumentNullException(nameof(normalLeaderBoardFetcher));
            Hardcore = hardcoreLeaderBoardFetcher ?? throw new ArgumentNullException(nameof(hardcoreLeaderBoardFetcher));
        }

        public ILeaderBoardFetcher Normal { get; }

        public ILeaderBoardFetcher Hardcore { get; }
    }
}