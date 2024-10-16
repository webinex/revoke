using Microsoft.AspNetCore.Http;

namespace Webinex.Revoke.Middleware;

internal class DelegateIssuedAtAccessor : IRevokeIssuedAtAccessor
{
    private readonly Func<HttpContext, DateTime> _accessor;

    public DelegateIssuedAtAccessor(Func<HttpContext, DateTime> accessor)
    {
        _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
    }

    public Task<DateTime> GetAsync(HttpContext context)
    {
        return Task.FromResult(_accessor(context));
    }
}