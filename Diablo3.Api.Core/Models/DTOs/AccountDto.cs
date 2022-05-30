namespace Diablo3.Api.Core.Models.DTOs;

public class AccountDto
{
    public string BattleTag { get; set; }
    public int ParagonLevel { get; set; }
    public int ParagonLevelHardcore { get; set; }
    public int ParagonLevelSeason { get; set; }
    public int ParagonLevelSeasonHardcore { get; set; }
    public List<AccountHeroDataObject> Heroes { get; set; }
}