using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddInventoryApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            var assembly = typeof(DependencyInjection).Assembly;
            config.RegisterServicesFromAssembly(assembly);
        });

        return services;
    }
}
