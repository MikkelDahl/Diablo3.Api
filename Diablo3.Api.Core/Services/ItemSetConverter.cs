using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    internal static class ItemSetConverter
    {
        internal static HeroClass GetConvertedHeroClass(ItemSet itemSet)
        {
            return itemSet switch
            {
                ItemSet.All => throw new ArgumentException($"Ambiguous parameter {itemSet}"),
                ItemSet.NoSetBarbarian => HeroClass.Barbarian,
                ItemSet.NoSetCrusader => HeroClass.Crusader,
                ItemSet.NoSetMonk => HeroClass.Monk,
                ItemSet.NoSetDemonHunter => HeroClass.DemonHunter,
                ItemSet.NoSetNecromancer => HeroClass.Necromancer,
                ItemSet.NoSetWitchDoctor => HeroClass.WitchDoctor,
                ItemSet.NoSetWizard => HeroClass.Wizard,
                ItemSet.Raekor => HeroClass.Barbarian,
                ItemSet.MoTE => HeroClass.Barbarian,
                ItemSet.WhirlWind => HeroClass.Barbarian,
                ItemSet.ImmortalKings => HeroClass.Barbarian,
                ItemSet.NinetySavages => HeroClass.Barbarian,
                ItemSet.ShadowsMantle => HeroClass.DemonHunter,
                ItemSet.Marauder => HeroClass.DemonHunter,
                ItemSet.Natalyas => HeroClass.DemonHunter,
                ItemSet.Dreadlands => HeroClass.DemonHunter,
                ItemSet.UE => HeroClass.DemonHunter,
                ItemSet.Innas => HeroClass.Monk,
                ItemSet.MonkeyKing => HeroClass.Monk,
                ItemSet.PatternOfJustice => HeroClass.Monk,
                ItemSet.RaimentOfAThousandStorms => HeroClass.Monk,
                ItemSet.Uliana => HeroClass.Monk,
                ItemSet.DMO => HeroClass.Wizard,
                ItemSet.Firebird => HeroClass.Wizard,
                ItemSet.TalRasha => HeroClass.Wizard,
                ItemSet.TyphonsVeil => HeroClass.Wizard,
                ItemSet.Vyr => HeroClass.Wizard,
                ItemSet.Pestilence => HeroClass.Necromancer,
                ItemSet.Rathma => HeroClass.Necromancer,
                ItemSet.Inarius => HeroClass.Necromancer,
                ItemSet.TragOuls => HeroClass.Necromancer,
                ItemSet.Masquerade => HeroClass.Necromancer,
                ItemSet.Arachyr => HeroClass.WitchDoctor,
                ItemSet.Mundunugu => HeroClass.WitchDoctor,
                ItemSet.HellTooth => HeroClass.WitchDoctor,
                ItemSet.JadeHarvester => HeroClass.WitchDoctor,
                ItemSet.Zunnimassa => HeroClass.WitchDoctor,
                ItemSet.Aegis => HeroClass.Crusader,
                ItemSet.Akkhan => HeroClass.Crusader,
                ItemSet.Invoker => HeroClass.Crusader,
                ItemSet.Roland => HeroClass.Crusader,
                ItemSet.Seeker => HeroClass.Crusader,
                _ => throw new ArgumentOutOfRangeException(nameof(itemSet), itemSet, null)
            };
        }
        
         internal static int GetConvertedIndex(ItemSet itemSet)
        {
            return itemSet switch
            {
                ItemSet.All => -1,
                ItemSet.NoSetBarbarian => 0,
                ItemSet.NoSetCrusader => 0,
                ItemSet.NoSetMonk => 0,
                ItemSet.NoSetNecromancer => 0,
                ItemSet.NoSetWizard => 0,
                ItemSet.NoSetDemonHunter => 0,
                ItemSet.NoSetWitchDoctor => 0,
                ItemSet.Raekor => 1,
                ItemSet.MoTE => 2,
                ItemSet.WhirlWind => 3,
                ItemSet.ImmortalKings => 4,
                ItemSet.NinetySavages => 5,
                ItemSet.Marauder => 1,
                ItemSet.ShadowsMantle => 2,
                ItemSet.UE => 3,
                ItemSet.Natalyas => 4,
                ItemSet.Dreadlands => 5,
                ItemSet.Innas => 4,
                ItemSet.MonkeyKing => 2,
                ItemSet.PatternOfJustice => 5,
                ItemSet.RaimentOfAThousandStorms => 1,
                ItemSet.Uliana => 3,
                ItemSet.DMO => 3,
                ItemSet.Firebird => 1,
                ItemSet.TalRasha => 4,
                ItemSet.TyphonsVeil => 5,
                ItemSet.Vyr => 2,
                ItemSet.Pestilence => 4,
                ItemSet.Rathma => 1,
                ItemSet.Inarius => 3,
                ItemSet.TragOuls => 2,
                ItemSet.Masquerade => 5,
                ItemSet.Arachyr => 4,
                ItemSet.Mundunugu => 5,
                ItemSet.HellTooth => 2,
                ItemSet.JadeHarvester => 1,
                ItemSet.Zunnimassa => 3,
                ItemSet.Aegis => 5,
                ItemSet.Akkhan => 1,
                ItemSet.Invoker => 2,
                ItemSet.Roland => 3,
                ItemSet.Seeker => 4,
                _ => throw new ArgumentOutOfRangeException(nameof(itemSet), itemSet, null)
            };
        }
         
         internal static List<ItemSet> GetForClass(HeroClass heroClass)
         {
             return heroClass switch
             {
                 HeroClass.Barbarian => new List<ItemSet>
                 {
                     ItemSet.NoSetBarbarian,
                     ItemSet.Raekor,
                     ItemSet.MoTE,
                     ItemSet.WhirlWind,
                     ItemSet.ImmortalKings,
                     ItemSet.NinetySavages
                 },
                 HeroClass.Crusader => new List<ItemSet>
                 {
                     ItemSet.NoSetCrusader,
                     ItemSet.Akkhan,
                     ItemSet.Roland,
                     ItemSet.Seeker,
                     ItemSet.Invoker,
                     ItemSet.Aegis
                 },
                 HeroClass.DemonHunter => new List<ItemSet>
                 {
                     ItemSet.NoSetDemonHunter,
                     ItemSet.UE,
                     ItemSet.Dreadlands,
                     ItemSet.Marauder,
                     ItemSet.ShadowsMantle,
                     ItemSet.Natalyas
                 },
                 HeroClass.Monk => new List<ItemSet>
                 {
                     ItemSet.NoSetMonk,
                     ItemSet.Innas,
                     ItemSet.Uliana,
                     ItemSet.PatternOfJustice,
                     ItemSet.RaimentOfAThousandStorms,
                     ItemSet.MonkeyKing
                 },
                 HeroClass.Necromancer => new List<ItemSet>
                 {
                     ItemSet.NoSetNecromancer,
                     ItemSet.Pestilence,
                     ItemSet.Rathma,
                     ItemSet.Inarius,
                     ItemSet.TragOuls,
                     ItemSet.Masquerade
                 },
                 HeroClass.WitchDoctor => new List<ItemSet>
                 {
                     ItemSet.NoSetWitchDoctor,
                     ItemSet.Mundunugu,
                     ItemSet.Zunnimassa,
                     ItemSet.JadeHarvester,
                     ItemSet.HellTooth,
                     ItemSet.Arachyr
                 },
                 HeroClass.Wizard => new List<ItemSet>
                 {
                     ItemSet.NoSetWizard,
                     ItemSet.DMO,
                     ItemSet.TyphonsVeil,
                     ItemSet.Vyr,
                     ItemSet.Firebird,
                     ItemSet.TalRasha
                 },
                 _ => throw new ArgumentOutOfRangeException(nameof(heroClass), heroClass, null)
             };
         }
    }
}