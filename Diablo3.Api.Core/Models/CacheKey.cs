namespace Diablo3.Api.Core.Models
{
    internal readonly struct CacheKey : IEquatable<CacheKey>
    {
        public CacheKey(PlayerClass playerClass, bool hardcore)
        {
            PlayerClass = playerClass;
            Hardcore = hardcore;
        }
        
        public PlayerClass PlayerClass { get; }
        public bool Hardcore { get; }

        public override bool Equals(object? obj) =>
            obj is CacheKey other &&
            Equals(other);

        public bool Equals(CacheKey other) =>
            other.PlayerClass == PlayerClass &&
            other.Hardcore == Hardcore;

        public override int GetHashCode() => HashCode.Combine(PlayerClass, Hardcore);
    }
}