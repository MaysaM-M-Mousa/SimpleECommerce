using BuildingBlocks.Application.Database;
using BuildingBlocks.Application.Inbox;
using MassTransit;

namespace BuildingBlocks.Application.Idempotency;

public class IdempotentIntegrationEventFilter<T> : IFilter<ConsumeContext<T>> where T : IntegrationEvent.IntegrationEvent
{
    private readonly IInboxRepository _inboxRepository;
    private readonly IDatabaseTransaction _databaseTransaction;

    public IdempotentIntegrationEventFilter(
        IInboxRepository inboxRepository, 
        IDatabaseTransaction databaseTransaction)
    {
        _inboxRepository = inboxRepository;
        _databaseTransaction = databaseTransaction;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("IdempotentIntegrationEventFilter");
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        // TODO: extract consumer type here and pass it
        string handlerType = "UnknownHandler";

        var isProcessed = await _inboxRepository.IsProcessedAsync(context.Message.MessageId, handlerType);

        if (isProcessed)
        {
            return;
        }

        var inboxMessage = new InboxMessage 
        {
            Id = Guid.NewGuid(),
            MessageId = context.Message.MessageId,
            HandlerType = handlerType,
            OccurredOnUtc = DateTime.UtcNow
        };

        await _databaseTransaction.ExecuteInTransactionAsync(async () =>
        {
            _inboxRepository.Add(inboxMessage);
            await next.Send(context);
        });
    }
}
