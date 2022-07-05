using CoffeeShop.Domain;
using CoffeeShop.Domain.DomainEvents;
using N8T.Core.Domain;

namespace CoffeeShop.Kitchen.Domain;

public class KitchenOrder : EntityRootBase
{
    public Guid OrderId { get; }
    public ItemType ItemType { get; }
    public string ItemName { get; } = null!;
    public DateTime TimeIn { get; }
    public DateTime TimeUp { get; private set; }

    private KitchenOrder()
    {
        // for MediatR binding
    }

    private KitchenOrder(Guid orderId, ItemType itemType, DateTime timeIn)
    {
        OrderId = orderId;
        ItemType = itemType;
        ItemName = Item.GetItem(itemType)?.ToString()!; ;
        TimeIn = timeIn;
    }

    public KitchenOrder SetTimeUp(Guid itemLineId, DateTime timeUp)
    {
        AddDomainEvent(new OrderUp(OrderId, itemLineId, Item.GetItem(ItemType)?.ToString()!, ItemType, DateTime.UtcNow, "teesee"));
        TimeUp = timeUp;
        return this;
    }

    public static KitchenOrder From(Guid orderId, ItemType itemType, DateTime timeIn)
    {
        return new KitchenOrder(orderId, itemType, timeIn);
    }
}
