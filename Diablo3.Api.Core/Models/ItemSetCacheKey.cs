namespace Diablo3.Api.Core.Models
{
    internal readonly struct ItemSetCacheKey : IEquatable<ItemSetCacheKey>
    {
        public ItemSetCacheKey(PlayerClass playerClass, int set)
        {
            PlayerClass = playerClass;
            Set = set;
        }
        
        public PlayerClass PlayerClass { get; }
        public int Set { get; }

        public override bool Equals(object? obj) =>
            obj is ItemSetCacheKey other &&
            Equals(other);

        public bool Equals(ItemSetCacheKey other) =>
            other.PlayerClass == PlayerClass &&
            other.Set == Set;

        public override int GetHashCode() => HashCode.Combine(PlayerClass, Set);
    }
}