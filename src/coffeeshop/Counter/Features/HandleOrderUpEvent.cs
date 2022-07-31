using CoffeeShop.Domain;
using CoffeeShop.Domain.DomainEvents;
using MediatR;
using N8T.Core.Domain;
using N8T.Core.Repository;

namespace CoffeeShop.Counter.Features;

public class HandleOrderUpEvent : N8T.Infrastructure.Events.DomainEventHandler<OrderUp>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IPublisher _publisher;

    public HandleOrderUpEvent(IRepository<Order> orderRepository, IPublisher publisher)
    {
        _orderRepository = orderRepository;
        _publisher = publisher;
    }

    public override async Task HandleEvent(OrderUp @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);
        
        var order = await _orderRepository.FindById(@event.OrderId, cancellationToken);

        var orderUpdated = order.Apply(@event);
        await _orderRepository.EditAsync(orderUpdated, cancellationToken: cancellationToken);

        await order.RelayAndPublishEvents(_publisher, cancellationToken);
    }
}
