namespace Rabbita.InProc.FluentExtensions.Command;

using Core.Command;

public interface ICommandHandlerRegistry
{
    ICommandConsumerConfigBuilder<T> Register<T>() where T : ICommandHandler;
    (Type Handler, ICommandConsumerConfig Config) GetHandlerFor(ICommand @command);
}

public interface ICommandConsumerConfigBuilder<T> where T : ICommandHandler
{
    ICommandConsumerConfigBuilder<T> AddExceptionHandler<TException, THandler>()
        where TException : Exception
        where THandler : IExceptionHandler<TException>;
}