using BuildingBlocks.Application.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

internal class OutboxMessageRepository : IOutboxMessageRepository
{
    private readonly InventoryDbContext _context;

    public OutboxMessageRepository(InventoryDbContext context)
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
