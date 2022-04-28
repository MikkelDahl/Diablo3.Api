namespace Diablo3.Api.Core.Models
{
    public class DefaultClientConfiguration : ClientConfiguration
    {
        private DefaultClientConfiguration(CacheConfiguration cacheConfiguration) : base(cacheConfiguration)
        {
        
        }

        public static DefaultClientConfiguration GetConfiguration()
        {
            var cacheConfig = new CacheConfiguration(CacheOptions.Default);
            return new DefaultClientConfiguration(cacheConfig);
        }
    }
}