using CoffeeShop.Domain;
using CoffeeShop.Domain.ValueObjects;

namespace CoffeeShop.Counter.Domain;

public class OrderEventResult
{
    public Order Order { get; set; }
    public List<OrderIn> BaristaTickets { get; set; } = new();
    public List<OrderIn> KitchenTickets { get; set; } = new();
    public List<OrderUpdate> OrderUpdates { get; set; } = new();
}
