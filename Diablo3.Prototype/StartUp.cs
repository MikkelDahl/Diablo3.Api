using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diablo3.Prototype
{
    public static class StartUp
    {
        private static void ConfigureServices(IServiceCollection serviceCollection)
        {

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
            serviceCollection.AddTransient<Program>();
        }
    }
}