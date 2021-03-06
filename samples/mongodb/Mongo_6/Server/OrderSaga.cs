﻿using System;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

#region thesaga

public class OrderSaga : Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleTimeouts<CompleteOrder>
{
    IBus bus;
    static ILog logger = LogManager.GetLogger<OrderSaga>();

    public OrderSaga(IBus bus)
    {
        this.bus = bus;
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
            .ToSaga(sagaData => sagaData.OrderId);
    }

    public void Handle(StartOrder message)
    {
        Data.OrderId = message.OrderId;
        var orderDescription = $"The saga for order {message.OrderId}";
        Data.OrderDescription = orderDescription;
        logger.Info($"Received StartOrder message {Data.OrderId}. Starting Saga");
        logger.Info("Order will complete in 5 seconds");
        var timeoutData = new CompleteOrder
        {
            OrderDescription = orderDescription
        };
        RequestTimeout(TimeSpan.FromSeconds(5), timeoutData);
    }

    public void Timeout(CompleteOrder state)
    {
        logger.Info($"Saga with OrderId {Data.OrderId} completed");
        var orderCompleted = new OrderCompleted
        {
            OrderId = Data.OrderId
        };
        bus.Publish(orderCompleted);
        MarkAsComplete();
    }

}

#endregion