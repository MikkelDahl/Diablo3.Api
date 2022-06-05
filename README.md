# Diablo3.Api
[![build](https://github.com/MikkelDahl/Diablo3.Api/actions/workflows/build.yml/badge.svg)](https://github.com/MikkelDahl/Diablo3.Api/actions/workflows/build.yml)
[![tests](https://github.com/MikkelDahl/Diablo3.Api/actions/workflows/tests.yml/badge.svg)](https://github.com/MikkelDahl/Diablo3.Api/actions/workflows/tests.yml)
[![version](https://img.shields.io/badge/latest-1.0.0-orange)]()
[![.Net](https://img.shields.io/badge/.Net-6.0.x-red)](https://versionsof.net/)

Diablo3.Api is a C# .Net library for the [Diablo 3 API](https://develop.battle.net/documentation/diablo-3/game-data-apis). Information on how to connect to blizzards API can be found here: [Get started with Blizzard API](https://develop.battle.net/documentation/guides/getting-started). Note that all leaderboard data is seasonal and current season only. There are currently no plans to support non-seasonal leaderboards or older seasons.

# Examples
## Initializing an instance of IClient
Creating a new instance of the DiabloClient is done through the Client Factory:
```
var client = await new DiabloClientFactory(Region.EU, "[CLientId]", "[ClientSecret]").BuildAsync();
``` 

## Getting the highest ranked player of [class]
```
var leaderBoard = await client.LeaderBoards.Normal.GetAsync(HeroClass.Barbarian);
var highestRankedPlayer = leaderBoard.GetHighestRankedPlayer();
var highestRankedPlayerHero = await client.Characters.GetAsync(highestRankedPlayer.Id, highestRankedPlayer.BattleTag);
Console.WriteLine("Highest ranked player:{0}", highestRankedPlayerHero);
``` 

## Getting the strongest/most popular set for [class]
```
var leaderBoard = await client.LeaderBoards.Normal.GetAsync(PlayerClass.Barbarian);
var mostPopularSet = leaderBoard.GetMostPopularSet();
Console.WriteLine($"Most popular set: {leaderBoard.GetMostPopularSet().Set} - {leaderBoard.GetMostPopularSet().Count}/{leaderBoard.Entries.Count}");
``` 

## Getting the icon/image of an item
```
 var item = await client.Items.GetAsync("tasker"); //Partial keyword will fetch the first matching item containing the keyword
 var icon = item.IconUri;
``` 

## Caching
The default cache implementation will cache data per request with a TTL of 120 seconds. Caching currently supports three modes: Default, Preload & NoCache:
```
var configuration = new ClientConfiguration(new CacheConfiguration(CacheOptions.NoCache)); //cache disabled
var configuration = new ClientConfiguration(new CacheConfiguration(CacheOptions.Preload)); //initialize cache before making any requests
var configuration = DefaultClientConfiguration.GetConfiguration(); //default CacheOption with CacheTtl set to 100s
``` 
Note that preloading the cache will only preload leaderboards and items. Account/Hero data is still cached per request.
