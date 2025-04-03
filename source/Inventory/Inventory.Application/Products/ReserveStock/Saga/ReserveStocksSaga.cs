using Inventory.IntegrationEvents;
using MassTransit;
using Order.IntegrationEvents;

namespace Inventory.Application.Products.ReserveStock.Saga;

public class ReserveStocksSaga : MassTransitStateMachine<ReserveStocksSagaState>
{
    // States
    public State Reservation { get; private set; }
    public State Releasing { get; private set; }
    public State Failed { get; private set; }

    // Events
    public Event<OrderPlacedIntegrationEvent> OrderPlacedEvent { get; private set; }
    public Event<StockReservedIntegrationEvent> StockReservedEvent { get; private set; }
    public Event<StockReleasedIntegrationEvent> StockReleasedEvent { get; private set; }
    public Event<StocksReservationCompletedIntegrationEvent> StocksReservationCompletedEvent { get; private set; }
    public Event<StocksReservationFailedIntegrationEvent> StocksReservationFailedEvent { get; private set; }
    public Schedule<ReserveStocksSagaState, ReservationTimeoutExpiredIntegrationEvent> ReservationTimeoutExpiredEvent { get; private set; }

    public ReserveStocksSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderPlacedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => StockReservedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => StockReleasedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => StocksReservationCompletedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => StocksReservationFailedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Schedule(
            () => ReservationTimeoutExpiredEvent, 
            x => x.ReservationTimeoutToken, 
            x =>
            {
                x.Delay = TimeSpan.FromMinutes(10);
                x.Received = r => r.CorrelateById(m => m.Message.OrderId);
            });

        Initially(
          When(OrderPlacedEvent)
            .IfElse(
              condition: context => context.Message.Items.Any(),
              thenActivityCallback: context => context.Then(context =>
              {
                  context.Saga.OrderId = context.Message.OrderId;
                  context.Saga.ReservationDetails.ProductsToReserve = context.Message.Items.Select(i => new ProductQuantity(i.ProductId, i.Quantity)).ToList();
              })
              .TransitionTo(Reservation)
              .Schedule(ReservationTimeoutExpiredEvent, context => new ReservationTimeoutExpiredIntegrationEvent
              {
                  MessageId = Guid.NewGuid(),
                  OrderId = context.Saga.CorrelationId
              })
              .ThenAsync(async context =>
              {
                  var nextProductToReserve = context.Saga.ReservationDetails.ProductsToReserve.First();
                  await context.Publish(new ReserveStockRequest(nextProductToReserve.ProductId, nextProductToReserve.Quantity, context.Saga.OrderId));
              }),
              elseActivityCallback: context => context
              .TransitionTo(Failed)
              .ThenAsync(async context => await context.Publish(new StocksReservationFailedIntegrationEvent
              {
                  MessageId = Guid.NewGuid(),
                  OrderId = context.Saga.CorrelationId,
                  Reason = "No products to reserve"
              })))
        );

        During(Reservation,
            When(StockReservedEvent)
                .Then(context =>
                {
                    var reservedProduct = new ProductQuantity(context.Message.ProductId, context.Message.Quantity);
                    context.Saga.ReservationDetails.ReservedProducts.Add(reservedProduct);
                    context.Saga.ReservationDetails.ProductsToReserve.Remove(reservedProduct);
                })
                .IfElse(
                condition: context => !context.Saga.ReservationDetails.ProductsToReserve.Any(),
                thenActivityCallback: context => context
                .Unschedule(ReservationTimeoutExpiredEvent)
                .ThenAsync(async context => await context.Publish(new StocksReservationCompletedIntegrationEvent
                {
                    MessageId = Guid.NewGuid(),
                    OrderId = context.Saga.CorrelationId,
                }))
                .Finalize(),
                elseActivityCallback: context => context.ThenAsync(async context =>
                {
                    var nextProductToReserve = context.Saga.ReservationDetails.ProductsToReserve.First();
                    await context.Publish(new ReserveStockRequest(nextProductToReserve.ProductId, nextProductToReserve.Quantity, context.Saga.OrderId));
                })),

            When(ReservationTimeoutExpiredEvent!.Received)
            .IfElse(condition: context => !context.Saga.ReservationDetails.ReservedProducts.Any(),
            thenActivityCallback: context => context
            .TransitionTo(Failed)
            .ThenAsync(async context => await context.Publish(new StocksReservationFailedIntegrationEvent
            {
                MessageId = Guid.NewGuid(),
                OrderId = context.Saga.CorrelationId,
                Reason = "Reservation timeout expired"
            })),
            elseActivityCallback: context => context
            .TransitionTo(Releasing)
            .ThenAsync(async context =>
            {
                var nextProductToRelease = context.Saga.ReservationDetails.ReservedProducts.First();
                await context.Publish(new ReleaseStockRequest(nextProductToRelease.ProductId, nextProductToRelease.Quantity, context.Saga.OrderId));
            }))
        );

        During(Releasing,
            When(StockReleasedEvent)
            .Then(context =>
            {
                var releasedProduct = new ProductQuantity(context.Message.ProductId, context.Message.Quantity);
                context.Saga.ReservationDetails.ReleasedProducts.Add(releasedProduct);
                context.Saga.ReservationDetails.ReservedProducts.Remove(releasedProduct);
            })
            .IfElse(
                condition: context => !context.Saga.ReservationDetails.ReservedProducts.Any(), 
                thenActivityCallback: context => context
                .TransitionTo(Failed)
                .ThenAsync(async context => await context.Publish(new StocksReservationFailedIntegrationEvent
                {
                    MessageId = Guid.NewGuid(),
                    OrderId = context.Saga.CorrelationId,
                    Reason = "Reservation timeout expired"
                })),
                elseActivityCallback: context => context.ThenAsync(async context =>
                {
                    var nextProductToRelease = context.Saga.ReservationDetails.ReservedProducts.First();
                    await context.Publish(new ReleaseStockRequest(nextProductToRelease.ProductId, nextProductToRelease.Quantity, context.Saga.OrderId));
                })));

        During(Failed,
            Ignore(StocksReservationFailedEvent));

        During(Final,
            Ignore(StocksReservationCompletedEvent),
            Ignore(StocksReservationFailedEvent));
    }
}

/*
 TODO:
    1. Scheduled message                            -- done
    2. Enhancements                                 -- done
    3. Fire completion events                       -- done
    4. Outbox pattern for transactional messaging
    5. Idempotency
    6. Retries
    7. Concurrency
 */