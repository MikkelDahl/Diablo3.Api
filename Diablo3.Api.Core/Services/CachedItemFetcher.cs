using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;

namespace Diablo3.Api.Core.Services;

internal class CachedItemFetcher : IItemFetcher
{
    private readonly IItemFetcher itemFetcher;
    private readonly ICache<string, Item> cache;

    public CachedItemFetcher(IItemFetcher itemFetcher, ICache<string, Item> cache)
    {
        this.itemFetcher = itemFetcher ?? throw new ArgumentNullException(nameof(itemFetcher));
        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<Item> GetAsync(string name)
    {
        var item = await cache.GetAsync(name);
        if (item is not null)
            return item;

        item = await itemFetcher.GetAsync(name);
        await cache.SetAsync(name, item);
        return item;
    }

    public async Task<ICollection<Item>> GetAsync(params string[] names)
    {
        var items = new List<Item>();
        foreach (var name in names)
        {
            var item = await cache.GetAsync(name);
            if (item is null)
            {
                item = await itemFetcher.GetAsync(name);
                await cache.SetAsync(name, item);
            }

            items.Add(item);
        }

        return items;
    }
}