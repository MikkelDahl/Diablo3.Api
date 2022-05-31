using Diablo3.Api.Core.Services;
using Diablo3.Api.Core.Services.Characters;

namespace Diablo3.Api.Core
{
    public interface IClient
    {
        IAccountFetcher Accounts { get; }
        IHeroFetcher Characters { get; }
        IItemFetcher Items { get; }
        ILeaderBoardService LeaderBoards { get; }
    }
}