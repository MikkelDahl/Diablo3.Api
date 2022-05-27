using Diablo3.Api.Core.Services;

namespace Diablo3.Api.Core
{
    public interface IClient
    {
        IHeroFetcher Characters { get; }
        IItemCache Items { get; }
        ILeaderBoardService LeaderBoards { get; }
    }
}