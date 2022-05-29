using Diablo3.Api.Core.Services;

namespace Diablo3.Api.Core
{
    internal class DiabloClient : IClient
    {
        public DiabloClient(ILeaderBoardService leaderBoardFetcher, IHeroFetcher heroFetcher, IItemFetcher itemCache)
        {
            Characters = heroFetcher ?? throw new ArgumentNullException(nameof(heroFetcher));
            Items = itemCache ?? throw new ArgumentNullException(nameof(itemCache));
            LeaderBoards = leaderBoardFetcher ?? throw new ArgumentNullException(nameof(leaderBoardFetcher));
        }

        public IHeroFetcher Characters { get; }
        public IItemFetcher Items { get; }
        public ILeaderBoardService LeaderBoards { get; }
    }
}