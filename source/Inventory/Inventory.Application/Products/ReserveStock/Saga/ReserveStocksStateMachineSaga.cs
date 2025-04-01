using MassTransit;

namespace Inventory.Application.Products.ReserveStock.Saga;

public class ReserveStocksStateMachineSaga : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; }

    public Guid OrderId { get; set; }

    public List<ProductQuantity> ReservedProducts { get; set; } = new();

    public List<ProductQuantity> ProductsToReserve { get; set; } = new();

    public List<ProductQuantity> ProductsToRelease { get; set; } = new();
}

public record ProductQuantity(int ProductId, int Quantity);


// Requests - Commands
public record ReserveStockRequest(int ProductId, int Quantity, Guid OrderId);

public record ReleaseStockRequest(int ProductId, int Quantity, Guid OrderId);