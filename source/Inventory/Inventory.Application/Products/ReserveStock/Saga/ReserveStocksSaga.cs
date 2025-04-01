using Inventory.IntegrationEvents;
using MassTransit;
using Order.IntegrationEvents;

namespace Inventory.Application.Products.ReserveStock.Saga;

public class ReserveStocksSaga : MassTransitStateMachine<ReserveStocksStateMachineSaga>
{
    // States
    public State Reservation { get; private set; }
    public State Completed { get; private set; }
    public State Failed { get; private set; }

    // Events
    public Event<OrderPlacedIntegrationEvent> OrderPlacedEvent { get; private set; }
    public Event<StockReservedIntegrationEvent> StockReservedEvent { get; private set; }

    public ReserveStocksSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderPlacedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => StockReservedEvent, x => x.CorrelateById(context => context.Message.OrderId));

        Initially(
          When(OrderPlacedEvent)
            .IfElse(context => context.Message.Items.Any(),
                x => x.Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.ProductsToReserve = context.Message.Items.Select(i => new ProductQuantity(i.ProductId, i.Quantity)).ToList();
                })
                .ThenAsync(async context =>
                {
                    var nextProductToReserve = context.Saga.ProductsToReserve.First();
                    await context.Publish(new ReserveStockRequest(nextProductToReserve.ProductId, nextProductToReserve.Quantity, context.Saga.OrderId));
                })
                .TransitionTo(Reservation),
                x => x.TransitionTo(Failed))
        );

        During(Reservation,
            When(StockReservedEvent)
                .Then(context =>
                {
                    var reservedProduct = new ProductQuantity(context.Message.ProductId, context.Message.Quantity);
                    context.Saga.ProductsToReserve.Remove(reservedProduct);
                    context.Saga.ReservedProducts.Add(reservedProduct);
                })
                .IfElse(context => !context.Saga.ProductsToReserve.Any(),
                x => x.TransitionTo(Completed),
                x => x.ThenAsync(async context =>
                {
                    var nextProductToReserve = context.Saga.ProductsToReserve.First();
                    await context.Publish(new ReserveStockRequest(nextProductToReserve.ProductId, nextProductToReserve.Quantity, context.Saga.OrderId));
                }))
        );
    }
}
