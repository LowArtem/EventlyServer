using EventlyServer.Services.Security;

namespace EventlyServer.Services;

public static class ServiceRegistrator
{
    public static IServiceCollection AddServices(this IServiceCollection services) => services
        .AddScoped<UserService>()
        .AddScoped<TokenService>();
}