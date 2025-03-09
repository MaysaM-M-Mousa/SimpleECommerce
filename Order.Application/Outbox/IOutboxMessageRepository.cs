namespace Order.Application.Outbox;

public interface IOutboxMessageRepository
{ 
    Task<List<OutboxMessage>> GetUnprocessedMessagesAsync();

    public Task SaveChangesAsync();
}
