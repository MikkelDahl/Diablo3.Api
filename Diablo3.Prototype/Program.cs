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
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var clientConfig = new ClientConfiguration(new CacheConfiguration(CacheOptions.Preload));
            var clientId = configuration.GetSection("Credentials").GetSection("ClientId").Value;
            var clientSecret = configuration.GetSection("Credentials").GetSection("ClientSecret").Value;
            IClient client = new DiabloClientFactory(Region.EU, clientId, clientSecret).Build(clientConfig);

            LeaderBoard leaderBoard = await client.GetLeaderBoardForClassAsync(HeroClass.Wizard);
            LadderHero player = leaderBoard.GetHighestParagonPlayer();
            LadderHero player2 = leaderBoard.GetHighestRankedPlayer();
            Console.WriteLine($"Highest paragon: {player.BattleTag}, {player.Paragon}, {player.BattleTag}");
            Console.WriteLine($"Rank 1: {player2.BattleTag}, {player2.Paragon}, RiftLevel: {player2.RiftLevel}");
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




