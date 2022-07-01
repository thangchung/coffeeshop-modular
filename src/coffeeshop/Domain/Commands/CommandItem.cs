namespace CoffeeShop.Domain.Commands;

// Representst the individual line items in a PlaceOrderCommand
public class CommandItem
{
    public ItemType ItemType { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
