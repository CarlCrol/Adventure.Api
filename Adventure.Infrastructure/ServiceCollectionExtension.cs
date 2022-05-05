using Adventure.Core.Administration.Queries;
using Adventure.Core.Profiles;
using Adventure.Core.Repositories;
using Adventure.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adventure.Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection Initialize(this IServiceCollection services, IConfiguration configuration)
    {
        // initialize persistence
        var connectionString = configuration.GetValue<string>("ConnectionStrings:ConnectionString");
        services.AddDbContext<Persistence.AdventureDbContext>(options => options.UseSqlite(connectionString));

        // initialize respositories
        services.AddScoped<IAdventureRepository, AdventureRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserAdventureRepository, UserAdventureRepository>();

        // initialize mediatR
        services.AddMediatR(typeof(GetAdventureQuery).Assembly);

        // Add Mapper
        services.AddAutoMapper(cfg => cfg.AddProfile<DomainProfile>());
        
        return services;
    }
}