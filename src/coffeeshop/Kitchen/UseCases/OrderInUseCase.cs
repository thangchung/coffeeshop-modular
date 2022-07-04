using CoffeeShop.Domain;
using CoffeeShop.Domain.ValueObjects;
using CoffeeShop.Kitchen.Domain;
using MediatR;
using N8T.Core.Repository;

namespace CoffeeShop.Kitchen.UseCases;

public class OrderInUseCase : INotificationHandler<KitchenOrderIn>
{
    private readonly IRepository<KitchenOrder> _kitchenOrderRepository;
    private readonly IPublisher _publisher;

    public OrderInUseCase(IRepository<KitchenOrder> kitchenOrderRepository, IPublisher publisher)
    {
        _kitchenOrderRepository = kitchenOrderRepository;
        _publisher = publisher;
    }
    
    public async Task Handle(KitchenOrderIn orderIn, CancellationToken cancellationToken)
    {
        var kitchenOrder = new KitchenOrder(orderIn.OrderId, orderIn.ItemType, DateTime.UtcNow);
        await Task.Delay(CalculateDelay(orderIn.ItemType), cancellationToken);
        
        var orderUp = new OrderUp(orderIn.OrderId, orderIn.ItemLineId, Item.GetItem(orderIn.ItemType)?.ToString()!, orderIn.ItemType, DateTime.UtcNow, "teesee");
        kitchenOrder.TimeUp = DateTime.UtcNow;

        await _kitchenOrderRepository.AddAsync(kitchenOrder);

        await _publisher.Publish(orderUp);
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
