# Transactional Outbox & Inbox Patterns
This branch of the repository demonstrates the Transactional Outbox and Inbox patterns—two essential reliability patterns for modern, distributed, event-driven systems.
In this example, two services are implemented:
* `Order` service - implements the **Outbox** pattern.
* `Inventory` service -implements the **Inbox** pattern.

# Main Workflow
When an order is placed in the **Order service**, it triggers an integration event that gets published to a message broker. The **Inventory service** listens to this event and reacts by reserving or deducting stock accordingly.

# Technologies
* **Microsoft SQL Server** - for persistence.
* **RabbitMQ** - as the message broker.
* **MassTransit** - for service bus and message handling.


# Transactional Outbox Pattern
A reliability pattern that ensures atomic persistence of domain changes and event publication. This pattern prevents message loss and duplication by writing both domain data and event messages within the same database transaction.

### Implementation Details
* When a command is applied to the `Order` aggregate, it mutates the state and appends a domain event.
* An EF Core interceptor captures these events before saving and generates corresponding outbox messages.
  * [Outbox Interceptor Implementation](https://github.com/MaysaM-M-Mousa/SimpleECommerce/blob/outbox-inbox-demo/source/Order/Order.Persistence/Interceptors/OutboxInterceptor.cs)
* These messages are added to the Outbox table within the **same transaction**.
* A background worker polls this table for new events and publishes them to the message broker.
  * [Outbox Publisher Job](https://github.com/MaysaM-M-Mousa/SimpleECommerce/blob/outbox-inbox-demo/source/Order/Order.Infrastructure/BackgroundJobs/ProcessOutboxMessagesJob.cs)

# Inbox Pattern
A pattern that guarantees exactly-once processing of incoming messages by tracking their processing status in a local state table.

### Implementation Details
* A MassTransit filter is injected into the consumer pipeline of the Inventory service.
  * [idempotency filter implementation](https://github.com/MaysaM-M-Mousa/SimpleECommerce/blob/outbox-inbox-demo/source/BuildingBlocks/BuildingBlocks.Application/Idempotency/IdempotentIntegrationEventFilter.cs)
* For each message, the filter checks if the message has already been processed (based on MessageId).
  * If yes, it skips processing.
  * If no, it processes the message and records it in the Inbox table as part of the same transaction.
* A unique constraint on the MessageId column prevents concurrent or duplicate message processing.

# Further Reading
For a deeper dive into the theory behind these patterns, check out this presentation on [Transactional Outbox & Inbox patterns - Slideshare](https://www.slideshare.net/slideshow/transactional-outbox-inbox-patterns-pptx/277579109)
