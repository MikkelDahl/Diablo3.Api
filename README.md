# Diablo3.Api
[![build](https://github.com/MikkelDahl/Diablo3.Api/actions/workflows/build.yml/badge.svg)](https://github.com/MikkelDahl/Diablo3.Api/actions/workflows/build.yml)
[![tests](https://github.com/MikkelDahl/Diablo3.Api/actions/workflows/tests.yml/badge.svg)](https://github.com/MikkelDahl/Diablo3.Api/actions/workflows/tests.yml)

Diablo3.Api is a C# .Net library for the [Diablo 3 API](https://develop.battle.net/documentation/diablo-3/game-data-apis). Information on how to connect to blizzards API can be found here: [Get started with Blizzard API](https://develop.battle.net/documentation/guides/getting-started). Note that all leaderboard data is seasonal only. There are currently no plans to support non-seasonal leaderboards.

# Examples
## Getting the highest ranked player of [class]
```
var configuration = DefaultClientConfiguration.GetConfiguration();
var client = new DiabloClientFactory(Region.EU, "[CLientId]", "[ClientSecret]").Build(configuration);
var demonHunterSeasonalLeaderBoard = await client.GetForClassAsync(PlayerClass.DemonHunter);
var highestRankedDemonHunter = demonHunterSeasonalLeaderBoard.Entries.First();
Console.WriteLine($"Highest Ranked DemonHunter: {highestRankedDemonHunter.Player.Name}, RiftLevel: {highestRankedDemonHunter.RiftInformation.Level}{highestRankedDemonHunter.Player.PlayerClass}");
``` 

## Caching
The default cache implementation will cache data per request with a TTL of 100 seconds. Caching currently supports three modes: Default, Preload & NoCache:
```
var configuration = new ClientConfiguration(new CacheConfiguration(CacheOptions.NoCache)); //cache disabled
var configuration = new ClientConfiguration(new CacheConfiguration(CacheOptions.Preload)); //initialize cache before making any requests
var configuration = DefaultClientConfiguration.GetConfiguration(); //default CacheOption with CacheTtl set to 100s
``` 
