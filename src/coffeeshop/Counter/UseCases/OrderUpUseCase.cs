using CoffeeShop.Domain;
using CoffeeShop.Domain.ValueObjects;
using CoffeeShop.Infrastructure.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using N8T.Core.Repository;

namespace CoffeeShop.Counter.UseCases;

public class OrderUpUseCase : INotificationHandler<OrderUp>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

    public OrderUpUseCase(IRepository<Order> orderRepository, IHubContext<NotificationHub, INotificationClient> hubContext)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException();
        _hubContext = hubContext ?? throw new ArgumentNullException();
    }
    
    public async Task Handle(OrderUp notification, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindById(notification.OrderId);
        
        var orderResult = order.Apply(notification);
        await _orderRepository.EditAsync(orderResult.Order);

        foreach (var orderUpdate in orderResult.OrderUpdates)
        {
            await _hubContext.Clients.All.SendMessage($"{orderUpdate.OrderId}-{orderUpdate.ItemLineId}-{Item.GetItem(orderUpdate.ItemType)?.ToString()}-{orderUpdate.OrderStatus}");
        }
    }
}
