namespace BuildingBlocks.Application.Database;

public interface IDatabaseTransaction
{
    Task ExecuteInTransactionAsync(Func<Task> operation);
}
