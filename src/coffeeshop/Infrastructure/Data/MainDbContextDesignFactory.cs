﻿using N8T.Infrastructure.EfCore;

namespace CoffeeShop.Infrastructure.Data;

public class MainDbContextDesignFactory : DbContextDesignFactoryBase<MainDbContext>
{
    public MainDbContextDesignFactory() : base("coffeeshopdb")
    {
    }
}
