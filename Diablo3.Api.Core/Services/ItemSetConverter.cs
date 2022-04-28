using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    internal static class ItemSetConverter
    {
        private static readonly Dictionary<ItemSetCacheKey, ItemSet> ItemSets = new()
        {
            { new ItemSetCacheKey(PlayerClass.Barbarian, 0), ItemSet.None },
            { new ItemSetCacheKey(PlayerClass.Barbarian, 1), ItemSet.Raekor },
            { new ItemSetCacheKey(PlayerClass.Barbarian, 2), ItemSet.MoTE },
            { new ItemSetCacheKey(PlayerClass.Barbarian, 3), ItemSet.WhirlWind },
            { new ItemSetCacheKey(PlayerClass.Barbarian, 4), ItemSet.ImmortalKings },
            { new ItemSetCacheKey(PlayerClass.Barbarian, 5), ItemSet.NinetySavages },

            { new ItemSetCacheKey(PlayerClass.Crusader, 0), ItemSet.None },
            { new ItemSetCacheKey(PlayerClass.Crusader, 1), ItemSet.Akkhan },
            { new ItemSetCacheKey(PlayerClass.Crusader, 2), ItemSet.Invoker },
            { new ItemSetCacheKey(PlayerClass.Crusader, 3), ItemSet.Roland },
            { new ItemSetCacheKey(PlayerClass.Crusader, 4), ItemSet.Seeker },
            { new ItemSetCacheKey(PlayerClass.Crusader, 5), ItemSet.Aegis },
            
            { new ItemSetCacheKey(PlayerClass.DemonHunter, 0), ItemSet.None },
            { new ItemSetCacheKey(PlayerClass.DemonHunter, 1), ItemSet.Marauder },
            { new ItemSetCacheKey(PlayerClass.DemonHunter, 2), ItemSet.ShadowsMantle },
            { new ItemSetCacheKey(PlayerClass.DemonHunter, 3), ItemSet.UE },
            { new ItemSetCacheKey(PlayerClass.DemonHunter, 4), ItemSet.Natalyas },
            { new ItemSetCacheKey(PlayerClass.DemonHunter, 5), ItemSet.Dreadlands },
        
            { new ItemSetCacheKey(PlayerClass.Monk, 0), ItemSet.None },
            { new ItemSetCacheKey(PlayerClass.Monk, 1), ItemSet.RaimentOfAThousandStorms },
            { new ItemSetCacheKey(PlayerClass.Monk, 2), ItemSet.MonkeyKing },
            { new ItemSetCacheKey(PlayerClass.Monk, 3), ItemSet.Uliana },
            { new ItemSetCacheKey(PlayerClass.Monk, 4), ItemSet.Innas },
            { new ItemSetCacheKey(PlayerClass.Monk, 5), ItemSet.PatternOfJustice },
        
            { new ItemSetCacheKey(PlayerClass.Necromancer, 0), ItemSet.None },
            { new ItemSetCacheKey(PlayerClass.Necromancer, 1), ItemSet.Rathma },
            { new ItemSetCacheKey(PlayerClass.Necromancer, 2), ItemSet.TragOuls },
            { new ItemSetCacheKey(PlayerClass.Necromancer, 3), ItemSet.Inarius },
            { new ItemSetCacheKey(PlayerClass.Necromancer, 4), ItemSet.Pestilence },
            { new ItemSetCacheKey(PlayerClass.Necromancer, 5), ItemSet.Masquerade },
        
            { new ItemSetCacheKey(PlayerClass.WitchDoctor, 0), ItemSet.None },
            { new ItemSetCacheKey(PlayerClass.WitchDoctor, 1), ItemSet.JadeHarvester },
            { new ItemSetCacheKey(PlayerClass.WitchDoctor, 2), ItemSet.HellTooth },
            { new ItemSetCacheKey(PlayerClass.WitchDoctor, 3), ItemSet.Zunnimassa },
            { new ItemSetCacheKey(PlayerClass.WitchDoctor, 4), ItemSet.Arachyr },
            { new ItemSetCacheKey(PlayerClass.WitchDoctor, 5), ItemSet.Mundunugu },
        
            { new ItemSetCacheKey(PlayerClass.Wizard, 0), ItemSet.None },
            { new ItemSetCacheKey(PlayerClass.Wizard, 1), ItemSet.Firebird },
            { new ItemSetCacheKey(PlayerClass.Wizard, 2), ItemSet.Vyr },
            { new ItemSetCacheKey(PlayerClass.Wizard, 3), ItemSet.DMO },
            { new ItemSetCacheKey(PlayerClass.Wizard, 4), ItemSet.TalRasha },
            { new ItemSetCacheKey(PlayerClass.Wizard, 5), ItemSet.TyphonsVeil }
        };

        internal static ItemSet GetConvertedSet(PlayerClass playerClass, int set)
        {
            var cacheKey = new ItemSetCacheKey(playerClass, set);
            return ItemSets[cacheKey];
        }
    }
}