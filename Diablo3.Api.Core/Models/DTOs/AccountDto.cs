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
        var timePlayed = new Dictionary<HeroClass, TimeSpan>
        {
            { HeroClass.Barbarian, TimeSpan.FromHours(SeasonalProfiles.Season26.TimePlayed.Barbarian * 3000) },
            { HeroClass.Crusader, TimeSpan.FromHours(SeasonalProfiles.Season26.TimePlayed.Crusader * 3000) },
            { HeroClass.DemonHunter, TimeSpan.FromHours(SeasonalProfiles.Season26.TimePlayed.DemonHunter * 3000) },
            { HeroClass.Monk, TimeSpan.FromHours(SeasonalProfiles.Season26.TimePlayed.Monk * 3000) },
            { HeroClass.Necromancer, TimeSpan.FromHours(SeasonalProfiles.Season26.TimePlayed.Necromancer * 3000) },
            { HeroClass.WitchDoctor, TimeSpan.FromHours(SeasonalProfiles.Season26.TimePlayed.WitchDoctor * 3000) },
            { HeroClass.Wizard, TimeSpan.FromHours(SeasonalProfiles.Season26.TimePlayed.Wizard * 3000) },
        };

        return new Account(BattleTag, heroes, timePlayed);
    }
}