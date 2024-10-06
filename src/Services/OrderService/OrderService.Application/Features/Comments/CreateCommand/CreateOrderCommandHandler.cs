using System;
using EventBus.Base.Abstraction;
using MediatR;
using OrderService.Application.IntegrationEvents;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.OrderAggregate;

namespace OrderService.Application.Features.Comments.CreateCommand;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository orderRepository;
    private readonly IEventBus eventBus;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus)
    {
        this.orderRepository = orderRepository;
        this.eventBus = eventBus;
    }
    public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var addr = new Address(request.City, request.Street, request.State, request.Country, request.ZipCode);
        Order dbOrder = new(request.UserName, addr, request.CardTypeId, request.CardNumber, request.CardSecurityNumber, request.CarHolderName, request.CardExpiration);
        request.OrderItems.ToList().ForEach(x => dbOrder.AddOrderItem(x.ProductId, x.ProductName, x.UnitPrice, x.PictureUrl, x.Units));

        await orderRepository.AddAsync(dbOrder);
        await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        
        var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(request.UserName);
        eventBus.Publish(orderStartedIntegrationEvent);
        return true;
    }
}