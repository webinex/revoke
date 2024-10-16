namespace Webinex.Revoke.FusionCache;

public interface IRevokeFusionCacheConfiguration
{
    /// <summary>
    /// Name to resolve proper FusionCache. <br/>
    /// See details here https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/NamedCaches.md
    /// </summary>
    /// <param name="cacheName">Cache name to use. If null, default cache will be used</param>
    public IRevokeFusionCacheConfiguration UseCacheName(string? cacheName);

    /// <summary>
    ///     Distributed cache key prefix.
    ///     It might be configured if "revoke issuer" configured to use different key prefix.
    ///     Default: "revoke-"
    /// </summary>
    /// <param name="prefix">Cache key prefix to use</param>
    IRevokeFusionCacheConfiguration UsePrefix(string prefix);

    /// <summary>
    ///     Distributed cache key separator for <see cref="RevokeId.Kind"/> and <see cref="RevokeId.Value"/>
    ///     It might be configured if "revoke issuer" configured to use different key separator.
    ///     Default: "-"
    /// </summary>
    /// <param name="separator">Cache key separator to use</param>
    IRevokeFusionCacheConfiguration UseSeparator(string separator);

    /// <summary>
    ///     Distributed cache entry time to live.
    ///     If used to revoke access tokens, might match with access token time to live.
    ///     Used only on "revoke issuer" side.
    /// </summary>
    /// <param name="expiration">Expiration factory</param>
    IRevokeFusionCacheConfiguration UseExpiration(Func<RevokeId, TimeSpan> expiration);

    /// <summary>
    /// Ignored if FusionCache has Backplane.
    /// MemoryCache expiration must be significantly shorter if FusionCache doesn't have backplane.
    /// By default expiration is 1 minute
    /// </summary>
    IRevokeFusionCacheConfiguration UseMemoryCacheExpiration(Func<RevokeId, TimeSpan> expiration);
}