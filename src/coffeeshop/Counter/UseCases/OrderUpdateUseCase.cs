using CoffeeShop.Domain;
using CoffeeShop.Domain.DomainEvents;
using CoffeeShop.Infrastructure.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeShop.Counter.UseCases
{
    public class OrderUpdateUseCase : N8T.Infrastructure.Events.DomainEventHandler<OrderUpdate>
    {
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

        public OrderUpdateUseCase(IHubContext<NotificationHub, INotificationClient> hubContext, IPublisher publisher) 
            : base(publisher)
        {
            _hubContext = hubContext;
        }

        public override async Task HandleEvent(OrderUpdate @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);
            
            var message = $"{@event.OrderId}-{@event.ItemLineId}-{Item.GetItem(@event.ItemType)?.ToString()}-{@event.OrderStatus}";
            Console.WriteLine(message);
            await _hubContext.Clients.All.SendMessage(message);
        }
    }
}
