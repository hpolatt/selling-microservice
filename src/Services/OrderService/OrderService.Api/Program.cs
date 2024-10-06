using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using OrderService.Api.Extensions;
using OrderService.Api.IntegrationEvents.EventHandlers;
using OrderService.Api.IntegrationEvents.Events;
using OrderService.Application;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




// Configuration 
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddSingleton<IConfiguration>(configuration);

// Registerions
builder.Services.AddLogging(configure => configure.AddConsole());
builder.Services.AddApllicationRegisteration(); 
builder.Services.AddPersistenceRegistration(configuration);
builder.Services.ConfigureConsul(configuration);

builder.Services.AddSingleton(sp => {
    EventBusConfig eventBusConfig = new EventBusConfig(){
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "OrderService",
        EventBusType = EventBusType.RabbitMQ,
    };

    return EventBusFactory.Create(eventBusConfig, sp);
});


// register MigrateDbContext from IWebhost
var webhost = builder.WebHost.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateOnBuild = false;
}).Build();

webhost.MigrateDbContext<OrderDbContext>((context, services) =>
{
    var logger = services.GetRequiredService<ILogger<OrderDbContext>>();
    new OrderDbContextSeed().SeedAsync(context, logger).Wait();
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Services.GetRequiredService<IEventBus>().Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
