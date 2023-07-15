namespace Rabbita.Core.Event;

public interface IBaseEventHandlerRegistry
{
    (Type Handler, IEventConsumerConfig Config) GetHandlerFor(IEvent @event);
}