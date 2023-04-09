using System;

namespace Webinex.Revoke.Middleware
{
    public static class ProtegoRevokeConfigurationExtensions
    {
        public static IRevokeConfiguration AddMiddleware(
            this IRevokeConfiguration revokeConfiguration,
            Action<IRevokeMiddlewareConfiguration> configure)
        {
            revokeConfiguration = revokeConfiguration ?? throw new ArgumentNullException(nameof(revokeConfiguration));
            configure = configure ?? throw new ArgumentNullException(nameof(configure));

            var configuration = new RevokeMiddlewareConfiguration(revokeConfiguration.Services);
            configure(configuration);
            configuration.Complete();

            return revokeConfiguration;
        }
    }
}