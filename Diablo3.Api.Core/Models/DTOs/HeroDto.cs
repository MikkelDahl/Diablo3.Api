namespace Diablo3.Api.Core.Models.DTOs
{
    internal class HeroDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int ParagonLevel { get; set; }
        public int HighestSoloRiftCompleted { get; set; }
        public bool Hardcore { get; set; }
        public bool Seasonal { get; set; }
    
        public Hero ToHero(string battleTag)
        {
            var upperCaseStringRepresentation = char.ToUpper(Class[0]) + Class.Substring(1, Class.Length - 1);
            if (upperCaseStringRepresentation.Contains("hunter") || upperCaseStringRepresentation.Contains("doctor"))
                upperCaseStringRepresentation = upperCaseStringRepresentation[0] 
                                                + upperCaseStringRepresentation.Substring(1, 4)
                                                + char.ToUpper(upperCaseStringRepresentation[6])
                                                + upperCaseStringRepresentation.Substring(7, upperCaseStringRepresentation.Length - 7);
        
            Enum.TryParse<HeroClass>(upperCaseStringRepresentation, out var playerClass);

            return new Hero(Id, Name, battleTag, playerClass, ParagonLevel, HighestSoloRiftCompleted);
        }
    }
}