using MediatR;
using N8T.Core.Domain;

namespace CoffeeShop.Domain.ValueObjects;

public class OrderIn : ValueObject, INotification
{
    public Guid OrderId { get; set; }
    public Guid ItemLineId { get; set; }
    public string Name { get; set; }
    public ItemType ItemType { get; set; }
    public DateTime TimeIn { get; set; }

    public OrderIn(Guid orderId, Guid itemLineId, string name, ItemType itemType)
    {
        OrderId = orderId;
        ItemLineId = itemLineId;
        Name = name;
        ItemType = itemType;
        TimeIn = DateTime.UtcNow;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        return new object[] { OrderId, ItemLineId, Name, ItemType, TimeIn };
    }
}
