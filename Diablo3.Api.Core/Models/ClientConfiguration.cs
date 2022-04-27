namespace Diablo3.Api.Core.Models;

public class ClientConfiguration
{
    public CacheConfiguration CacheConfiguration { get; }

    public ClientConfiguration(CacheConfiguration cacheConfiguration)
    {
        CacheConfiguration = cacheConfiguration;
    }
}