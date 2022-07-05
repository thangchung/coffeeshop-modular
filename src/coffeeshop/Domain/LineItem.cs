using N8T.Core.Domain;

namespace CoffeeShop.Domain;

public class LineItem : EntityBase
{
    public Order Order { get; set; }
    public Guid OrderId { get; set; }
    public ItemType ItemType { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public ItemStatus ItemStatus { get; set; }
    public bool IsBaristaOrder { get; set; }
    
    public LineItem()
    {
    }

    public LineItem(ItemType itemType, string name, decimal price, ItemStatus itemStatus, Order order, bool isBarista)
    {
        ItemType = itemType;
        Name = name;
        Price = price;
        ItemStatus = itemStatus;

        Order = order;
        OrderId = order.Id;
        IsBaristaOrder = isBarista;
    }
}
