using System;

namespace Core.Rabbita.FluentExtensions
{
    public interface IEventHandlerRegistry
    {
        IEventHandlerRegistry Register<T>() where T : IEventHandler;
        Type GetHandlerFor(IEvent @event);
    }
}
