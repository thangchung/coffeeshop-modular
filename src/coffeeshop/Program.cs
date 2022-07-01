// dotnet tool update --global dotnet-ef --version 7.0.0-preview.5.22302.2
// dotnet ef migrations add InitCoffeeShopDb -c MainDbContext -o Infrastructure/Data/Migrations

using CoffeeShop;
using CoffeeShop.Domain;
using CoffeeShop.Infrastructure.Data;
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

app.MapControllers();

app.MapHub<NotificationHub>("/message");

app.MapFallback(() => Results.Redirect("/swagger"));

await app.DoDbMigrationAsync(app.Logger);

app.Run();
