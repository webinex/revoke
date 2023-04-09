using System;
using Microsoft.Extensions.Caching.Distributed;
using Webinex.Revoke.Stores.DistributedCache;

namespace Webinex.Revoke
{
    public static class RevokeConfigurationExtensions
    {
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
}