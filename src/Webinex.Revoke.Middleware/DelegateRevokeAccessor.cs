using Microsoft.AspNetCore.Http;

namespace Webinex.Revoke.Middleware;

internal class DelegateRevokeAccessor : IRevokeAccessor
{
    private readonly Func<HttpContext, IEnumerable<RevokeId>> _accessor;

    public DelegateRevokeAccessor(Func<HttpContext, IEnumerable<RevokeId>> accessor)
    {
        _accessor = accessor;
    }

    public Task<IEnumerable<RevokeId>> GetAsync(HttpContext context)
    {
        return Task.FromResult(_accessor(context));
    }
}