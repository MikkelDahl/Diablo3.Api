using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Extensions
{
    public static class ClassExtensions
    {
        public static PlayerClass ToPlayerClass(this ItemSet itemSet)
        {
            return itemSet switch
            {
                ItemSet.Raekor or ItemSet.MoTE or ItemSet.WhirlWind or ItemSet.ImmortalKings or ItemSet.NinetySavages =>
                    PlayerClass.Barbarian,
                ItemSet.Marauder or ItemSet.Natalya or ItemSet.Dreadlands or ItemSet.UE or ItemSet.Danetta => PlayerClass
                    .DemonHunter,
                ItemSet.Innas or ItemSet.MonkeyKing or ItemSet.PatternOfJustice or ItemSet.RaimentOfAThousandStorms
                    or ItemSet.Uliana => PlayerClass.Monk,
                ItemSet.DMO or ItemSet.Firebird or ItemSet.TalRasha or ItemSet.TyphonsVeil or ItemSet.Vyr => PlayerClass
                    .Wizard,
                _ => throw new ArgumentOutOfRangeException(nameof(itemSet), itemSet, null)
            };
        }
    }
}