using EventlyServer.Data;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventlyServerTest;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        InitDatabase.RunScript().Wait();

        services.AddDbContext<InHolidayContext>();
        services.AddScoped<IRepository<Guest>, DbRepository<Guest>>();
        services.AddScoped<IRepository<LandingInvitation>, LandingInvitationRepository>();
        services.AddScoped<IRepository<User>, UserRepository>();
        services.AddScoped<GuestService>();
        services.AddScoped<LandingInvitationService>();
    }
}