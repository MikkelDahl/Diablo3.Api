using System;
using System.IO;
using System.Threading.Tasks;
using Diablo3.Api.Core;
using Diablo3.Api.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diablo3.Prototype
{
    class Program
    {
        private static IConfigurationRoot configuration;

        private static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var clientConfig = new ClientConfiguration(new CacheConfiguration(CacheOptions.Default));
            var clientId = configuration.GetSection("Credentials").GetSection("ClientId").Value;
            var clientSecret = configuration.GetSection("Credentials").GetSection("ClientSecret").Value;
            var client = new DiabloClientFactory(Region.EU, clientId, clientSecret, clientConfig).Build();

            var leaderBoard = await client.LeaderBoards.Normal.GetAsync(HeroClass.Wizard);
            var player = leaderBoard.GetHighestParagonPlayer();
            var player2 = leaderBoard.GetHighestRankedPlayer();
            Console.WriteLine($"Highest paragon: {player.BattleTag}, {player.Paragon}, {player.BattleTag}");
            Console.WriteLine($"Rank 1: {player2.BattleTag}, {player2.Paragon}, RiftLevel: {player2.RiftLevel}");
            var testItem = await client.Items.GetAsync("cain");
            Console.WriteLine(testItem.Name);
            var allItems = await client.Items.GetAllAsync();
            Console.WriteLine($"Fetched {allItems.Count} items");
            var wrathBoard = await client.LeaderBoards.Normal.GetForItemSetAsync(ItemSet.WhirlWind);
            var wrathBoard2 = await client.LeaderBoards.Normal.GetForItemSetAsync(ItemSet.WhirlWind);
            
            var wrathBoard3 = await client.LeaderBoards.Hardcore.GetForItemSetAsync(ItemSet.WhirlWind);
            var wrathBoard4 = await client.LeaderBoards.Hardcore.GetForItemSetAsync(ItemSet.WhirlWind);
            
            var wrathBoard5 = await client.LeaderBoards.Normal.GetForItemSetAsync(ItemSet.WhirlWind);
            // foreach (var entry in wrathBoard.Entries)
            // {
            //     Console.WriteLine(entry.RiftInformation.ClearDate + " - " + entry.LadderHero.BattleTag);
            // }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {

            configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).FullName, "..\\..\\..\\"))
                .AddJsonFile("appsettings.json", false)
                .Build();
            
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
            serviceCollection.AddTransient<Program>();
        }

 
    }
}




