using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories.Abstracts;
using System;

namespace EventlyServer.Data.Repositories
{
    public static class RepositoryRegistrator
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) => services
            .AddScoped<IRepository<Guest>, DbRepository<Guest>>()
            .AddScoped<IRepository<LandingInvitation>, LandingInvitationRepository>()
            .AddScoped<IRepository<Template>, TemplateRepository>()
            .AddScoped<IRepository<TypesOfEvent>, DbRepository<TypesOfEvent>>()
            .AddScoped<IRepository<User>, UserRepository>();
    }
}
