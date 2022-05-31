using Diablo3.Api.Core.Exceptions;
using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.DTOs;

namespace Diablo3.Api.Core.Services.Characters;

public class AccountFetcher : IAccountFetcher
{
    private readonly IBattleNetApiHttpClient battleNetApiHttpClient;

    public AccountFetcher(IBattleNetApiHttpClient battleNetApiHttpClient)
    {
        this.battleNetApiHttpClient = battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
    }

    public Account Get(string battleTag) => Task.Run(() => GetAsync(battleTag)).GetAwaiter().GetResult();

    public async Task<Account> GetAsync(string battleTag)
    {
        var request = CreateGetRequest(battleTag);
        try
        {
            var accountDto = await battleNetApiHttpClient.GetBnetApiResponseAsync<AccountDto>(request);
            return accountDto.ToAccount();
        }
        catch (Exception e)
        {
            throw new AccountNotFoundException(battleTag);
        }
    }

    private string CreateGetRequest(string battleTag)
    {
        var region = battleNetApiHttpClient.GetCurrentRegion();
        return $"https://{region.ToString().ToLower()}.api.blizzard.com/d3/profile/{battleTag}/?locale=en_US&access_token=USviYJ3cddgAhM2VCm87D10RBhlacVFj8m";
    }
}