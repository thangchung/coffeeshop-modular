using CoffeeShop.Domain;
using MediatR;
using N8T.Core.Repository;
using N8T.Core.Specification;
using System.Linq.Expressions;

namespace CoffeeShop.Counter.UseCases;

public static class OrderFulfillmentRouteMapper
{
    public static IEndpointRouteBuilder MapOrderFulfillmentApiRoutes(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/v1/api/fulfillment-orders", async (ISender sender) => await sender.Send(new OrderFulfillmentQuery()));
        return builder;
    }
}

public record OrderFulfillmentQuery : IRequest<IResult>
{
}

public class OrderFulfillmentSpec : SpecificationBase<Order>
{
    public OrderFulfillmentSpec()
    {
        AddInclude(x => x.LineItems);
    }
    
    public override Expression<Func<Order, bool>> Criteria => x => x.OrderStatus == OrderStatus.FULFILLED;
}

public class QueryOrderFulfillmentUseCase : N8T.Infrastructure.Events.RequestHandler<OrderFulfillmentQuery, IResult>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly ILogger<QueryOrderFulfillmentUseCase> _logger;

    public QueryOrderFulfillmentUseCase(
        IRepository<Order> orderRepository,
        IPublisher publisher,
        ILogger<QueryOrderFulfillmentUseCase> logger) : base(publisher)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public override async Task<IResult> Handle(OrderFulfillmentQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        var orders = await _orderRepository.FindAsync(new OrderFulfillmentSpec(), cancellationToken);
        return Results.Ok(orders);
    }
}