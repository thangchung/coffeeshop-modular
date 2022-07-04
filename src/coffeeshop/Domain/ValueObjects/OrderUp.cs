using MediatR;
using N8T.Core.Domain;

namespace CoffeeShop.Domain.ValueObjects;

public class OrderUp : ValueObject, INotification
{
    // OrderIn info
    public Guid OrderId { get; set; }
    public Guid ItemLineId { get; set; }
    public string Name { get; set; }
    public ItemType ItemType { get; set; }
    public DateTime TimeIn { get; set; }
    
    public string MadeBy { get; set; }
    public DateTime TimeUp { get; set; }

    public OrderUp(Guid orderId, Guid itemLineId, string name, ItemType itemType, DateTime timeUp, string madeBy)
    {
        OrderId = orderId;
        ItemLineId = itemLineId;
        Name = name;
        ItemType = itemType;
        TimeIn = DateTime.UtcNow;
        MadeBy = madeBy;
        TimeUp = timeUp;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        return new object[] { OrderId, ItemLineId, Name, ItemType, TimeIn, MadeBy, TimeUp };
    }
}

public class BaristaOrderUp : OrderUp
{
    public BaristaOrderUp(Guid orderId, Guid itemLineId, string name, ItemType itemType, DateTime timeUp, string madeBy) 
        : base(orderId, itemLineId, name, itemType, timeUp, madeBy)
    {
    }
}

public class KitchenOrderUp : OrderUp
{
    public KitchenOrderUp(Guid orderId, Guid itemLineId, string name, ItemType itemType, DateTime timeUp, string madeBy)
        : base(orderId, itemLineId, name, itemType, timeUp, madeBy)
    {
    }
}
