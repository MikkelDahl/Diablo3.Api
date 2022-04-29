namespace Diablo3.Api.Core.Models
{
    public record Hero(int Id, string Name, string BattleTag, HeroClass HeroClass, int Paragon, int HighestGreaterRiftCompleted)
    {
        public void Deconstruct(out int Id, out string Name, out string BattleTag, out HeroClass HeroClass, out int Paragon, out int HighestGreaterRiftCompleted)
        {
            Id = this.Id;
            Name = this.Name;
            BattleTag = this.BattleTag;
            HeroClass = this.HeroClass;
            Paragon = this.Paragon;
            HighestGreaterRiftCompleted = this.HighestGreaterRiftCompleted;
        }
    }
}