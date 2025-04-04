using BuildingBlocks.IntegrationEvent;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Order.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderApplication(
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
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["RabbitMq:ConnectionString"], h =>
                {
                    h.Username(configuration["RabbitMq:Username"]);
                    h.Password(configuration["RabbitMq:Password"]);
                });

                config.Publish<IntegrationEvent>(s => s.Exclude = true);

                config.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
