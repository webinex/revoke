namespace Webinex.Revoke.FusionCache;

internal class RevokeFusionCacheSettings : IRevokeFusionCacheSettings, IRevokeFusionCacheConfiguration
{
    public string? CacheName { get; private set; }
    public string Prefix { get; private set; } = "revoke-";
    public string Separator { get; private set; } = "-";
    public Func<RevokeId, TimeSpan> MemoryCacheExpiration { get; private set; } = _ => TimeSpan.FromMinutes(1);
    public Func<RevokeId, TimeSpan> Expiration { get; private set; } = _ => TimeSpan.FromHours(1);
    
    public IRevokeFusionCacheConfiguration UseCacheName(string? cacheName)
    {
        CacheName = cacheName;
        return this;
    }

    public IRevokeFusionCacheConfiguration UsePrefix(string prefix)
    {
        Prefix = prefix;
        return this;
    }

    public IRevokeFusionCacheConfiguration UseSeparator(string separator)
    {
        Separator = separator;
        return this;
    }

    public IRevokeFusionCacheConfiguration UseExpiration(Func<RevokeId, TimeSpan> expiration)
    {
        Expiration = expiration;
        return this;
    }

    public IRevokeFusionCacheConfiguration UseMemoryCacheExpiration(Func<RevokeId, TimeSpan> expiration)
    {
        MemoryCacheExpiration = expiration;
        return this;
    }
}