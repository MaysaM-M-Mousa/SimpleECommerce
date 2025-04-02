using MassTransit;

namespace Inventory.Application.Products.ReserveStock.Saga;

public class ReserveStocksSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; }

    public Guid OrderId { get; set; }

    public ReservationDetails ReservationDetails { get; set; } = new();

}

public class ReservationDetails
{
    public List<ProductQuantity> ReservedProducts { get; set; } = [];

    public List<ProductQuantity> ProductsToReserve { get; set; } = [];

    public List<ProductQuantity> ProductsToRelease { get; set; } = [];
}

public record ProductQuantity(int ProductId, int Quantity);


// Requests - Commands
public record ReserveStockRequest(int ProductId, int Quantity, Guid OrderId);

public record ReleaseStockRequest(int ProductId, int Quantity, Guid OrderId);