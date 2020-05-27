using System;
using System.Collections.Generic;
using System.Linq;
using Core.Rabbita.FluentExtensions;
using Core.Rabbita.Helpers;

namespace Core.Rabbita.Core
{
    public sealed class CommandHandlerRegistry : ICommandHandlerRegistry
    {
        private readonly Dictionary<Type, Type> _handlers = new Dictionary<Type, Type>();
        public IEnumerable<Type> RegisteredHandlers => _handlers.Values;

        public ICommandHandlerRegistry Register<T>() where T : ICommandHandler
        {
            var supportedQueryTypes = typeof(T).GetGenericInterfaces(typeof(IEventHandler<>));

            if (supportedQueryTypes.Count == 0)
                throw new ArgumentException("The handler must implement the ICommandHandler<> interface.");
            if (_handlers.Keys.Any(registeredType => supportedQueryTypes.Contains(registeredType)))
                throw new ArgumentException("The command handled by the received handler already has a registered handler.");

            foreach (var key in supportedQueryTypes)
                _handlers.TryAdd(key, typeof(T));

            return this;
        }

        public Type GetHandlerFor(ICommand @command)
        {
            if (@command == null)
                throw new ArgumentException("The command can't be null");
            if (!_handlers.TryGetValue(@command.GetType(), out var type))
                throw new KeyNotFoundException("Not found Hanlder");
            return type;
        }
    }
}