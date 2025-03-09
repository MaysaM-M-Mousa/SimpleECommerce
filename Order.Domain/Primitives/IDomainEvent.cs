using MediatR;

namespace Order.Domain.Primitives;

public interface IDomainEvent : INotification
{
    public Guid Id { get; }
};

public record DomainEvent(Guid Id, DateTime OccurredOn) : IDomainEvent
{
    Guid IDomainEvent.Id => Id;
}
