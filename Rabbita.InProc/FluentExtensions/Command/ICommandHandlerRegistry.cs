namespace Rabbita.InProc.FluentExtensions.Command;

using Core.Command;

public interface ICommandHandlerRegistry:IBaseCommandHandlerRegistry
{
    ICommandConsumerConfigBuilder<T> Register<T>() where T : ICommandHandler;
}

public interface ICommandConsumerConfigBuilder<T> where T : ICommandHandler
{
    ICommandConsumerConfigBuilder<T> AddExceptionHandler<TException, THandler>()
        where TException : Exception
        where THandler : IExceptionHandler<TException>;
}