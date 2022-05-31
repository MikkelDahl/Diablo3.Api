using Diablo3.Api.Core.Services;
using Diablo3.Api.Core.Services.Characters;

namespace Diablo3.Api.Core
{
    internal class DiabloClient : IClient
    {
        public DiabloClient(ILeaderBoardService leaderBoardFetcher, IHeroFetcher heroFetcher, IItemFetcher itemCache, IAccountFetcher accounts)
        {
            Characters = heroFetcher ?? throw new ArgumentNullException(nameof(heroFetcher));
            Items = itemCache ?? throw new ArgumentNullException(nameof(itemCache));
            Accounts = accounts ?? throw new ArgumentNullException(nameof(accounts));
            LeaderBoards = leaderBoardFetcher ?? throw new ArgumentNullException(nameof(leaderBoardFetcher));
        }

        public IAccountFetcher Accounts { get; }
        public IHeroFetcher Characters { get; }
        public IItemFetcher Items { get; }
        public ILeaderBoardService LeaderBoards { get; }
    }
}