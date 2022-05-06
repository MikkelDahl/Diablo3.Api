using Diablo3.Api.Core.Models;
using Diablo3.Api.Core.Services;

namespace Diablo3.Api.Core.Extensions;

public static class DataExtensions
{
    public static HeroClass ToHeroClass(this ItemSet set) => ItemSetConverter.GetConvertedHeroClass(set);
}