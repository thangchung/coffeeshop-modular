using CoffeeShop.Domain;
using CoffeeShop.Domain.DomainEvents;
using CoffeeShop.Kitchen.Domain;
using MediatR;
using N8T.Core.Domain;
using N8T.Core.Repository;

namespace CoffeeShop.Kitchen.UseCases;

public class OrderInUseCase : N8T.Infrastructure.Events.DomainEventHandler<KitchenOrderIn>
{
    private readonly IRepository<KitchenOrder> _kitchenOrderRepository;
    private readonly IPublisher _publisher;

    public OrderInUseCase(IRepository<KitchenOrder> kitchenOrderRepository, IPublisher publisher) 
    {
        _kitchenOrderRepository = kitchenOrderRepository;
        _publisher = publisher;
    }
    
    public override async Task HandleEvent(KitchenOrderIn @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);

        var kitchenOrder = KitchenOrder.From(@event.OrderId, @event.ItemType, DateTime.UtcNow);
        
        await Task.Delay(CalculateDelay(@event.ItemType), cancellationToken);
        
        kitchenOrder.SetTimeUp(@event.ItemLineId, DateTime.UtcNow);
        
        await _kitchenOrderRepository.AddAsync(kitchenOrder, cancellationToken: cancellationToken);

        await kitchenOrder.RelayAndPublishEvents(_publisher, cancellationToken);
    }

    private static TimeSpan CalculateDelay(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.CROISSANT => TimeSpan.FromSeconds(7),
            ItemType.CROISSANT_CHOCOLATE => TimeSpan.FromSeconds(7),
            ItemType.CAKEPOP => TimeSpan.FromSeconds(5),
            ItemType.MUFFIN => TimeSpan.FromSeconds(7),
            _ => TimeSpan.FromSeconds(3)
        };
    }
}
