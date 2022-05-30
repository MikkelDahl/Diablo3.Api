namespace Diablo3.Api.Core.Models;

internal record Account
{
    public string Id { get; }
    public List<Hero> Heroes { get; }
    public Dictionary<HeroClass, TimeSpan> TimePlayed { get; } //convert to hours by taking the number from the dto and multiply with 3000 

    // Hero time played math examples:
    // 21Hours = 0.007
    // 286Hours = 0.094
    // 27Hours = 0.009
    // 67Hours = 0.022
}