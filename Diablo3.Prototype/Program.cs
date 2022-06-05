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
            var client = await new DiabloClientFactory(Region.EU, clientId, clientSecret, clientConfig).BuildAsync();

          
            var leaderBoard = await client.LeaderBoards.Normal.GetAsync(HeroClass.Barbarian);
            var highestRankedPlayer = leaderBoard.GetHighestRankedPlayer();
            var account = await client.Accounts.GetAsync(highestRankedPlayer.BattleTag);
            Console.WriteLine("Account: " + account.BattleTag);
            Console.WriteLine("Playtime: " + account.TimePlayedInCurrentSeason[HeroClass.Barbarian]);
            Console.WriteLine("Playtime: " + account.TimePlayedInCurrentSeason[HeroClass.DemonHunter]);
            Console.WriteLine("Playtime: " + account.TimePlayedInCurrentSeason[HeroClass.Monk]);
            Console.WriteLine("Playtime: " + account.TimePlayedInCurrentSeason[HeroClass.Wizard]);
            var testItem = await client.Items.GetAsync("p");
            Console.WriteLine(testItem.Name + " - " + testItem.Effect);
            var wrathBoard = await client.LeaderBoards.Normal.GetAsync(ItemSet.WhirlWind);

            // foreach (var entry in wrathBoard.Entries)
            // {
            //     Console.WriteLine("GR: " + entry.RiftInformation.Level + " - " + entry.RiftInformation.ClearDate + " - " + entry.LadderHero.BattleTag);
            // }

            var hero = await client.Characters.GetAsync(wrathBoard.GetHighestRankedPlayer().Id,
                wrathBoard.GetHighestRankedPlayer().BattleTag);
            
            Console.WriteLine(hero.Name + $" GR: {hero.HighestGreaterRiftCompleted} - " + hero.BattleTag);
            Console.WriteLine($"Most popular set: {leaderBoard.GetMostPopularSet().Set} - {leaderBoard.GetMostPopularSet().Count}/1000");
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




