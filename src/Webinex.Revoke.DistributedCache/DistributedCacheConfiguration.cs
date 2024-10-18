using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Webinex.Revoke.DistributedCache;

internal class DistributedCacheSettings : IDistributedCacheConfiguration, IDistributedCacheSettings
{
    private readonly IServiceCollection _services;

    public string Prefix { get; private set; } = "revoke-";
    public string Separator { get; private set; } = "-";
    public Func<RevokeId, TimeSpan> Expiration { get; private set; } = _ => TimeSpan.FromHours(1);
    public string ClientCachePrefix { get; private set; } = "revoke-temp-";
    public TimeSpan ClientCacheExpiration { get; private set; }

    public DistributedCacheSettings(IServiceCollection services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public IDistributedCacheConfiguration UsePrefix(string prefix)
    {
        Prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
        return this;
    }

    public IDistributedCacheConfiguration UseSeparator(string separator)
    {
        Separator = separator ?? throw new ArgumentNullException(nameof(separator));
        return this;
    }

    public IDistributedCacheConfiguration UseExpiration(Func<RevokeId, TimeSpan> expiration)
    {
        Expiration = expiration ?? throw new ArgumentNullException(nameof(expiration));
        return this;
    }

    public IDistributedCacheConfiguration UseClientCache(string? prefix = null, TimeSpan? expiration = null)
    {
        ClientCacheExpiration = expiration ?? TimeSpan.FromSeconds(10);
        ClientCachePrefix = prefix ?? "revoke-temp-";
            
        _services.AddMemoryCache();
        _services.TryAddScoped<IRevokeClientCache, RevokeClientCache>();
            
        return this;
    }
}