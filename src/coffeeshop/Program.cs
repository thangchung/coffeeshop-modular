// dotnet tool update --global dotnet-ef --version 7.0.0-preview.6.22329.4
// cd src/coffeeshop/ && dotnet ef migrations add InitCoffeeShopDb -c MainDbContext -o Infrastructure/Data/Migrations

using CoffeeShop.Counter.Features;
using CoffeeShop.Domain;
using CoffeeShop.Infrastructure.Data;
using CoffeeShop.Infrastructure.Hubs;
using N8T.Infrastructure;
using N8T.Infrastructure.Controller;
using N8T.Infrastructure.EfCore;
using Spectre.Console;

AnsiConsole.Write(new FigletText("CoffeeShop APIs").Color(Color.MediumPurple));

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
    {
        options.AddPolicy(
            name: "api",
            builder =>
            {
                builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials();
            });
    })
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

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.MapGet("/error", () => Results.Problem("An error occurred.", statusCode: 500))
    .ExcludeFromDescription();

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();

app.UseCors("api");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

app.MapOrderInApiRoutes();
app.MapOrderFulfillmentApiRoutes();

app.MapHub<NotificationHub>("/message");

app.MapFallback(() => Results.Redirect("/swagger"));

await app.DoDbMigrationAsync(app.Logger);

app.Run();