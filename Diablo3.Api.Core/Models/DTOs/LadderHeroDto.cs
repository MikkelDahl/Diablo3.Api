namespace Diablo3.Api.Core.Models.DTOs
{
    internal class LadderHeroDto
    {
        public int Id { get; set; }
        public string Class { get; set; }
        public string BattleTag { get; set; }
        public int ParagonLevel { get; set; }
        public int RiftLevel { get; set; }
    
        public LadderHero ToLadderHero()
        {
            var classString = char.ToUpper(Class[0]) + Class.Substring(1, Class.Length - 1);
            if (classString.Contains("hunter") )
                classString = "DH";
            else if (classString.Contains("doctor"))
                classString = classString[0] 
                                                + classString.Substring(1, 4)
                                                + char.ToUpper(classString[6])
                                                + classString.Substring(7, classString.Length - 7);
        
            Enum.TryParse<HeroClass>(classString, out var playerClass);

            return new LadderHero(Id, BattleTag, playerClass, ParagonLevel, RiftLevel);
        }
    }
}