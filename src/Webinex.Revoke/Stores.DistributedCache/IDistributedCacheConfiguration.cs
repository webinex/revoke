using System;

namespace Webinex.Revoke.Stores.DistributedCache
{
    public interface IDistributedCacheConfiguration
    {
        /// <summary>
        ///     Distributed cache key prefix.
        ///     It might be configured if "revoke issuer" configured to use different key prefix.
        ///     Default: "revoke-"
        /// </summary>
        /// <param name="prefix">Cache key prefix to use</param>
        IDistributedCacheConfiguration UsePrefix(string prefix);
        
        /// <summary>
        ///     Distributed cache key separator for <see cref="RevokeId.Kind"/> and <see cref="RevokeId.Value"/>
        ///     It might be configured if "revoke issuer" configured to use different key separator.
        ///     Default: "-"
        /// </summary>
        /// <param name="separator">Cache key separator to use</param>
        IDistributedCacheConfiguration UseSeparator(string separator);
        
        /// <summary>
        ///     Distributed cache entry time to live.
        ///     If used to revoke access tokens, might match with access token time to live.
        ///     Used only on "revoke issuer" side.
        /// </summary>
        /// <param name="expiration">Expiration factory</param>
        IDistributedCacheConfiguration UseExpiration(Func<RevokeId, TimeSpan> expiration);
        
        /// <summary>
        ///     Adds MemoryCache which will cache results for <paramref name="expiration"/>.
        ///     It's useful when user opens a screen we'll probably make multiple requests and no needs to
        ///     check DistributedCache every time.
        /// </summary>
        /// <param name="prefix">Prefix for cache keys. Default: "revoke-temp-"</param>
        /// <param name="expiration">Cache entry time to live. Default: 10s</param>
        IDistributedCacheConfiguration UseClientCache(string prefix = null, TimeSpan? expiration = null);
    }
}