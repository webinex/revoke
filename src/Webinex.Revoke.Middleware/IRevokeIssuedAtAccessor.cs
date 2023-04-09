using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Webinex.Revoke.Middleware
{
    public interface IRevokeIssuedAtAccessor
    {
        Task<DateTime> GetAsync(HttpContext context);
    }
}