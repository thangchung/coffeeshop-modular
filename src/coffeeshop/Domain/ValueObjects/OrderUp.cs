namespace CoffeeShop.Domain.ValueObjects;

public class OrderUp : OrderIn
{
    public string MadeBy { get; set; }
    public DateTime TimeUp { get; set; }

    public OrderUp(Guid orderId, Guid itemLineId, string name, ItemType itemType, DateTime timeUp, string madeBy)
        : base(orderId, itemLineId, name, itemType)
    {
        MadeBy = madeBy;
        TimeUp = timeUp;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        return new object[] { MadeBy, TimeUp }.Concat(base.GetEqualityComponents());
    }
}
