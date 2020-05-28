using System;

namespace Rabbita.Core.FluentExtensions
{
    public interface IEventHandlerRegistry
    {
        IEventHandlerRegistry Register<T>() where T : IEventHandler;
        Type GetHandlerFor(IEvent @event);
    }
}
