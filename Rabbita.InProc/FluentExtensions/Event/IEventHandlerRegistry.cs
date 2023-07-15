namespace Rabbita.InProc.FluentExtensions.Event;

using Rabbita.Core.Event;

public interface IEventHandlerRegistry : IBaseEventHandlerRegistry
{
    IEventConsumerConfigBuilder<T> Register<T>() where T : IEventHandler;
}

public interface IEventConsumerConfigBuilder<T> where T : IEventHandler
{
    IEventConsumerConfigBuilder<T> AddExceptionHandler<TException, THandler>()
        where TException : Exception
        where THandler : IExceptionHandler<TException>;
}