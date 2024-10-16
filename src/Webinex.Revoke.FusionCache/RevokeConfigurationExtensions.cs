using Microsoft.Extensions.DependencyInjection;

namespace Webinex.Revoke.FusionCache;

public static class RevokeConfigurationExtensions
{
    /// <summary>
    /// Uses FusionCache as cache client. DOESN'T automatically register FusionCache client. You must do it separately
    /// <param name="configuration"><see cref="IRevokeConfiguration"/></param>
    /// <param name="cacheName">
    /// Name to resolve proper FusionCache. <br/>
    /// See details here https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/NamedCaches.md <br/>
    /// If null, default cache will be used
    /// </param>
    /// </summary>
    public static IRevokeConfiguration UseFusionCache(
        this IRevokeConfiguration configuration,
        string? cacheName = default)
    {
        return configuration.UseFusionCache(e => e.UseCacheName(cacheName));
    }

    /// <summary>
    /// Uses FusionCache as cache client. DOESN'T automatically register FusionCache client. You must do it separately
    /// </summary>
    public static IRevokeConfiguration UseFusionCache(
        this IRevokeConfiguration configuration,
        Action<IRevokeFusionCacheConfiguration> configure)
    {
        var settings = new RevokeFusionCacheSettings();
        configure(settings);

        configuration.Services.AddSingleton<IRevokeFusionCacheSettings>(settings);
        configuration.Services.AddScoped<FusionCacheRevoke>();
        configuration.Services.AddScoped<IRevokeReadStore>(x => x.GetRequiredService<FusionCacheRevoke>());
        configuration.Services.AddScoped<IRevokeWriteStore>(x => x.GetRequiredService<FusionCacheRevoke>());

        return configuration;
    }
}