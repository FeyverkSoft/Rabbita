namespace Rabbita.InProc.FluentExtensions.Event;

using Rabbita.Core.Event;

internal sealed class EventConsumerConfigBuilder<T>  :IEventConsumerConfigBuilder<T> where T : IEventHandler
{
    private readonly EventConsumerConfig _config;
    public EventConsumerConfigBuilder(EventConsumerConfig config)
    {
        _config ??= config ?? throw new NullReferenceException(nameof(config));
    }

    public IEventConsumerConfigBuilder<T> AddExceptionHandler<TException, THandler>() where TException : Exception where THandler : IExceptionHandler<TException>
    {
        var ex = typeof(TException).FullName;
        if (_config.ExceptionHandlers.TryGetValue(ex, out var type))
            throw new ArgumentException($"Handler for {ex} already registred");
        
        _config.ExceptionHandlers.Add(ex, typeof(THandler));
        
        return this;
    }
}