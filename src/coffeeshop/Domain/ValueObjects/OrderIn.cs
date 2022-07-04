using MediatR;
using N8T.Core.Domain;

namespace CoffeeShop.Domain.ValueObjects;

public class OrderIn : ValueObject, INotification
{
    public Guid OrderId { get; set; }
    public Guid ItemLineId { get; set; }
    public ItemType ItemType { get; set; }
    public DateTime TimeIn { get; set; }

    public OrderIn(Guid orderId, Guid itemLineId, ItemType itemType)
    {
        OrderId = orderId;
        ItemLineId = itemLineId;
        ItemType = itemType;
        TimeIn = DateTime.UtcNow;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        return new object[] { OrderId, ItemLineId, ItemType, TimeIn };
    }
}

public class BaristaOrderIn : OrderIn
{
    public BaristaOrderIn(Guid orderId, Guid itemLineId, ItemType itemType) 
        : base(orderId, itemLineId, itemType)
    {
    }
}

public class KitchenOrderIn : OrderIn
{
    public KitchenOrderIn(Guid orderId, Guid itemLineId, ItemType itemType)
        : base(orderId, itemLineId, itemType)
    {
    }
}
