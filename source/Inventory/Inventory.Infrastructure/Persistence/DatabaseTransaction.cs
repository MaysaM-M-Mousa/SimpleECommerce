using BuildingBlocks.Application.Database;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence;

internal class DatabaseTransaction : IDatabaseTransaction
{
    private readonly InventoryDbContext _context;

    public DatabaseTransaction(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task ExecuteInTransactionAsync(Func<Task> operation)
    {
        var existingTransaction = _context.Database.CurrentTransaction;
        if (existingTransaction != null)
        {
            await operation();
            return;
        }

        var executionStrategy = _context.Database.CreateExecutionStrategy();

        await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await operation();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
            }
        });
    }
}
