using CoffeeShop.Domain;
using CoffeeShop.Domain.Commands;
using MediatR;
using N8T.Core.Repository;

namespace CoffeeShop.Counter.UseCases;

public class OrderInUseCase : N8T.Infrastructure.Events.RequestHandler<PlaceOrderCommand, IResult>
{
    private readonly IRepository<Order> _orderRepository;

    public OrderInUseCase(IRepository<Order> orderRepository, IPublisher publisher) : base(publisher)
    {
        _orderRepository = orderRepository;
    }

    public override async Task<IResult> Handle(PlaceOrderCommand placeOrderCommand, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(placeOrderCommand);

        var order = Order.From(placeOrderCommand);
        await _orderRepository.AddAsync(order, cancellationToken: cancellationToken);

        await RelayAndPublishEvents(order, cancellationToken);

        return Results.Ok();
    }
}
