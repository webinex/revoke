using System;
using Microsoft.Extensions.DependencyInjection;

namespace Webinex.Revoke
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds Revoke services.
        ///     It's an ability to "blacklist" access tokens.
        ///
        ///     For example, if we have access token with user roles information inside
        ///     we'd like to revoke that token if user roles changed.
        /// </summary>
        /// <param name="services">Protego configuration</param>
        /// <param name="configure">Configuration delegate</param>
        public static IServiceCollection AddRevoke(
            this IServiceCollection services,
            Action<IRevokeConfiguration> configure)
        {
            services = services ?? throw new ArgumentNullException(nameof(services));
            configure = configure ?? throw new ArgumentNullException(nameof(configure));

            var configuration = new RevokeConfiguration(services);
            configure(configuration);
            configuration.Complete();

            return services;
        }
    }
}