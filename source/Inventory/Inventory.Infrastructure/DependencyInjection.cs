using BuildingBlocks.Application.Database;
using BuildingBlocks.Application.Inbox;
using BuildingBlocks.Application.Outbox;
using Inventory.Domain.Products;
using Inventory.Infrastructure.BackgroundJobs;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Persistence.Interceptors;
using Inventory.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInventoryInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IInboxRepository, InboxRepository>();
        services.AddScoped<IDatabaseTransaction, DatabaseTransaction>();
        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
        services.AddSingleton<OutboxInterceptor>();

        services.AddDbContext<InventoryDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("InventoryDbConnectionString"), builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
            });
            options.AddInterceptors(sp.GetRequiredService<OutboxInterceptor>());
        });

        services.AddQuartz();
        services.AddMassTransitConfigs(configuration);

        return services;
    }

    private static IServiceCollection AddQuartz(this IServiceCollection services)
    {
        services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            config
            .AddJob<ProcessOutboxMessagesJob>(jobKey)
            .AddTrigger(t => t.ForJob(jobKey).WithSimpleSchedule(s => s.WithIntervalInSeconds(10).RepeatForever()));

        });

        services.AddQuartzHostedService();

        return services;
    }

    private static IServiceCollection AddMassTransitConfigs(
        this IServiceCollection services,
        IConfiguration configuration)
    {
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

                //config.UseConsumeFilter(typeof(IdempotentIntegrationEventFilter<>), context);
                //config.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
