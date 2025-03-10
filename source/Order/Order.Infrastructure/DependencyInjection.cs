using Microsoft.Extensions.DependencyInjection;
using Order.Infrastructure.BackgroundJobs;
using Quartz;

namespace Order.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderInfrastructure(this IServiceCollection services)
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
}
