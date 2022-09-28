namespace Rabbita.InProc;

using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Core.Command;
using Core.Infrastructure;
using Core.Message;

using FluentExtensions.Command;

internal sealed class InternalCommandProcessor : BackgroundService
{
    private readonly AsyncConcurrentQueue<ICommand> _queue;
    private readonly ICommandHandlerRegistry _dispatchers;
    private readonly IServiceProvider _provider;
    private readonly ILogger _logger;

    public InternalCommandProcessor(
        AsyncConcurrentQueue<ICommand> queue,
        IServiceProvider provider,
        ICommandHandlerRegistry dispatchers,
        ILogger<InternalCommandProcessor> logger)
    {
        _queue = queue;
        _dispatchers = dispatchers;
        _provider = provider;
        _logger = logger;
    }

    private async void ListenCommand(CancellationToken cancellationToken)
    {
        await Task.Yield();
        // пока что так, а не для каждой обработки свой скоуп, так  будет производительнее. Но есть и недостатки.
        using var scope = _provider.CreateScope();
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (_queue.Count > 0)
                    {
                        var message = await _queue.DequeueAsync(cancellationToken);
                        var (processorType, config) = _dispatchers.GetHandlerFor(message);
                        var processor = scope.ServiceProvider.GetService(processorType);
                        var method = processor.GetType().GetMethod(nameof(ICommandHandler<ICommand>.HandleAsync));
                        /* Это будет работать только для общего Scope на все вызовы
                         Однако если захочу сделать using var scope = _provider.CreateScope(); внутри цикла, то в пролёте
                        if(!_methodInfoDict.TryGetValue(processor, out var method))
                        {
                            method = processor.GetType().GetMethod(nameof(ICommandHandler<ICommand>.HandleAsync));
                            _methodInfoDict.Add(processor, method);
                        }
                        */
                        ThreadPool.QueueUserWorkItem(async state =>
                        {
                            try
                            {
                                await (Task)method.Invoke(processor, new Object[] { message, cancellationToken });
                            }
                            catch (Exception e)
                            {
                                await ProcessException(processorType, config, message, e, cancellationToken);
                            }
                        });
                    }
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e, e.Message);
                }

                if (_queue.Count == 0)
                {
                    await Task.Delay(100, cancellationToken);
                }
            }
        }
        catch (TaskCanceledException)
        {
            //nothing
        }
    }


    private async Task ProcessException(Type processorType, ICommandConsumerConfig config, IMessage message, Exception ex, CancellationToken cancellationToken)
    {
        var (handler, matchExceptionType) = config.GetExceptionHandlerFor(ex);
        if (handler is null || matchExceptionType is null)
        {
            _logger.LogError(ex, $"ExceptionHandler not found for exception {ex.GetType()}: {processorType.Name}: {ex.Message};");
            return;
        }

        using var scope = _provider.CreateScope();
        var processor = scope.ServiceProvider.GetService(handler);
        var method = processor?.GetType().GetMethods()
            .Where(m => m.Name.Equals(nameof(IExceptionHandler<Exception>.ExceptionHandleAsync)))
            .SingleOrDefault(m => m.GetParameters()[1].ParameterType.FullName.Equals(matchExceptionType));
        if (processor is null || method is null)
        {
            _logger.LogError(ex, $"ExceptionHandler not found for exception {ex.GetType()}: {processorType.Name}: {ex.Message};");
            return;
        }

        await (Task)method.Invoke(processor, new Object[] { message, ex, cancellationToken });
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ListenCommand(stoppingToken);
        return Task.CompletedTask;
    }
}