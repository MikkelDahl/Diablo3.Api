namespace Diablo3.Api.Core.Services
{
    public interface ILeaderBoardService
    {
        ILeaderBoardFetcher Normal { get; }
        ILeaderBoardFetcher Hardcore { get; }
    }
}