using System;
using EventBus.Base.Events;

namespace PaymentService.Api.IntegrationEvents.Events;

public class PaymentFailedIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }

    public string ErrorMessage { get; }

    public PaymentFailedIntegrationEvent(int orderId, string errorMessage)
    {
        OrderId = orderId;
        ErrorMessage = errorMessage;
    }

}
