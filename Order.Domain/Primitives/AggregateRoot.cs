namespace Order.Domain.Primitives;

public abstract class AggregateRoot
{
    public Guid Id { get; set; }

    private List<IDomainEvent> _domainEvents = new();

    public void RaiseDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() 
        => _domainEvents.ToList();

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}
