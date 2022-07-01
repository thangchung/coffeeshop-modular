using CoffeeShop.Domain;
using CoffeeShop.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using N8T.Core.Repository;

namespace CoffeeShop.Counter.UseCases;

public class OrderInUseCase : IRequestHandler<PlaceOrderCommand, IResult>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
    private readonly IPublisher _publisher;

    public OrderInUseCase(IRepository<Order> orderRepository, IHubContext<NotificationHub, INotificationClient> hubContext, IPublisher publisher)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException();
        _hubContext = hubContext;
        _publisher = publisher;
    }
    
    public async Task<IResult> Handle(PlaceOrderCommand placeOrderCommand, CancellationToken cancellationToken)
    {
        if (placeOrderCommand is null)
        {
            throw new ArgumentNullException(nameof(placeOrderCommand));
        }

        var orderEventResult = Order.From(placeOrderCommand);
        await _orderRepository.AddAsync(orderEventResult.Order);

        foreach (var orderUpdate in orderEventResult.OrderUpdates)
        {
            await _hubContext.Clients.All.SendMessage($"{orderUpdate.OrderId}-{orderUpdate.ItemLineId}-{orderUpdate.Name}-{orderUpdate.OrderStatus}");
        }

        if (orderEventResult.BaristaTickets.Any())
        {
            foreach (var ticket in orderEventResult.BaristaTickets)
            {
                await _publisher.Publish(ticket);
            }
        }

        if (orderEventResult.KitchenTickets.Any())
        {
            foreach (var ticket in orderEventResult.KitchenTickets)
            {
                await _publisher.Publish(ticket);
            }
        }

        return Results.Ok();
    }
}
