namespace Rabbita.InProc.FluentExtensions.Command;

using Core.Command;

internal sealed class CommandConsumerConfigBuilder<T>  :ICommandConsumerConfigBuilder<T> where T : ICommandHandler
{
    private readonly CommandConsumerConfig _config;
    public CommandConsumerConfigBuilder(CommandConsumerConfig config)
    {
        _config ??= config ?? throw new NullReferenceException(nameof(config));
    }

    public ICommandConsumerConfigBuilder<T> AddExceptionHandler<TException, THandler>() where TException : Exception where THandler : IExceptionHandler<TException>
    {
        var ex = typeof(TException).FullName;
        if (_config.ExceptionHandlers.TryGetValue(ex, out var type))
            throw new ArgumentException($"Handler for {ex} already registred");
        
        _config.ExceptionHandlers.Add(ex, typeof(THandler));
        
        return this;
    }
}