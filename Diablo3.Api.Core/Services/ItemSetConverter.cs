using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    public static class ItemSetConverter
    {
        private static readonly Dictionary<ItemSetCacheKey, ItemSet> itemSets = new()
        {
            { new ItemSetCacheKey(PlayerClass.Barbarian, 0), ItemSet.None },
            { new ItemSetCacheKey(PlayerClass.Barbarian, 1), ItemSet.WhirlWind },
            { new ItemSetCacheKey(PlayerClass.Barbarian, 2), ItemSet.Raekor },
            { new ItemSetCacheKey(PlayerClass.Barbarian, 3), ItemSet.ImmortalKings },
            { new ItemSetCacheKey(PlayerClass.Barbarian, 4), ItemSet.MoTE },
            { new ItemSetCacheKey(PlayerClass.Barbarian, 5), ItemSet.NinetySavages },
        
            { new ItemSetCacheKey(PlayerClass.DemonHunter, 0), ItemSet.None },
            { new ItemSetCacheKey(PlayerClass.DemonHunter, 1), ItemSet.UE },
            { new ItemSetCacheKey(PlayerClass.DemonHunter, 2), ItemSet.Natalya },
            { new ItemSetCacheKey(PlayerClass.DemonHunter, 3), ItemSet.Marauder },
            { new ItemSetCacheKey(PlayerClass.DemonHunter, 4), ItemSet.Danetta },
            { new ItemSetCacheKey(PlayerClass.DemonHunter, 5), ItemSet.Dreadlands },
        
            { new ItemSetCacheKey(PlayerClass.Monk, 0), ItemSet.None },
            { new ItemSetCacheKey(PlayerClass.Monk, 1), ItemSet.Innas },
            { new ItemSetCacheKey(PlayerClass.Monk, 2), ItemSet.Uliana },
            { new ItemSetCacheKey(PlayerClass.Monk, 3), ItemSet.MonkeyKing },
            { new ItemSetCacheKey(PlayerClass.Monk, 4), ItemSet.PatternOfJustice },
            { new ItemSetCacheKey(PlayerClass.Monk, 5), ItemSet.RaimentOfAThousandStorms },
        
            { new ItemSetCacheKey(PlayerClass.Necromancer, 0), ItemSet.None },
            { new ItemSetCacheKey(PlayerClass.Necromancer, 1), ItemSet.Pestilence },
            { new ItemSetCacheKey(PlayerClass.Necromancer, 2), ItemSet.Carnival },
            { new ItemSetCacheKey(PlayerClass.Necromancer, 3), ItemSet.Inarius },
            { new ItemSetCacheKey(PlayerClass.Necromancer, 4), ItemSet.Rathmas },
            { new ItemSetCacheKey(PlayerClass.Necromancer, 5), ItemSet.TragOuls },
        
            { new ItemSetCacheKey(PlayerClass.WitchDoctor, 0), ItemSet.None },
            { new ItemSetCacheKey(PlayerClass.WitchDoctor, 1), ItemSet.HellTooth },
            { new ItemSetCacheKey(PlayerClass.WitchDoctor, 2), ItemSet.Arachyr },
            { new ItemSetCacheKey(PlayerClass.WitchDoctor, 3), ItemSet.Mundunugu },
            { new ItemSetCacheKey(PlayerClass.WitchDoctor, 4), ItemSet.Zunnimassa },
            { new ItemSetCacheKey(PlayerClass.WitchDoctor, 5), ItemSet.JadeHarvester },
        
            { new ItemSetCacheKey(PlayerClass.Wizard, 0), ItemSet.None },
            { new ItemSetCacheKey(PlayerClass.Wizard, 1), ItemSet.Firebird },
            { new ItemSetCacheKey(PlayerClass.Wizard, 2), ItemSet.TalRasha },
            { new ItemSetCacheKey(PlayerClass.Wizard, 3), ItemSet.TyphonsVeil },
            { new ItemSetCacheKey(PlayerClass.Wizard, 4), ItemSet.Vyr },
            { new ItemSetCacheKey(PlayerClass.Wizard, 5), ItemSet.DMO }
        };

        public static ItemSet GetConvertedSet(PlayerClass playerClass, int set)
        {
            var cacheKey = new ItemSetCacheKey(playerClass, set);
            return itemSets[cacheKey];
        }
    }
}