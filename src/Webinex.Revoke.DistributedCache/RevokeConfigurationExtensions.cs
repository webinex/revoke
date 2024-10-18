using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Webinex.Revoke.DistributedCache;

public static class RevokeConfigurationExtensions
{
    public static IRevokeConfiguration UseDistributedCache<T>(
        this IRevokeConfiguration configuration,
        Action<IDistributedCacheConfiguration> configure)
        where T : IDistributedCache
    {
        var settings = new DistributedCacheSettings(configuration.Services);
        configure(settings);

        configuration.Services.AddSingleton<IDistributedCacheSettings>(settings);
        configuration.Services.AddScoped<DistributedCacheRevokeStore<T>>();
        configuration.Services.AddScoped<IRevokeReadStore>(x => x.GetRequiredService<DistributedCacheRevokeStore<T>>());
        configuration.Services.AddScoped<IRevokeWriteStore>(x => x.GetRequiredService<DistributedCacheRevokeStore<T>>());
        configuration.Services.TryAddSingleton<IRevokeClientCache, EmptyRevokeClientCache>();

        return configuration;
    }
        
    /// <summary>
    ///     Adds distributed cache as a read/write revoke store
    /// </summary>
    /// <param name="configuration">Configuration</param>
    public static IRevokeConfiguration UseDistributedCache(
        this IRevokeConfiguration configuration)
    {
        configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        return configuration.UseDistributedCache<IDistributedCache>(_ => { });
    }

    /// <summary>
    ///     Adds distributed cache as a read/write revoke store
    /// </summary>
    /// <param name="configuration">Configuration</param>
    /// <param name="configure">Distributed cache configuration delegate</param>
    public static IRevokeConfiguration UseDistributedCache(
        this IRevokeConfiguration configuration,
        Action<IDistributedCacheConfiguration> configure)
    {
        configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        configure = configure ?? throw new ArgumentNullException(nameof(configure));

        return configuration.UseDistributedCache<IDistributedCache>(configure);
    }

    /// <summary>
    ///     Adds distributed cache as a read/write revoke store with given expiration time for entries.
    /// </summary>
    /// <param name="configuration">Configuration</param>
    /// <param name="expiration">Time to live for revoke entries</param>
    public static IRevokeConfiguration UseDistributedCache(
        this IRevokeConfiguration configuration,
        TimeSpan expiration)
    {
        configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        return configuration.UseDistributedCache<IDistributedCache>(x => x.UseExpiration(expiration));
    }
}