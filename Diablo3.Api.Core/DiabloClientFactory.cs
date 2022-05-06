﻿using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Models.Cache;
using Diablo3.Api.Core.Services;
using Serilog;

namespace Diablo3.Api.Core
{
    public class DiabloClientFactory
    {
        private readonly IBattleNetApiHttpClient battleNetApiHttpClient;
        private readonly ISeasonIformationFetcher seasonIformationFetcher;
        private readonly ILogger logger;

        public DiabloClientFactory(Region region, string clientId, string clientSecret)
        {
            var credentials = new Credentials(clientId, clientSecret);
            battleNetApiHttpClient = new BattleNetApiHttpClient(credentials, region);
            seasonIformationFetcher = new SeasonIformationFetcher(battleNetApiHttpClient);
            
            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

        }

        public IClient Build(ClientConfiguration configuration)
        {
            var heroFetcher = BuildHeroFetcher(configuration);
            var currentSeason = seasonIformationFetcher.GetCurrentSeasonAsync().Result;
            return new DiabloClient(heroFetcher, configuration, battleNetApiHttpClient, logger, currentSeason);
        }

        private IHeroFetcher BuildHeroFetcher(ClientConfiguration configuration)
        {
            var heroFetcher = new HeroFetcher(battleNetApiHttpClient); 
            var cache = new Cache<int, Hero>(configuration.CacheConfiguration);
            return configuration.CacheConfiguration.Options == CacheOptions.NoCache
                ? heroFetcher
                : new CachedHeroFetcher(heroFetcher, cache);
        }
    }
}