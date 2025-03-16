using BuildingBlocks.Domain;

namespace Inventory.Domain.Products.DomainEvents;

public record ProductCreatedDomainEvent(
    int ProductId, 
    string Name, 
    string Description, 
    int Quantity, 
    decimal Price) : 
    DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
