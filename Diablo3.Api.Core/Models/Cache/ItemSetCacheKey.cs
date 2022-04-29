namespace Diablo3.Api.Core.Models
{
    internal readonly struct ItemSetCacheKey : IEquatable<ItemSetCacheKey>
    {
        public ItemSetCacheKey(HeroClass heroClass, int set)
        {
            HeroClass = heroClass;
            Set = set;
        }
        
        public HeroClass HeroClass { get; }
        public int Set { get; }

        public override bool Equals(object? obj) =>
            obj is ItemSetCacheKey other &&
            Equals(other);

        public bool Equals(ItemSetCacheKey other) =>
            other.HeroClass == HeroClass &&
            other.Set == Set;

        public override int GetHashCode() => HashCode.Combine(HeroClass, Set);
    }
}