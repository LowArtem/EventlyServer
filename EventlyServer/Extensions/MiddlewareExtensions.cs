using EventlyServer.Services.Security;
using Microsoft.AspNetCore.Antiforgery;

namespace EventlyServer.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseXsrfProtection(this IApplicationBuilder builder, IAntiforgery antiforgery)
        => builder.UseMiddleware<XsrfProtectionMiddleware>(antiforgery);
}