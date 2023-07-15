namespace Rabbita.Mq.FluentExtensions.Event;

using System;

using Abstraction;

using Core;
using Core.MessageSerializer;

using Rabbita.Core.Event;

internal sealed class EventConsumerConfigBuilder<T> : IEventConsumerConfigBuilder<T> where T : IEventHandler
{
    private readonly EventConsumerConfig _config;

    public EventConsumerConfigBuilder(EventConsumerConfig config)
    {
        _config ??= config ?? throw new NullReferenceException(nameof(config));
    }

    public IEventConsumerConfigBuilder<T> AddExceptionHandler<TException, THandler>()
        where TException : Exception where THandler : IExceptionHandler<TException>
    {
        var ex = typeof(TException).FullName;
        if (_config.ExceptionHandlers.TryGetValue(ex, out var type))
            throw new ArgumentException($"Handler for {ex} already registred");

        _config.ExceptionHandlers.Add(ex, typeof(THandler));

        return this;
    }
    
    
    public IEventConsumerConfigBuilder<T> AddSerializer(IMessageSerializer messageSerializer)
    {
        throw new NotImplementedException();
    }

    public IEventConsumerConfigBuilder<T> SetConsumerPrefetchCount(Int32 count)
    {
        throw new NotImplementedException();
    }

    public IEventConsumerConfigBuilder<T> FromInstance(String instanceName, Action<IInstanceHandlerConfigBuilder<T>> instanceHandlerOptions)
    {
        throw new NotImplementedException();
    }
}