using BuildingBlocks.Application.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Order.Persistence.Outbox;

internal class OutboxMessageRepository : IOutboxMessageRepository
{
    private readonly OrdersDbContext _context;

    public OutboxMessageRepository(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<List<OutboxMessage>> GetUnprocessedMessagesAsync()
        => await _context
            .OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(10)
            .ToListAsync();

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
