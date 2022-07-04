using CoffeeShop.Domain;
using N8T.Core.Domain;

namespace CoffeeShop.Kitchen.Domain;

public class KitchenOrder : EntityRootBase
{
    public Guid OrderId { get; set; }
    public ItemType ItemType { get; set; }
    public DateTime TimeIn { get; set; }
    public DateTime TimeUp { get; set; }

    public KitchenOrder(Guid orderId, ItemType itemType, DateTime timeIn)
    {
        OrderId = orderId;
        ItemType = itemType;
        TimeIn = timeIn;
    }

    public KitchenOrder(Guid orderId, ItemType itemType, DateTime timeIn, DateTime timeUp)
    {
        OrderId = orderId;
        ItemType = itemType;
        TimeIn = timeIn;
        TimeUp = timeUp;
    }
}
