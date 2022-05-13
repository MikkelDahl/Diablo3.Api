using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.DTOs;

namespace Diablo3.Api.Core.Services;

public class ItemFetcher : IItemFetcher
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
                .Select(i => new Item(i.Name, i.Icon)))
            .ToList();
    }

    private static string CreateQuery(string itemType) => 
        $"https://eu.api.blizzard.com/d3/data/item-type/{itemType}?locale=en_GB&access_token=USrytbc2GnIg6P2a9OP6PyQGJhd4P3VM6Y";

    private List<string> ItemTypes => new List<string>
    {
        "helm",
        "gloves",
        "legs"
    };
}