using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Webinex.Revoke;

internal class RevokeConfiguration : IRevokeConfiguration
{
    public RevokeConfiguration(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
            
        Services.TryAddScoped<IRevoke, Revoke>();
    }

    public IServiceCollection Services { get; }

    public IDictionary<string, object> Values { get; } = new Dictionary<string, object>();
}