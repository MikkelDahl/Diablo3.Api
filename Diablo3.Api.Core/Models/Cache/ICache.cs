namespace Diablo3.Api.Core.Models.Cache
{
    internal interface ICache<TKey, TValue>
    {
        TValue Get(TKey key);
        Task<TValue> GetAsync(TKey key);
        Task SetAsync(TKey key, TValue value);
    }
}