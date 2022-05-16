using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.DTOs;

namespace Diablo3.Api.Core.Services;

internal class ItemFetcher : IItemFetcher
{
    private readonly IBattleNetApiHttpClient battleNetApiHttpClient;

    public ItemFetcher(IBattleNetApiHttpClient battleNetApiHttpClient)
    {
        this.battleNetApiHttpClient =
            battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
    }

    public async Task<ICollection<Item>> GetAllItemsAsync()
    {
        var queries = ItemTypes.Select(CreateQuery);
        var queryTasks = queries.Select(q => battleNetApiHttpClient.GetBnetApiResponseAsync<ICollection<ItemDto>>(q)).ToList();
        await Task.WhenAll(queryTasks);
        
        return queryTasks
            .SelectMany(t => t.Result
                .Where(item => item.Id.Contains("Unique"))
                .Select(i => i.ToItem()))
            .ToList();
    }

    private static string CreateQuery(string itemType) => 
        $"https://eu.api.blizzard.com/d3/data/item-type/{itemType}?locale=en_GB&access_token=USrytbc2GnIg6P2a9OP6PyQGJhd4P3VM6Y";

    private static IEnumerable<string> ItemTypes => new List<string>
    {
        "helm",
        "gloves",
        "legs",
        "boots",
        "shoulders",
        "chestarmor",
        "amulet",
        "ring",
        "sword",
        "axe",
        "spear",
        "shield",
        "staff",
        "wand",
        "dagger",
        "beltbarbarian"
    };
}