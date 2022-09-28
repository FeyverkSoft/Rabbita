namespace Rabbita.InProc.FluentExtensions.Event;

using Rabbita.Core.Event;

public interface IEventHandlerRegistry
{
    IEventConsumerConfigBuilder<T> Register<T>() where T : IEventHandler;
    (Type Handler, IEventConsumerConfig Config) GetHandlerFor(IEvent @event);
}

public interface IEventConsumerConfigBuilder<T> where T : IEventHandler
{
    IEventConsumerConfigBuilder<T> AddExceptionHandler<TException, THandler>() 
        where TException : Exception
        where THandler : IExceptionHandler<TException>;
}