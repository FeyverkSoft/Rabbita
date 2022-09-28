using System;
using System.Diagnostics.CodeAnalysis;

using Rabbita.Core;
using Rabbita.Core.Event;
using Rabbita.Core.MessageSerializer;
using Rabbita.Mq.Abstraction;

namespace Rabbita.Mq.FluentExtensions;

public interface IEventHandlerRegistry
{
    IConsumerConfigBuilder<T> Register<T>() where T : IEventHandler;
    Type GetHandlerFor(IEvent @event);
}

public interface IConsumerConfigBuilder<T> where T : IEventHandler
{
    IConsumerConfigBuilder<T> AddSerializer([NotNull] IMessageSerializer messageSerializer);
    IConsumerConfigBuilder<T> SetConsumerCount(Int32 count);
    IConsumerConfigBuilder<T> FromInstance([NotNull] String instanceName, Action<IInstanceHandlerConfigBuilder<T>> instanceHandlerOptions);

    IConsumerConfigBuilder<T> FromDefaultInstance(Action<IInstanceHandlerConfigBuilder<T>> instanceHandlerOptions)
        => FromInstance(RabbitaConst.DefaultInstanceName, instanceHandlerOptions);

    IConsumerConfigBuilder<T> AddExceptionHandler<T1>() where T1 : IExceptionHandler<Exception>;
}