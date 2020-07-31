using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rabbita.Core;
using Rabbita.Core.MessageSerializer;
using Rabbita.Entity.Entity;

namespace Rabbita.Entity.Worker
{
    internal sealed class MessagePullingWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IEventBus? _eventBus;
        private readonly ICommandBus? _commandBus;
        private readonly IMessageSerializer _serializer;
        private readonly ILogger _logger;

        public MessagePullingWorker(
            ILogger<MessagePullingWorker> logger,
            IServiceScopeFactory scopeFactory,
            IMessageSerializer serializer
        )
        {
            _scopeFactory = scopeFactory;
            _eventBus = _scopeFactory.CreateScope().ServiceProvider.GetService<IEventBus>();
            _commandBus = _scopeFactory.CreateScope().ServiceProvider.GetService<ICommandBus>();
            _serializer = serializer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            await using var dbContext = _scopeFactory.CreateScope().ServiceProvider.GetService<WorkerDbContext>();
            var query = dbContext.Messages.Where(_ => !_.IsSent);

            if (_eventBus == null)
                query = query.Where(_ => _.MessageType != MessageType.Event);
            if (_commandBus == null)
                query = query.Where(_ => _.MessageType != MessageType.Command);

            query = query.OrderBy(_ => _.Order);

            while (!cancellationToken.IsCancellationRequested){
                try{
                    var count = await query.CountAsync(cancellationToken);
                    if (count == 0){
                        await Task.Delay(millisecondsDelay: 500, cancellationToken: cancellationToken);
                        continue;
                    }

                    var num = (Int32) (Math.Log(count, 2) * 5);

                    foreach (var messageInfo in query.Take((5 * num) + 1).ToList()){
                        switch (messageInfo.MessageType){
                            case MessageType.Event:
                                if (messageInfo.Body != null){
                                    await _eventBus?.Send((IEvent) _serializer.Deserialize(messageInfo.Body));
                                }

                                messageInfo.MarkAsSent();
                                break;

                            case MessageType.Command:
                                if (messageInfo.Body != null){
                                    await _commandBus?.Send((ICommand) _serializer.Deserialize(messageInfo.Body));
                                }

                                messageInfo.MarkAsSent();
                                break;
                        }
                    }

                    await dbContext.SaveChangesAsync(cancellationToken);

                    if (num >= 100)
                        num = 98;
                    await Task.Delay(millisecondsDelay: 100 - num, cancellationToken: cancellationToken);
                }
                catch (Exception ex){
                    _logger.LogError(ex.Message, ex);
                    await Task.Delay(millisecondsDelay: 2500, cancellationToken: cancellationToken);
                }
            }
        }
    }
}