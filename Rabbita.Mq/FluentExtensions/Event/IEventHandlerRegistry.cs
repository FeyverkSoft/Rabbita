namespace Rabbita.Mq.FluentExtensions.Event
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Core;

    using Rabbita.Core.Event;

    using Core.MessageSerializer;

    using Abstraction;

    public interface IEventHandlerRegistry : IBaseEventHandlerRegistry
    {
        IEventConsumerConfigBuilder<T> Register<T>() where T : IEventHandler;
    }

    public interface IEventConsumerConfigBuilder<T> where T : IEventHandler
    {
        IEventConsumerConfigBuilder<T> AddSerializer([NotNull] IMessageSerializer messageSerializer);
        IEventConsumerConfigBuilder<T> SetConsumerPrefetchCount(Int32 count);
        IEventConsumerConfigBuilder<T> FromInstance([NotNull] String instanceName, Action<IInstanceHandlerConfigBuilder<T>> instanceHandlerOptions);

        IEventConsumerConfigBuilder<T> FromDefaultInstance(Action<IInstanceHandlerConfigBuilder<T>> instanceHandlerOptions)
            => FromInstance(RabbitaConst.DefaultInstanceName, instanceHandlerOptions);

        IEventConsumerConfigBuilder<T> AddExceptionHandler<TException, THandler>()
            where TException : Exception
            where THandler : IExceptionHandler<TException>;
    }
}