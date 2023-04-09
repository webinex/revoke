using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Webinex.Revoke.Stores;
using Webinex.Revoke.Stores.DistributedCache;

namespace Webinex.Revoke
{
    internal class RevokeConfiguration : IRevokeConfiguration
    {
        public RevokeConfiguration(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            
            Services.TryAddScoped<IRevoke, Revoke>();
        }

        public IServiceCollection Services { get; }

        public IDictionary<string, object> Values { get; } = new Dictionary<string, object>();

        public IRevokeConfiguration UseDistributedCache<T>(Action<IDistributedCacheConfiguration> configure)
            where T : IDistributedCache
        {
            var settings = new DistributedCacheSettings(Services);
            configure(settings);

            Services.AddSingleton<IDistributedCacheSettings>(settings);
            Services.AddScoped<DistributedCacheRevokeStore<T>>();
            Services.AddScoped<IRevokeReadStore>(x => x.GetRequiredService<DistributedCacheRevokeStore<T>>());
            Services.AddScoped<IRevokeWriteStore>(x => x.GetRequiredService<DistributedCacheRevokeStore<T>>());

            return this;
        }

        public void Complete()
        {
            Services.TryAddSingleton<IRevokeClientCache, EmptyRevokeClientCache>();
        }
    }
}