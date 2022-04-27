using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    public interface IBattleNetApiHttpClient
    {
        Task<T> GetBnetApiResponseAsync<T>(string request);
        Task<AccessToken> CreateAccessTokenAsync(Region region, string clientId, string clientSecret);
        Region GetCurrentRegion();
    }
};

