using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Rabbita.FluentExtensions;
using Core.Rabbita.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Rabbita.InProc
{
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
            ILogger<InternalEventProcessor> logger)
        {
            _queue = queue;
            _dispatchers = dispatchers;
            _provider = provider;
            _logger = logger;
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
                                    _logger.LogError(e, $"{processor}: {e.Message}");
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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ListenCommand(stoppingToken);
            return Task.CompletedTask;
        }
    }
}