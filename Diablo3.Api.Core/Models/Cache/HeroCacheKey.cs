namespace Diablo3.Api.Core.Models.Cache;

internal readonly struct HeroCacheKey : IEquatable<HeroCacheKey>
{
    public HeroCacheKey(int id, TimeSpan clearTimeMs)
    {
        Id = id;
        ClearTimeMs = clearTimeMs;
    }
    
    public int Id { get; }
    public TimeSpan ClearTimeMs { get; }

    public override bool Equals(object? obj) =>
        obj is HeroCacheKey other &&
        Equals(other);

    public bool Equals(HeroCacheKey other) =>
        other.Id == Id &&
        other.ClearTimeMs == ClearTimeMs;

    public override int GetHashCode() => HashCode.Combine(Id, ClearTimeMs);
}