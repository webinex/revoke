using System;

namespace Webinex.Revoke.Stores.DistributedCache
{
    public static class DistributedCacheConfigurationExtensions
    {
        /// <summary>
        ///     Distributed cache entry time to live.
        ///     If used to revoke access tokens, might match with access token time to live.
        ///     Used only on "revoke issuer" side.
        /// </summary>
        /// <param name="configuration">Configuration</param>
        /// <param name="expiration">Expiration time to live for all <see cref="RevokeId"/></param>
        public static IDistributedCacheConfiguration UseExpiration(
            this IDistributedCacheConfiguration configuration,
            TimeSpan expiration)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            return configuration.UseExpiration(_ => expiration);
        }
    }
}