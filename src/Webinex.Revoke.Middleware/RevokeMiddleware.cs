using System.Net;
using Microsoft.AspNetCore.Http;

namespace Webinex.Revoke.Middleware;

internal class RevokeMiddleware
{
    private readonly RequestDelegate _next;

    public RevokeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext context,
        IRevoke revoke,
        IRevokeAccessor revokeAccessor,
        IRevokeIssuedAtAccessor revokeIssuedAtAccessor,
        IRevokeMiddlewareSettings settings)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        var checks = await revokeAccessor.GetAsync(context);
        var issuedAt = await revokeIssuedAtAccessor.GetAsync(context);
        var revoked = await revoke.RevokedAnyAsync(checks, issuedAt);

        if (revoked)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.Headers[settings.RevokedHeaderName] = true.ToString().ToLower();
            return;
        }

        await _next(context);
    }
}