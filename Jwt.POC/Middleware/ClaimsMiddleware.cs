using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Jwt.POC.Middleware
{
    public class ClaimsMiddleware
    {
        private readonly RequestDelegate _next;
        public ClaimsMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            await _next(context);
        }
    }
}
