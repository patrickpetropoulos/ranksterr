using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ranksterr.Application.Abstractions.Clock;
using Ranksterr.Application.Abstractions.Data;
using Ranksterr.Domain.Abstractions;
using Ranksterr.Domain.Item;
using Ranksterr.Infrastructure.Clock;
using Ranksterr.Infrastructure.Data;
using Ranksterr.Infrastructure.Repositories;

namespace Ranksterr.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastucture(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        AddPersistance(services, configuration);

        return services;
    }

    private static void AddPersistance(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ??
                               throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, b=> b.MigrationsAssembly("Ranksterr.Server.Api"));
        });

        services.AddScoped<IItemRepository, ItemRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(connectionString));
        
    }
}