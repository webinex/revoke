using Microsoft.AspNetCore.Builder;

namespace Webinex.Revoke.Middleware;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    ///     Adds revoke middleware that checks incoming request for revoke.
    ///     If request matches any revoke criteria. Request would be declined.
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder"/></param>
    /// <returns><see cref="IApplicationBuilder"/></returns>
    public static IApplicationBuilder UseRevoke(
        this IApplicationBuilder app)
    {
        app = app ?? throw new ArgumentNullException(nameof(app));
            
        app.UseMiddleware<RevokeMiddleware>();
        return app;
    }
}