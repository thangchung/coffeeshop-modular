using CoffeeShop.Domain;
using CoffeeShop.Domain.Commands;
using FluentValidation;
using MediatR;
using N8T.Core.Domain;
using N8T.Core.Repository;

namespace CoffeeShop.Counter.Features;

public static class OrderInRouteMapper
{
    public static IEndpointRouteBuilder MapOrderInApiRoutes(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/v1/api/orders", async (PlaceOrderCommand command, ISender sender) => await sender.Send(command));
        return builder;
    }
}

internal class OrderInValidator : AbstractValidator<PlaceOrderCommand>
{
    public OrderInValidator()
    {
    }
}

public class PlaceOrder : IRequestHandler<PlaceOrderCommand, IResult>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IPublisher _publisher;

    public PlaceOrder(IRepository<Order> orderRepository, IPublisher publisher)
    {
        _orderRepository = orderRepository;
        _publisher = publisher;
    }

    public async Task<IResult> Handle(PlaceOrderCommand placeOrderCommand, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(placeOrderCommand);

        var order = Order.From(placeOrderCommand);
        await _orderRepository.AddAsync(order, cancellationToken: cancellationToken);

        await order.RelayAndPublishEvents(_publisher, cancellationToken);

        return Results.Ok();
    }
}
