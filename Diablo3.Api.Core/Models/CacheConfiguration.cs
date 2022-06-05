namespace Diablo3.Api.Core.Models
{
    public class CacheConfiguration
    {
        public CacheConfiguration(CacheOptions options, int cacheTtlSeconds = 120)
        {
            Options = options;
            CacheTtl = new TimeSpan(0, 0, cacheTtlSeconds);
        }
    
        public CacheOptions Options { get; }
        public TimeSpan CacheTtl { get; }

   
    }
}