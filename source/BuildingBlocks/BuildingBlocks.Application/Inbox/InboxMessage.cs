namespace BuildingBlocks.Application.Inbox;

public class InboxMessage
{
    public Guid Id { get; set; }

    public Guid MessageId { get; set; }

    public string HandlerType { get; set; }

    public DateTime OccurredOnUtc { get; set; }
}
