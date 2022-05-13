using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;

namespace Diablo3.Api.Core.Services;

internal class ItemCache
{
    private readonly IItemFetcher itemFetcher;
    private readonly ICache<string, ICollection<Item>> cache;

    public ItemCache(IItemFetcher itemFetcher, ICache<string, ICollection<Item>> cache)
    {
        this.itemFetcher = itemFetcher ?? throw new ArgumentNullException(nameof(itemFetcher));
        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<Item> GetAsync(string name)
    {
        var allItems = await cache.GetAsync("items");
        if (!allItems.Any()) 
            allItems = await itemFetcher.GetAllItemsAsync();

        return allItems.First(item => item.Name.ToLower().Contains(name.ToLower()));
    }
}