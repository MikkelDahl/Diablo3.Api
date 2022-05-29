using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;
using Diablo3.Api.Core.Models.DTOs;
using Serilog;

namespace Diablo3.Api.Core.Services
{
    internal class ItemFetcher : IItemFetcher
    {
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        private readonly ICache<string, ICollection<ItemBase>> cache;
        private readonly ILogger logger;

        public ItemFetcher(IBattleNetApiHttpClient battleNetApiHttpClient, ILogger logger)
        {
            this.battleNetApiHttpClient = battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Item> GetAsync(string name)
        {
            var baseItems = await cache.GetAsync("items");
            if (baseItems is null || !baseItems.Any())
            {
                baseItems = await GetBaseItemsAsync();
                await cache.SetAsync("items", baseItems);
            }

            var itemToFetch = baseItems.First(item => item.Name.Contains(name));
            var query = CreateItemQuery(itemToFetch.Path);
            var itemDto = await battleNetApiHttpClient.GetBnetApiResponseAsync<ItemDto>(query);
            return itemDto.ToItem();
        }

        public async Task<ICollection<Item>> GetAsync(params string[] names)
        {
            var baseItems = await cache.GetAsync("items");
            if (baseItems is null || !baseItems.Any())
            {
                baseItems = await GetBaseItemsAsync();
                await cache.SetAsync("items", baseItems);
            }

            var itemPaths = baseItems
                .Where(i => names.Any(n => i.Name.Contains(n)))
                .Select(i => i.Path);
            
            var queries = itemPaths.Select(CreateItemQuery);
            var queryTasks = queries
                .Select(q => battleNetApiHttpClient.GetBnetApiResponseAsync<ICollection<ItemDto>>(q))
                .ToList();
            
            try
            {
                await Task.WhenAll(queryTasks);
            }
            catch (Exception e)
            {
                logger.Error("Failed to get api response for item: {Message}",e.Message);
            }
            
            return queryTasks.Where(task => task.IsCompletedSuccessfully)
                .SelectMany(t => t.Result
                    .Select(i => i.ToItem()))
                .ToList();
        }
        
        private async Task<ICollection<ItemBase>> GetBaseItemsAsync()
        {
            var queries = ItemTypes.Select(CreateItemBaseQuery);
            var queryTasks = queries.Select(q => battleNetApiHttpClient.GetBnetApiResponseAsync<ICollection<ItemBaseDto>>(q)).ToList();
            try
            {
                await Task.WhenAll(queryTasks);
            }
            catch (Exception e)
            {
                logger.Error("Failed to get api response for item: {Message}",e.Message);
            }
            
            return queryTasks.Where(task => task.IsCompletedSuccessfully)
                .SelectMany(t => t.Result
                    .Where(item => item.Id.Contains("Unique"))
                    .Select(i => i.ToItemBase()))
                .ToList();
        }
        
        private static string CreateItemQuery(string itemPath) => 
            $"https://eu.api.blizzard.com/d3/data/{itemPath}?locale=en_US&access_token=USrytbc2GnIg6P2a9OP6PyQGJhd4P3VM6Y";


        private static string CreateItemBaseQuery(string itemType) => 
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
            "beltbarbarian",
            "helmbarbarian",
            "chestarmorbarbarian",
            "helmcrusader",
            "helmdemonhunter",
            "helmmonk",
            "helmwizard",
            "helmnecromancer",
            "helmwitchdoctor",
            "sword2h"
        };
    }
}