﻿using Microsoft.EntityFrameworkCore.Diagnostics;
using Order.Application.Outbox;
using Order.Domain.Primitives;
using System.Text.Json;
namespace Order.Persistence.Interceptors;

internal class OutboxInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var events = dbContext
            .ChangeTracker
            .Entries<AggregateRoot>()
            .Select(e => e.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.GetDomainEvents();

                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        var outboxMessages = events
            .Select(e => new OutboxMessage()
            {
                Id = e.Id,
                OccurredOnUtc = DateTime.UtcNow,
                Type = e.GetType().AssemblyQualifiedName,
                Content = JsonSerializer.Serialize(e, e .GetType())
            }).ToList();

        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
