namespace Diablo3.Api.Core.Models.DTOs;

internal class AccountDto
{
    public string BattleTag { get; set; }
    public int ParagonLevel { get; set; }
    public int ParagonLevelHardcore { get; set; }
    public int ParagonLevelSeason { get; set; }
    public int ParagonLevelSeasonHardcore { get; set; }
    public List<HeroDto> Heroes { get; set; }
    public SeasonalProfilesObject SeasonalProfiles { get; set; }

    public Account ToAccount()
    {
        var heroes = Heroes.Select(h => h.ToHero(BattleTag)).ToList();
        var timePlayed = new Dictionary<HeroClass, TimeSpan>();
        return new Account(BattleTag, heroes, timePlayed);
    }
}