using Diablo3.Api.Core.Models.DTOs;

namespace Diablo3.Api.Core.Services;

public class SeasonIformationFetcher : ISeasonIformationFetcher
{
    private readonly IBattleNetApiHttpClient battleNetApiHttpClient;

    public SeasonIformationFetcher(IBattleNetApiHttpClient battleNetApiHttpClient)
    {
        this.battleNetApiHttpClient = battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
    }

    public async Task<int> GetCurrentSeasonAsync()
    {
        const string request = "https://eu.api.blizzard.com/data/d3/season/?access_token=USf56m8BU5LNl13XSnu7x8c0EMNwprwNCB";
        var seasonDataObject = await battleNetApiHttpClient.GetBnetApiResponseAsync<SeasonDataObject>(request);

        return seasonDataObject.current_season;
    }
}