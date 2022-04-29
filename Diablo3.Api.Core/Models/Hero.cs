namespace Diablo3.Api.Core.Models
{
    public record Hero(int Id, string Name, string BattleTag, HeroClass HeroClass, int Paragon, int HighestGreaterRiftCompleted);
}