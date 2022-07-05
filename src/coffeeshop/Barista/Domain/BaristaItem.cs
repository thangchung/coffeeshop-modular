using CoffeeShop.Domain;
using CoffeeShop.Domain.DomainEvents;
using N8T.Core.Domain;

namespace CoffeeShop.Barista.Domain;

public class BaristaItem : EntityRootBase
{
    public ItemType ItemType { get; }
    public string ItemName { get; } = null!;
    public DateTime TimeIn { get; }
    public DateTime TimeUp { get; private set; }

    private BaristaItem()
    {
        // for MediatR binding
    }

    private BaristaItem(ItemType itemType, DateTime timeIn)
    {
        ItemType = itemType;
        ItemName = Item.GetItem(itemType)?.ToString()!;
        TimeIn = timeIn;
    }

    public static BaristaItem From(ItemType itemType, DateTime timeIn)
    {
        return new BaristaItem(itemType, timeIn);
    }

    public BaristaItem SetTimeUp(Guid orderId, Guid itemLineId, DateTime timeUp)
    {
        AddDomainEvent(new OrderUp(orderId, itemLineId, Item.GetItem(ItemType)?.ToString()!, ItemType, DateTime.UtcNow, "teesee"));
        TimeUp = timeUp;
        return this;
    }
}
