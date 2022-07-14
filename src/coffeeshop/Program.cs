// dotnet tool update --global dotnet-ef --version 7.0.0-preview.6.22329.4
// cd src/coffeeshop/ && dotnet ef migrations add InitCoffeeShopDb -c MainDbContext -o Infrastructure/Data/Migrations

using CoffeeShop.Counter.UseCases;
using CoffeeShop.Domain;
using CoffeeShop.Domain.Commands;
using CoffeeShop.Infrastructure.Data;
using CoffeeShop.Infrastructure.Hubs;
using MediatR;
using N8T.Infrastructure;
using N8T.Infrastructure.Controller;
using N8T.Infrastructure.EfCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddCustomCors()
    .AddHttpContextAccessor()
    .AddCustomMediatR(new[] { typeof(Order) })
    .AddCustomValidators(new[] { typeof(Order) });

builder.Services.AddPostgresDbContext<MainDbContext>(
                builder.Configuration.GetConnectionString("coffeeshopdb"),
                null,
                svc => svc.AddRepository(typeof(Repository<>)))
            .AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDaprClient();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCustomCors();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.MapGet("/error", () => Results.Problem("An error occurred.", statusCode: 500))
    .ExcludeFromDescription();

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

app.MapGroup("/v1/api").MapOrderApi();

app.MapHub<NotificationHub>("/message");

app.MapFallback(() => Results.Redirect("/swagger"));

await app.DoDbMigrationAsync(app.Logger);

app.Run();

public static class OrderApi
{
    public static RouteGroupBuilder MapOrderApi(this RouteGroupBuilder group)
    {
        group.MapPost("/orders",
            async (PlaceOrderCommand command, ISender sender) => await sender.Send(command));

        group.MapGet("/fulfillment-orders",
            async (ISender sender) => await sender.Send(new OrderFulfillmentQuery()));

        return group;
    }
}