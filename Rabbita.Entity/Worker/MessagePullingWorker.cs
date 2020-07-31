using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Rabbita.Core;
using Rabbita.Core.MessageSerializer;
using Rabbita.Entity.Entity;

namespace Rabbita.Entity.Worker
{
    internal sealed class MessagePullingWorker : BackgroundService
    {
        private readonly WorkerDbContext _dbContext;
        private readonly IEventBus _eventBus;
        private readonly ICommandBus _commandBus;
        private readonly IMessageSerializer _serializer;
        private readonly ILogger _logger;

        public MessagePullingWorker(
            ILogger<MessagePullingWorker> logger,
            WorkerDbContext dbContext,
            IEventBus eventBus,
            ICommandBus commandBus,
            IMessageSerializer serializer
            )
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
            _commandBus = commandBus;
            _serializer = serializer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            var query = _dbContext.Messages.Where(_ => !_.IsSent).OrderBy(_ => _.Order);
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var num = (Int32)(Math.Log(await query.CountAsync(cancellationToken), 2) * 5);

                    foreach (var messageInfo in query.Take((5 * num) + 1))
                    {
                        switch (messageInfo.MessageType)
                        {
                            case MessageType.Event:
                                await _eventBus.Send((IEvent)_serializer.Deserialize(messageInfo.Body));
                                break;

                            case MessageType.Command:
                                await _commandBus.Send((ICommand)_serializer.Deserialize(messageInfo.Body));
                                break;
                        }

                        messageInfo.MarkAsSent();
                    }

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    if (num >= 100)
                        num = 98;
                    await Task.Delay(millisecondsDelay: 100 - num, cancellationToken: cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    await Task.Delay(millisecondsDelay: 2500, cancellationToken: cancellationToken);
                }
            }
        }
    }
}
