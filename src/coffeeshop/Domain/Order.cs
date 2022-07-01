using CoffeeShop.Counter.Domain;
using CoffeeShop.Domain.Commands;
using CoffeeShop.Domain.ValueObjects;
using N8T.Core.Domain;

namespace CoffeeShop.Domain;

public class Order : EntityRootBase
{
    public OrderSource OrderSource { get; set; }
    public Guid LoyaltyMemberId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public Location Location { get; set; }
    public List<LineItem> LineItems { get; set; } = new();
    //public List<LineItem> KitchenLineItems { get; set; } = new();

    public static OrderEventResult From(PlaceOrderCommand placeOrderCommand)
    {
        var order = new Order
        {
            OrderSource = placeOrderCommand.OrderSource,
            Location = placeOrderCommand.Location,
            OrderStatus = OrderStatus.IN_PROGRESS
        };

        var orderEventResult = new OrderEventResult();
        if (placeOrderCommand.BaristaItems.Any())
        {
            foreach (var baritaItem in placeOrderCommand.BaristaItems)
            {
                var lineItem = new LineItem(baritaItem.ItemType, baritaItem.Name, baritaItem.Price, ItemStatus.IN_PROGRESS, order, true);
                orderEventResult.BaristaTickets.Add(new OrderIn(order.Id, lineItem.Id, lineItem.Name, lineItem.ItemType));
                orderEventResult.OrderUpdates.Add(new OrderUpdate(order.Id, lineItem.Id, lineItem.Name, lineItem.ItemType, OrderStatus.IN_PROGRESS));
                order.LineItems.Add(lineItem);
            }
        }
        
        if (placeOrderCommand.KitchenItems.Any())
        {
            foreach(var kitchenItem in placeOrderCommand.KitchenItems)
            {
                var lineItem = new LineItem(kitchenItem.ItemType, kitchenItem.Name, kitchenItem.Price, ItemStatus.IN_PROGRESS, order, false);
                orderEventResult.KitchenTickets.Add(new OrderIn(order.Id, lineItem.Id, lineItem.Name, lineItem.ItemType));
                orderEventResult.OrderUpdates.Add(new OrderUpdate(order.Id, lineItem.Id, lineItem.Name, lineItem.ItemType, OrderStatus.IN_PROGRESS));
                order.LineItems.Add(lineItem);
            }
        }

        orderEventResult.Order = order;
        return orderEventResult;
    }

    public OrderEventResult Apply(OrderUp orderUp)
    {
        var orderEventResult = new OrderEventResult();
        orderEventResult.OrderUpdates.Add(new OrderUpdate(Id, orderUp.ItemLineId, orderUp.Name, orderUp.ItemType, OrderStatus.FULFILLED, orderUp.MadeBy));
        
        if (LineItems.Any())
        {
            var item = LineItems.FirstOrDefault(i => i.Id == orderUp.ItemLineId);
            if(item is not null)
            {
                item.ItemStatus = ItemStatus.FULFILLED;
                orderEventResult.OrderUpdates.Add(new OrderUpdate(Id, item.Id, item.Name, item.ItemType, OrderStatus.FULFILLED, orderUp.MadeBy));
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

        orderEventResult.Order = this;
        return orderEventResult;
    }
}
