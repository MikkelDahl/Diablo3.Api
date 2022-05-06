namespace Diablo3.Api.Core.Models.Cache;

internal interface ICache<TKey, TValue>
{
    Task<TValue> GetAsync(TKey key);
}