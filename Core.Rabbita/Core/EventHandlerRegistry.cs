using System;
using System.Collections.Generic;
using System.Linq;
using Core.Rabbita.FluentExtensions;
using Core.Rabbita.Helpers;

namespace Core.Rabbita.Core
{
    public sealed class EventHandlerRegistry : IEventHandlerRegistry
    {
        private readonly Dictionary<Type, Type> _handlers = new Dictionary<Type, Type>();
        public IEnumerable<Type> RegisteredHandlers => _handlers.Values;

        public IEventHandlerRegistry Register<T>() where T : IEventHandler
        {
            var supportedQueryTypes = typeof(T).GetGenericInterfaces(typeof(IEventHandler<>));

            if (supportedQueryTypes.Count == 0)
                throw new ArgumentException("The handler must implement the IEventHandler<> interface.");
            if (_handlers.Keys.Any(registeredType => supportedQueryTypes.Contains(registeredType)))
                throw new ArgumentException("The event handled by the received handler already has a registered handler.");

            foreach (var key in supportedQueryTypes)
                _handlers.TryAdd(key, typeof(T));

            return this;
        }

        public Type GetHandlerFor(IEvent @event)
        {
            if (@event == null)
                throw new ArgumentException("The event can't be null");
            if (!_handlers.TryGetValue(@event.GetType(), out var type))
                throw new KeyNotFoundException("Not found Hanlder");
            return type;
        }
    }
}