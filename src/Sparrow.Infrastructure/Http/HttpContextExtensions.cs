using Microsoft.AspNetCore.Http;

namespace Sparrow.Infrastructure.Http
{
    public static class HttpContextExtensions
    {
        public static string GetClientIp(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
