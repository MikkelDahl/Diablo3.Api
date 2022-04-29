namespace Diablo3.Api.Core.Models.DTOs
{
    public class HeroDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlayerClass { get; set; }
        public string BattleTag { get; set; }
        public int ParagonLevel { get; set; }
        public int HighestSoloRiftCompleted { get; set; }
    
        public Hero ToHero()
        {
            var upperCaseStringRepresentation = char.ToUpper(PlayerClass[0]) + PlayerClass.Substring(1, PlayerClass.Length - 1);
            if (upperCaseStringRepresentation.Contains("hunter") || upperCaseStringRepresentation.Contains("doctor"))
                upperCaseStringRepresentation = upperCaseStringRepresentation[0] 
                                                + upperCaseStringRepresentation.Substring(1, 4)
                                                + char.ToUpper(upperCaseStringRepresentation[6])
                                                + upperCaseStringRepresentation.Substring(7, upperCaseStringRepresentation.Length - 7);
        
            Enum.TryParse<HeroClass>(upperCaseStringRepresentation, out var playerClass);

            return new Hero(Id, Name, BattleTag, playerClass, ParagonLevel, HighestSoloRiftCompleted);
        }
    }
}