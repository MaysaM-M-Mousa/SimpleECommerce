using BuildingBlocks.Application.Idempotency;
using Inventory.Application.Products.DeductStock;
using Inventory.Application.Products.ReleaseStock;
using MassTransit;
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

        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderPlacedIntegrationEventHandler>();
            x.AddConsumer<OrderCancelledIntegrationEventHandler>();

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["RabbitMq:ConnectionString"], h =>
                {
                    h.Username(configuration["RabbitMq:Username"]);
                    h.Password(configuration["RabbitMq:Password"]);
                });

                config.UseConsumeFilter(typeof(IdempotentIntegrationEventFilter<>), context);
                //config.ConfigureEndpoints(context);

                config.ReceiveEndpoint("order-placed-queue", e =>
                {
                    e.ConfigureConsumer<OrderPlacedIntegrationEventHandler>(context);
                });

                config.ReceiveEndpoint("order-cancelled-queue", e =>
                {
                    e.ConfigureConsumer<OrderCancelledIntegrationEventHandler>(context);
                });
            });
        });

        return services;
    }
}
