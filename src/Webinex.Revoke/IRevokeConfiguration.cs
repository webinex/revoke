using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Webinex.Revoke.Stores.DistributedCache;

namespace Webinex.Revoke
{
    public interface IRevokeConfiguration
    {
        /// <summary>
        ///     Service collection can be used in derived configurations.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        ///     Adds distributed cache as a read/write revoke store
        /// </summary>
        /// <param name="configure">Configuration delegate</param>
        /// <typeparam name="T">Type of distributed cache</typeparam>
        IRevokeConfiguration UseDistributedCache<T>(Action<IDistributedCacheConfiguration> configure)
            where T : IDistributedCache;
        
        /// <summary>
        ///     Values can be used in derived configurations.
        /// </summary>
        IDictionary<string, object> Values { get; }
    }
}