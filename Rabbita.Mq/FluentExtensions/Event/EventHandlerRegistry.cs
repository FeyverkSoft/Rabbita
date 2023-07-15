namespace Rabbita.Mq.FluentExtensions.Event;

using System;
using System.Collections.Generic;
using System.Linq;

using Rabbita.Core.Event;
using Rabbita.Core.Helpers;

public sealed class EventHandlerRegistry : IEventHandlerRegistry
{
    private readonly Dictionary<Type, (Type Handler, IEventConsumerConfig Config)> _handlers = new();
    public IEnumerable<(Type Handler, IEventConsumerConfig Config)> RegisteredHandlers => _handlers.Values;

    public IEventConsumerConfigBuilder<T> Register<T>() where T : IEventHandler
    {
        var type = typeof(T);
        var supportedQueryTypes = type.GetGenericInterfaces(typeof(IEventHandler<>));

        if (supportedQueryTypes.Count == 0)
            throw new ArgumentException("The handler must implement the IEventHandler<> interface.");
        if (_handlers.Keys.Any(registeredType => supportedQueryTypes.Contains(registeredType)))
            throw new ArgumentException("The event handled by the received handler already has a registered handler.");


        var config = new EventConsumerConfig();
        
        foreach (var eventsType in supportedQueryTypes)
            _handlers.TryAdd(eventsType, (type, config));

        return new EventConsumerConfigBuilder<T>(config);
    }

    public (Type Handler, IEventConsumerConfig Config) GetHandlerFor(IEvent @event)
    {
        if (@event == null)
            throw new ArgumentException("The event can't be null");
        if (!_handlers.TryGetValue(@event.GetType(), out var type))
            throw new KeyNotFoundException("Not found Hanlder");
        return type;
    }
}