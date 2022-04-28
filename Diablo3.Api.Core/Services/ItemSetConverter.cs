using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    internal static class ItemSetConverter
    {
        private static readonly Dictionary<ItemSetCacheKey, ItemSet> ItemSets = new()
        {
            { new ItemSetCacheKey(HeroClass.Barbarian, 0), ItemSet.None },
            { new ItemSetCacheKey(HeroClass.Barbarian, 1), ItemSet.Raekor },
            { new ItemSetCacheKey(HeroClass.Barbarian, 2), ItemSet.MoTE },
            { new ItemSetCacheKey(HeroClass.Barbarian, 3), ItemSet.WhirlWind },
            { new ItemSetCacheKey(HeroClass.Barbarian, 4), ItemSet.ImmortalKings },
            { new ItemSetCacheKey(HeroClass.Barbarian, 5), ItemSet.NinetySavages },

            { new ItemSetCacheKey(HeroClass.Crusader, 0), ItemSet.None },
            { new ItemSetCacheKey(HeroClass.Crusader, 1), ItemSet.Akkhan },
            { new ItemSetCacheKey(HeroClass.Crusader, 2), ItemSet.Invoker },
            { new ItemSetCacheKey(HeroClass.Crusader, 3), ItemSet.Roland },
            { new ItemSetCacheKey(HeroClass.Crusader, 4), ItemSet.Seeker },
            { new ItemSetCacheKey(HeroClass.Crusader, 5), ItemSet.Aegis },
            
            { new ItemSetCacheKey(HeroClass.DemonHunter, 0), ItemSet.None },
            { new ItemSetCacheKey(HeroClass.DemonHunter, 1), ItemSet.Marauder },
            { new ItemSetCacheKey(HeroClass.DemonHunter, 2), ItemSet.ShadowsMantle },
            { new ItemSetCacheKey(HeroClass.DemonHunter, 3), ItemSet.UE },
            { new ItemSetCacheKey(HeroClass.DemonHunter, 4), ItemSet.Natalyas },
            { new ItemSetCacheKey(HeroClass.DemonHunter, 5), ItemSet.Dreadlands },
        
            { new ItemSetCacheKey(HeroClass.Monk, 0), ItemSet.None },
            { new ItemSetCacheKey(HeroClass.Monk, 1), ItemSet.RaimentOfAThousandStorms },
            { new ItemSetCacheKey(HeroClass.Monk, 2), ItemSet.MonkeyKing },
            { new ItemSetCacheKey(HeroClass.Monk, 3), ItemSet.Uliana },
            { new ItemSetCacheKey(HeroClass.Monk, 4), ItemSet.Innas },
            { new ItemSetCacheKey(HeroClass.Monk, 5), ItemSet.PatternOfJustice },
        
            { new ItemSetCacheKey(HeroClass.Necromancer, 0), ItemSet.None },
            { new ItemSetCacheKey(HeroClass.Necromancer, 1), ItemSet.Rathma },
            { new ItemSetCacheKey(HeroClass.Necromancer, 2), ItemSet.TragOuls },
            { new ItemSetCacheKey(HeroClass.Necromancer, 3), ItemSet.Inarius },
            { new ItemSetCacheKey(HeroClass.Necromancer, 4), ItemSet.Pestilence },
            { new ItemSetCacheKey(HeroClass.Necromancer, 5), ItemSet.Masquerade },
        
            { new ItemSetCacheKey(HeroClass.WitchDoctor, 0), ItemSet.None },
            { new ItemSetCacheKey(HeroClass.WitchDoctor, 1), ItemSet.JadeHarvester },
            { new ItemSetCacheKey(HeroClass.WitchDoctor, 2), ItemSet.HellTooth },
            { new ItemSetCacheKey(HeroClass.WitchDoctor, 3), ItemSet.Zunnimassa },
            { new ItemSetCacheKey(HeroClass.WitchDoctor, 4), ItemSet.Arachyr },
            { new ItemSetCacheKey(HeroClass.WitchDoctor, 5), ItemSet.Mundunugu },
        
            { new ItemSetCacheKey(HeroClass.Wizard, 0), ItemSet.None },
            { new ItemSetCacheKey(HeroClass.Wizard, 1), ItemSet.Firebird },
            { new ItemSetCacheKey(HeroClass.Wizard, 2), ItemSet.Vyr },
            { new ItemSetCacheKey(HeroClass.Wizard, 3), ItemSet.DMO },
            { new ItemSetCacheKey(HeroClass.Wizard, 4), ItemSet.TalRasha },
            { new ItemSetCacheKey(HeroClass.Wizard, 5), ItemSet.TyphonsVeil }
        };

        internal static ItemSet GetConvertedSet(HeroClass heroClass, int set)
        {
            var cacheKey = new ItemSetCacheKey(heroClass, set);
            return ItemSets[cacheKey];
        }
    }
}