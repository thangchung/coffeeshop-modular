using MediatR;
using N8T.Core.Domain;

namespace CoffeeShop.Domain.ValueObjects;

public class OrderUpdate : ValueObject, INotification
{
    public Guid OrderId { get; set; }
    public Guid ItemLineId { get; set; }
    public ItemType ItemType { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string? MadeBy { get; set; }

    public OrderUpdate(Guid orderId, Guid itemLineId, ItemType itemType, OrderStatus orderStatus)
    {
        OrderId = orderId;
        ItemLineId = itemLineId;
        ItemType = itemType;
        OrderStatus = orderStatus;
        MadeBy = null;
    }

    public OrderUpdate(Guid orderId, Guid itemLineId, ItemType itemType, OrderStatus orderStatus, string madeBy)
    {
        OrderId = orderId;
        ItemLineId = itemLineId;
        ItemType = itemType;
        OrderStatus = orderStatus;
        MadeBy = madeBy;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        return new object[] { OrderId, ItemLineId, ItemType, OrderStatus };
    }
}
