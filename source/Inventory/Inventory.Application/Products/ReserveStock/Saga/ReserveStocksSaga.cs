using Inventory.IntegrationEvents;
using MassTransit;
using Order.IntegrationEvents;

namespace Inventory.Application.Products.ReserveStock.Saga;

public class ReserveStocksSaga : MassTransitStateMachine<ReserveStocksSagaState>
{
    // States
    public State Reservation { get; private set; }
    public State Releasing { get; private set; }
    public State Completed { get; private set; }
    public State Failed { get; private set; }

    // Events
    public Event<OrderPlacedIntegrationEvent> OrderPlacedEvent { get; private set; }
    public Event<StockReservedIntegrationEvent> StockReservedEvent { get; private set; }
    public Event<StockReleasedIntegrationEvent> StockReleasedEvent { get; private set; }

    public ReserveStocksSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderPlacedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => StockReservedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => StockReleasedEvent, x => x.CorrelateById(context => context.Message.OrderId));

        Initially(
          When(OrderPlacedEvent)
            .IfElse(context => context.Message.Items.Any(),
                x => x.Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.ReservationDetails.ProductsToReserve = context.Message.Items.Select(i => new ProductQuantity(i.ProductId, i.Quantity)).ToList();
                })
                .TransitionTo(Reservation)
                .ThenAsync(async context =>
                {
                    var nextProductToReserve = context.Saga.ReservationDetails.ProductsToReserve.First();
                    await context.Publish(new ReserveStockRequest(nextProductToReserve.ProductId, nextProductToReserve.Quantity, context.Saga.OrderId));
                }),
                x => x.TransitionTo(Failed))
        );

        During(Reservation,
            When(StockReservedEvent)
                .Then(context =>
                {
                    var reservedProduct = new ProductQuantity(context.Message.ProductId, context.Message.Quantity);
                    context.Saga.ReservationDetails.ReservedProducts.Add(reservedProduct);
                    context.Saga.ReservationDetails.ProductsToReserve.Remove(reservedProduct);
                })
                .IfElse(context => !context.Saga.ReservationDetails.ProductsToReserve.Any(),
                x => x.TransitionTo(Completed),
                x => x.ThenAsync(async context =>
                {
                    var nextProductToReserve = context.Saga.ReservationDetails.ProductsToReserve.First();
                    await context.Publish(new ReserveStockRequest(nextProductToReserve.ProductId, nextProductToReserve.Quantity, context.Saga.OrderId));
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
                x => !x.Saga.ReservationDetails.ReservedProducts.Any(), 
                x => x.TransitionTo(Completed),
                x => x.ThenAsync(async context =>
                {
                    var nextProductToRelease = context.Saga.ReservationDetails.ReservedProducts.First();
                    await context.Publish(new ReleaseStockRequest(nextProductToRelease.ProductId, nextProductToRelease.Quantity, context.Saga.OrderId));
                })));
    }
}
