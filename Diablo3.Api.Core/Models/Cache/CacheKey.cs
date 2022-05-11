namespace Diablo3.Api.Core.Models.Cache
{
    internal readonly struct CacheKey : IEquatable<CacheKey>
    {
        public CacheKey(HeroClass heroClass, ItemSet itemSet, bool hardcore = false)
        {
            HeroClass = heroClass;
            ItemSet = itemSet;
            Hardcore = hardcore;
        }
        
        public HeroClass HeroClass { get; }
        public ItemSet ItemSet { get; }
        public bool Hardcore { get; }

        public override bool Equals(object? obj) =>
            obj is CacheKey other &&
            Equals(other);

        public bool Equals(CacheKey other) =>
            other.HeroClass == HeroClass &&
            other.ItemSet == ItemSet &&
            other.Hardcore == Hardcore;

        public override int GetHashCode() => HashCode.Combine(HeroClass, ItemSet, Hardcore);
    }
}