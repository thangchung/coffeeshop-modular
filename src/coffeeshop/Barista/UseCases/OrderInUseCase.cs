using CoffeeShop.Barista.Domain;
using CoffeeShop.Domain;
using CoffeeShop.Domain.DomainEvents;
using MediatR;
using N8T.Core.Domain;
using N8T.Core.Repository;

namespace CoffeeShop.Barista.UseCases;

public class OrderInUseCase : N8T.Infrastructure.Events.DomainEventHandler<BaristaOrderIn>
{
    private readonly IRepository<BaristaItem> _baristaItemRepository;
    private readonly IPublisher _publisher;

    public OrderInUseCase(IRepository<BaristaItem> baristaItemRepository, IPublisher publisher)
    {
        _baristaItemRepository = baristaItemRepository;
        _publisher = publisher;
    }

    public override async Task HandleEvent(BaristaOrderIn @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);

        var baristaItem = BaristaItem.From(@event.ItemType, DateTime.UtcNow);

        await Task.Delay(CalculateDelay(@event.ItemType), cancellationToken);

        baristaItem.SetTimeUp(@event.OrderId, @event.ItemLineId, DateTime.UtcNow);

        await _baristaItemRepository.AddAsync(baristaItem, cancellationToken: cancellationToken);

        await baristaItem.RelayAndPublishEvents(_publisher, cancellationToken);
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
