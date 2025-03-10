using BuildingBlocks.Application.Inbox;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

internal class InboxRepository : IInboxRepository
{
    private readonly InventoryDbContext _context;

    public InboxRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsProcessedAsync(Guid messageId, string handlerType)
    {
        return await _context
            .InboxMessages
            .AnyAsync(m => m.MessageId == messageId && m.HandlerType == handlerType);
    }

    public void Add(InboxMessage message)
    {
        _context.InboxMessages.Add(message);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
