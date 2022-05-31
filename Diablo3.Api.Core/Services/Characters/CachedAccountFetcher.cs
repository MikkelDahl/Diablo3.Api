using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;

namespace Diablo3.Api.Core.Services.Characters;

internal class CachedAccountFetcher : IAccountFetcher
{
    private readonly IAccountFetcher accountFetcher;
    private readonly ICache<string, Account> cache;

    public CachedAccountFetcher(IAccountFetcher accountFetcher, ICache<string, Account> cache)
    {
        this.accountFetcher = accountFetcher ?? throw new ArgumentNullException(nameof(accountFetcher));
        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public Account Get(string battleTag)
    {
        var cachedData = cache.Get(battleTag);
        if (cachedData is not null)
            return cachedData;

        var account = accountFetcher.Get(battleTag);
        cache.SetAsync(battleTag, account).RunSynchronously();

        return account;
    }

    public async Task<Account> GetAsync(string battleTag)
    {
        var cachedData = await cache.GetAsync(battleTag);
        if (cachedData is not null)
            return cachedData;

        var account = await accountFetcher.GetAsync(battleTag);
        await cache.SetAsync(battleTag, account);

        return account;
    }
}