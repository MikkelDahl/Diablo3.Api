using Diablo3.Api.Core.Services;

namespace Diablo3.Api.Core
{
    internal class DiabloClient : IClient
    {
        internal DiabloClient(ILeaderBoardService leaderBoardFetcher, IHeroFetcher heroFetcher, IItemCache itemCache)
        {
            Characters = heroFetcher ?? throw new ArgumentNullException(nameof(heroFetcher));
            Items = itemCache ?? throw new ArgumentNullException(nameof(itemCache));
            LeaderBoards = leaderBoardFetcher ?? throw new ArgumentNullException(nameof(leaderBoardFetcher));
        }

        public IHeroFetcher Characters { get; }
        public IItemCache Items { get; }
        public ILeaderBoardService LeaderBoards { get; }
    }
}