using BuildingBlocks.Infrastructure.Inbox;
using Inventory.Domain.Products;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInventoryInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IInboxRepository, InboxRepository>();

        services.AddDbContext<InventoryDbContext>(
            opts => opts.UseSqlServer(configuration.GetConnectionString("InventoryDbConnectionString"), builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
            }));

        return services;
    }
}
