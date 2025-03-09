using BuildingBlocks.Application.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Domain.Orders;
using Order.Persistence.Interceptors;
using Order.Persistence.Orders;
using Order.Persistence.Outbox;

namespace Order.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddSingleton<OutboxInterceptor>();

        services.AddDbContext<OrdersDbContext>((sp, options)=>
        {
            options.UseSqlServer(configuration.GetConnectionString("OrdersDbConnectionString"), builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
            });
            options.AddInterceptors(sp.GetRequiredService<OutboxInterceptor>());
        });

        return services;
    }
}
