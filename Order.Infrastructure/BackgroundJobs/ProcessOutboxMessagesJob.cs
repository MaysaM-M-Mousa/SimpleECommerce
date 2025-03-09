using MediatR;
using Order.Application.Outbox;
using Quartz;
using System.Text.Json;

namespace Order.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
internal class ProcessOutboxMessagesJob : IJob
{
    private readonly IPublisher _publisher;
    private readonly IOutboxMessageRepository _outboxMessageRepository;

    public ProcessOutboxMessagesJob(
        IPublisher publisher, 
        IOutboxMessageRepository outboxMessageRepository)
    {
        _publisher = publisher;
        _outboxMessageRepository = outboxMessageRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _outboxMessageRepository.GetUnprocessedMessagesAsync();

        foreach (var message in messages)
        {
            try
            {
                var domainEvent = JsonSerializer.Deserialize(message.Content, Type.GetType(message.Type));

                await _publisher.Publish(domainEvent);

                message.ProcessedOnUtc = DateTime.UtcNow;

                await _outboxMessageRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                message.Error = ex.Message;

                await _outboxMessageRepository.SaveChangesAsync();
            }
        }

    }
}
