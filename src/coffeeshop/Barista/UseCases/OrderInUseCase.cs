using CoffeeShop.Barista.Domain;
using CoffeeShop.Domain;
using CoffeeShop.Domain.ValueObjects;
using MediatR;
using N8T.Core.Repository;

namespace CoffeeShop.Barista.UseCases;

public class OrderInUseCase : INotificationHandler<BaristaOrderIn>
{
    private readonly IRepository<BaristaItem> _baristaItemRepository;
    private readonly IPublisher _publisher;

    public OrderInUseCase(IRepository<BaristaItem> baristaItemRepository, IPublisher publisher)
    {
        _baristaItemRepository = baristaItemRepository;
        _publisher = publisher;
    }
    
    public async Task Handle(BaristaOrderIn orderIn, CancellationToken cancellationToken)
    {
        var baristaItem = new BaristaItem
        {
            ItemType = orderIn.ItemType,
            TimeIn = DateTime.UtcNow
        };

        await Task.Delay(CalculateDelay(orderIn.ItemType), cancellationToken);

        var orderUp = new OrderUp(orderIn.OrderId, orderIn.ItemLineId, Item.GetItem(orderIn.ItemType)?.ToString()!, orderIn.ItemType, DateTime.UtcNow, "teesee");
        baristaItem.TimeUp = DateTime.UtcNow;

        await _baristaItemRepository.AddAsync(baristaItem);

        await _publisher.Publish(orderUp);
    }

    private static TimeSpan CalculateDelay(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.COFFEE_BLACK => TimeSpan.FromSeconds(5),
            ItemType.COFFEE_WITH_ROOM => TimeSpan.FromSeconds(5),
            ItemType.ESPRESSO => TimeSpan.FromSeconds(7),
            ItemType.ESPRESSO_DOUBLE => TimeSpan.FromSeconds(7),
            ItemType.CAPPUCCINO => TimeSpan.FromSeconds(10),
            _ => TimeSpan.FromSeconds(3)
        };
    }
}
