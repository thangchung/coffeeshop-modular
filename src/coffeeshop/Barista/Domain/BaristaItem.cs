using CoffeeShop.Domain;
using N8T.Core.Domain;

namespace CoffeeShop.Barista.Domain;

public class BaristaItem : EntityRootBase
{
    public ItemType ItemType { get; set; }
    public DateTime TimeIn { get; set; }
    public DateTime TimeUp { get; set; }
}
