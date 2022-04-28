namespace Diablo3.Api.Core.Models
{
    public record Hero
    {
        public Hero(int Id, string Name, string BattleTag, HeroClass HeroClass, int Paragon, int HighestGreaterRiftCompleted)
        {
            this.Id = Id;
            this.Name = Name;
            this.BattleTag = BattleTag;
            this.HeroClass = HeroClass;
            this.Paragon = Paragon;
            this.HighestGreaterRiftCompleted = HighestGreaterRiftCompleted;
        }

        public Hero(int id)
        {
            
        }

        public int Id { get; init; }
        public string Name { get; init; }
        public string BattleTag { get; init; }
        public HeroClass HeroClass { get; init; }
        public int Paragon { get; init; }
        public int HighestGreaterRiftCompleted { get; init; }

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