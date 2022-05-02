namespace Diablo3.Api.Core.Services;

public interface ISeasonIformationFetcher
{
    Task<int> GetCurrentSeasonAsync();
}