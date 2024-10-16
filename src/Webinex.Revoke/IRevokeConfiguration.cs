using Microsoft.Extensions.DependencyInjection;

namespace Webinex.Revoke;

public interface IRevokeConfiguration
{
    /// <summary>
    ///     Service collection can be used in derived configurations.
    /// </summary>
    IServiceCollection Services { get; }
        
    /// <summary>
    ///     Values can be used in derived configurations.
    /// </summary>
    IDictionary<string, object> Values { get; }
}