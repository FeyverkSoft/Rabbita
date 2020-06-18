using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rabbita.Core;
using Rabbita.Core.FluentExtensions;
using Rabbita.Core.Infrastructure;

namespace Rabbita.InProc
{
    internal sealed class InternalCommandProcessor : BackgroundService
    {
        private readonly AsyncConcurrentQueue<ICommand> _queue;
        private readonly ICommandHandlerRegistry _dispatchers;
        private readonly IExceptionHandlerRegistry? _exceptionHandlerRegistry;
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;

        public InternalCommandProcessor(
            AsyncConcurrentQueue<ICommand> queue,
            IServiceProvider provider,
            ICommandHandlerRegistry dispatchers,
            ILogger<InternalEventProcessor> logger,
            IExceptionHandlerRegistry? exceptionHandlerRegistry = null)
        {
            _queue = queue;
            _dispatchers = dispatchers;
            _provider = provider;
            _logger = logger;
            _exceptionHandlerRegistry = exceptionHandlerRegistry;
        }

        private async void ListenCommand(CancellationToken cancellationToken)
        {
            await Task.Yield();
            using var scope = _provider.CreateScope();
            try{
                while (!cancellationToken.IsCancellationRequested){
                    try{
                        if (_queue.Count > 0){
                            var message = await _queue.DequeueAsync(cancellationToken);
                            var processor = scope.ServiceProvider.GetService(_dispatchers.GetHandlerFor(message));
                            var method = processor.GetType().GetMethod(nameof(IEventHandler<IEvent>.Handle));
                            ThreadPool.QueueUserWorkItem(async state =>
                            {
                                try{
                                    await (Task) method.Invoke(processor, new Object[] {message, cancellationToken});
                                }
                                catch (Exception e){
                                    await ProcessException(processor?.ToString(), message, e, cancellationToken);
                                }
                            });
                        }
                    }
                    catch (InvalidOperationException e){
                        _logger.LogError(e, e.Message);
                    }

                    await Task.Delay(1, cancellationToken);
                }
            }
            catch (TaskCanceledException){ }
        }

        private async Task ProcessException(String? processorName, IMessage message, Exception ex, CancellationToken cancellationToken)
        {
            var handler = _exceptionHandlerRegistry?.GetHandlerFor(ex);
            if (handler == null){
                _logger.LogError(ex, $"ExceptionHandler not found for: {processorName}: {ex.Message};");
                return;
            }

            using var scope = _provider.CreateScope();
            var processor = scope.ServiceProvider.GetService(handler);
            var method = processor.GetType().GetMethod(nameof(IExceptionHandler<Exception>.Handle));
            await (Task) method.Invoke(processor, new Object[] {message, ex, cancellationToken});
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ListenCommand(stoppingToken);
            return Task.CompletedTask;
        }
    }
}