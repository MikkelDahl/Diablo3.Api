using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services.Characters;

public interface IAccountFetcher
{
    Account Get(string battleTag);
    Task<Account> GetAsync(string battleTag);
}