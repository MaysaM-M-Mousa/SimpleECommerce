namespace BuildingBlocks.Domain;



public abstract class AggregateRoot
{
    private List<IDomainEvent> _domainEvents = new();

    public void RaiseDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
        => _domainEvents.ToList();

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}

public abstract class AggregateRoot<T> : AggregateRoot
{
    public T Id { get; protected set; }

    public AggregateRoot(T id)
    {
        Id = id;
    }
}