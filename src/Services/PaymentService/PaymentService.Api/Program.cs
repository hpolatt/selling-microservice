using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using PaymentService.Api.IntegrationEvents.EventHandlers;
using PaymentService.Api.IntegrationEvents.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConfiguration>(provider => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
builder.Services.AddLogging(configure => configure.AddConsole());
builder.Services.AddTransient<OrderStartedIntegrationEventHandler>();
builder.Services.AddSingleton(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "PaymentService",
        EventBusType = EventBusType.RabbitMQ
    };
    return EventBusFactory.Create(config, sp);
});

var app = builder.Build();

// listen to OrderStartedIntergationEvent from the event bus
app.Services.GetRequiredService<IEventBus>().Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
