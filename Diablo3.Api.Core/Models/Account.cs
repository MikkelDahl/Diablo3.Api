namespace Diablo3.Api.Core.Models;

public record Account(string BattleTag, List<Hero> Heroes, Dictionary<HeroClass, TimeSpan> TimePlayedInCurrentSeason)
{ }