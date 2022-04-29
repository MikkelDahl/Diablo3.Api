using System.Net.Http.Headers;
using System.Net.Http.Json;
using Diablo3.Api.Core.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Diablo3.Api.Core.Services
{
    internal class BattleNetApiHttpClient : IBattleNetApiHttpClient
{
    private AccessToken accessToken;
    private readonly string clientId;
    private readonly string clientSecret;
    private readonly Region region;

    public BattleNetApiHttpClient(Credentials credentials, Region region)
    {
        clientId = credentials.ClientId ?? throw new ArgumentNullException(nameof(clientId));
        clientSecret = credentials.Secret ?? throw new ArgumentNullException(nameof(clientSecret));
        this.region = region;
    }

    public async Task<T> GetBnetApiResponseAsync<T>(string request)
    {
        var httpClient = new HttpClient();
        var token = await GetNewIfExpired(accessToken);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
        var response = await httpClient.GetFromJsonAsync<T>(request);

        return response;
    }
    
    public async Task<string> GetBnetApiStringResponseAsync(string request)
    {
        var httpClient = new HttpClient();
        var token = await GetNewIfExpired(accessToken);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
        var response = await httpClient.GetStringAsync(request);

        return response;
    }
    
    public async Task<AccessToken> CreateAccessTokenAsync(Region region, string clientId, string clientSecret)
    {
        var regionString = region.ToString();
        var client = new RestClient($"https://{regionString}.battle.net/oauth/token");
        var request = new RestRequest(Method.POST);
        request.AddHeader("cache-control", "no-cache");
        request.AddHeader("content-type", "application/x-www-form-urlencoded");
        request.AddParameter("application/x-www-form-urlencoded",
            $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}",
            ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);

        return JsonConvert.DeserializeObject<AccessToken>(response.Content);
    }

    public Region GetCurrentRegion() => region;

    private async Task<AccessToken> GetNewIfExpired(AccessToken? token)
    {
        if (token is null || token.Expiration < 1000)
        {
            accessToken = await RequestNewAuthorizationToken();
            return accessToken;
        }

        return token;
    }

    private async Task<AccessToken> RequestNewAuthorizationToken()
    {
        var regionAsString = region.ToString().ToLower();
        var client = new RestClient($"https://{regionAsString}.battle.net/oauth/token");
        var request = new RestRequest(Method.POST);
        request.AddHeader("cache-control", "no-cache");
        request.AddHeader("content-type", "application/x-www-form-urlencoded");
        request.AddParameter("application/x-www-form-urlencoded",
            $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}",
            ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);

        return JsonConvert.DeserializeObject<AccessToken>(response.Content);
    }
}
};

