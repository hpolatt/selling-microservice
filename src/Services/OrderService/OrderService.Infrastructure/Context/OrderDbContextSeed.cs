using System;
using Polly;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Domain.SeedWork;

namespace OrderService.Infrastructure.Context;

public class OrderDbContextSeed
{
    public async Task SeedAsync(OrderDbContext context, ILogger<OrderDbContext> logger) {
        var policy = CreatePolicy(logger, nameof(OrderDbContextSeed)); 
        await policy.ExecuteAsync(async () => {
            var useCustomizationData = false; 
            var contentRootPath = "Seeding/Setups";

            using(context) {
                context.Database.Migrate();
                if (!context.CardTypes.Any()) {
                    context.CardTypes.AddRange(useCustomizationData ? GetCardTypesFromFile(contentRootPath, logger) : GetPreDefinedCardTypes());
                    await context.SaveChangesAsync();
                }

                if (!context.OrderStatuses.Any()) {
                    context.OrderStatuses.AddRange(useCustomizationData ? GetOrderStatusesFromFile(contentRootPath, logger) : GetPreDefinedOrderStatus());
                    await context.SaveChangesAsync();
                }

               
            }
        });
    }

    private IEnumerable<OrderStatus> GetOrderStatusesFromFile(string contentRootPath, ILogger<OrderDbContext> logger)
    {
        string fileName = "OrderStatus.txt";
        if (!File.Exists(fileName))
        {
            return GetPreDefinedOrderStatus();
        }

        var fileContent = File.ReadAllLines(fileName);

        int id = 1;
        var list = fileContent.Select(x => new OrderStatus(id++, x));

        return list;
        
    }

    private IEnumerable<OrderStatus> GetPreDefinedOrderStatus()
    {
        return new List<OrderStatus>
        {
            OrderStatus.Submitted,
            OrderStatus.AwaitingValidation,
            OrderStatus.StockConfirmed,
            OrderStatus.Paid,
            OrderStatus.Shipped,
            OrderStatus.Cancelled
        }; 
    }

    private IEnumerable<CardType> GetCardTypesFromFile(string contentRootPath, ILogger<OrderDbContext> logger)
    {
        string fileName = "CardTypes.txt";

        if (!File.Exists(fileName))
        {
            return GetPreDefinedCardTypes();
        }

        var fileContent = File.ReadAllLines(fileName);

        int id = 1;
        var list = fileContent.Select(x => new CardType(id++, x));

        return list;
    }

    private IEnumerable<CardType> GetPreDefinedCardTypes()
    {
        return Enumeration.GetAll<CardType>();
    }

    private IAsyncPolicy CreatePolicy(ILogger<OrderDbContext> logger, string prefix)
                {
                    return Policy.Handle<Exception>()
                        .WaitAndRetryAsync(
                            retryCount: 3,
                            sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                            onRetry: (exception, timeSpan, retry, ctx) =>
                            {
                                logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, 3);
                            }
                        );
                }
}


