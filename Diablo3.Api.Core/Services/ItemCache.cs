using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;

namespace Diablo3.Api.Core.Services;

internal class ItemCache : IItemCache
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
        var items = await cache.GetAsync("items");
        if (items is not null && items.Any()) 
            return items.First(item => item.Name.ToLower().Contains(name.ToLower()));
        
        items = await itemFetcher.GetAllItemsAsync();
        await cache.SetAsync("items", items);

        return items.First(item => item.Name.ToLower().Contains(name.ToLower()));
    }

    public async Task<ICollection<Item>> GetAllAsync()
    {
        var items = await cache.GetAsync("items");
        if (items is not null && items.Any()) 
            return items;
        
        items = await itemFetcher.GetAllItemsAsync();
        await cache.SetAsync("items", items);
        return items;
    }
}