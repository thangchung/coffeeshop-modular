using CoffeeShop.Domain;
using CoffeeShop.Domain.DomainEvents;
using MediatR;
using N8T.Core.Repository;

namespace CoffeeShop.Counter.UseCases;

public class OrderUpUseCase : N8T.Infrastructure.Events.DomainEventHandler<OrderUp>
{
    private readonly IRepository<Order> _orderRepository;

    public OrderUpUseCase(IRepository<Order> orderRepository, IPublisher publisher) : base(publisher)
    {
        _orderRepository = orderRepository;
    }

    public override async Task HandleEvent(OrderUp @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);
        
        var order = await _orderRepository.FindById(@event.OrderId, cancellationToken);

        var orderUpdated = order.Apply(@event);
        await _orderRepository.EditAsync(orderUpdated, cancellationToken: cancellationToken);

        await RelayAndPublishEvents(order, cancellationToken);
    }
}
