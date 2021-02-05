using System;
using System.Collections.Generic;
using System.Linq;

using Rabbita.Core.FluentExtensions;
using Rabbita.Core.Helpers;

namespace Rabbita.Core.HandlerRegistry
{
    public sealed class ExceptionHandlerRegistry : IExceptionHandlerRegistry
    {
        private readonly Dictionary<Type, Type> _handlers = new();
        public IEnumerable<Type> RegisteredHandlers => _handlers.Values;

        public IExceptionHandlerRegistry Register<T>() where T : IExceptionHandler
        {
            var supportedQueryTypes = typeof(T).GetGenericInterfaces(typeof(IExceptionHandler<>));

            if (supportedQueryTypes.Count == 0)
                throw new ArgumentException("The handler must implement the IExceptionHandler<> interface.");
            if (_handlers.Keys.Any(registeredType => supportedQueryTypes.Contains(registeredType)))
                throw new ArgumentException("The exception handled by the received handler already has a registered handler.");

            foreach (var key in supportedQueryTypes)
                _handlers.TryAdd(key, typeof(T));

            return this;
        }

        public Type? GetHandlerFor<TE>(TE exception) where TE : notnull, Exception
        {
            if (exception == null)
                throw new ArgumentException("The exception can't be null");
            return !_handlers.TryGetValue(exception.GetType(), out var type) ? null : type;
        }
    }
}