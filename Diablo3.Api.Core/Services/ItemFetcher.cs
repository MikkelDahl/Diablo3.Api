using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.DTOs;
using Serilog;

namespace Diablo3.Api.Core.Services
{
    internal class ItemFetcher : IItemFetcher
    {
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        private readonly ILogger logger;

        public ItemFetcher(IBattleNetApiHttpClient battleNetApiHttpClient, ILogger logger)
        {
            this.battleNetApiHttpClient = battleNetApiHttpClient ?? throw new ArgumentNullException(nameof(battleNetApiHttpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ICollection<Item>> GetAllItemsAsync()
        {
            var queries = ItemTypes.Select(CreateQuery);
            var queryTasks = queries.Select(q => battleNetApiHttpClient.GetBnetApiResponseAsync<ICollection<ItemDto>>(q)).ToList();
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
            "beltbarbarian",
            "helmbarbarian",
            "chestarmorbarbarian",
            "helmcrusader",
            "helmdemonhunter",
            "helmmonk",
            "helmwizard",
            "helmnecromancer",
            "helmwitchdoctor"
        };
    }
}