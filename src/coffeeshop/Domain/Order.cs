using CoffeeShop.Domain.Commands;
using CoffeeShop.Domain.DomainEvents;
using N8T.Core.Domain;

namespace CoffeeShop.Domain;

public class Order : EntityRootBase
{
    public OrderSource OrderSource { get; }
    public Guid LoyaltyMemberId { get; }
    public OrderStatus OrderStatus { get; private set; }
    public Location Location { get; }
    public List<LineItem> LineItems { get; } = new();

    private Order() 
    {
        // for MediatR binding    
    }

    private Order(OrderSource orderSource, Guid loyaltyMemberId, OrderStatus orderStatus, Location location)
    {
        OrderSource = orderSource;
        LoyaltyMemberId = loyaltyMemberId;
        OrderStatus = orderStatus;
        Location = location;
    }

    public static Order From(PlaceOrderCommand placeOrderCommand)
    {
        var order = new Order(placeOrderCommand.OrderSource, placeOrderCommand.LoyaltyMemberId, OrderStatus.IN_PROGRESS, placeOrderCommand.Location);

        if (placeOrderCommand.BaristaItems.Any())
        {
            foreach (var baritaItem in placeOrderCommand.BaristaItems)
            {
                var item = Item.GetItem(baritaItem.ItemType);
                var lineItem = new LineItem(baritaItem.ItemType, item.Type.ToString(), item.Price, ItemStatus.IN_PROGRESS, true);

                order.AddDomainEvent(new BaristaOrderIn(order.Id, lineItem.Id, lineItem.ItemType));
                order.AddDomainEvent(new OrderUpdate(order.Id, lineItem.Id, lineItem.ItemType, OrderStatus.IN_PROGRESS));

                order.LineItems.Add(lineItem);
            }
        }

        if (placeOrderCommand.KitchenItems.Any())
        {
            foreach (var kitchenItem in placeOrderCommand.KitchenItems)
            {
                var item = Item.GetItem(kitchenItem.ItemType);
                var lineItem = new LineItem(kitchenItem.ItemType, item.Type.ToString(), item.Price, ItemStatus.IN_PROGRESS, false);

                order.AddDomainEvent(new KitchenOrderIn(order.Id, lineItem.Id, lineItem.ItemType));
                order.AddDomainEvent(new OrderUpdate(order.Id, lineItem.Id, lineItem.ItemType, OrderStatus.IN_PROGRESS));

                order.LineItems.Add(lineItem);
            }
        }

        return order;
    }

    public Order Apply(OrderUp orderUp)
    {
        AddDomainEvent(new OrderUpdate(Id, orderUp.ItemLineId, orderUp.ItemType, OrderStatus.FULFILLED, orderUp.MadeBy));

        if (LineItems.Any())
        {
            var item = LineItems.FirstOrDefault(i => i.Id == orderUp.ItemLineId);
            if (item is not null)
            {
                item.ItemStatus = ItemStatus.FULFILLED;
                AddDomainEvent(new OrderUpdate(Id, item.Id, item.ItemType, OrderStatus.FULFILLED, orderUp.MadeBy));
            }
        }

        // if there are both barista and kitchen items concatenate them before checking status
        if (LineItems.Any())
        {
            if (LineItems.All(i => i.ItemStatus == ItemStatus.FULFILLED))
            {
                OrderStatus = OrderStatus.FULFILLED;
            }
        }

        return this;
    }
}
