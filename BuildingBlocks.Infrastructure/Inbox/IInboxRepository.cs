namespace BuildingBlocks.Infrastructure.Inbox;

public interface IInboxRepository
{
    Task<bool> IsProcessedAsync(Guid messageId, string handlerType);

    void Add(InboxMessage message);

    Task SaveChangesAsync();
}
