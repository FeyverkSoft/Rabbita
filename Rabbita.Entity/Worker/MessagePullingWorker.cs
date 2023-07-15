using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rabbita.Core.Command;
using Rabbita.Core.Event;
using Rabbita.Core.MessageSerializer;
using Rabbita.Entity.Entity;

namespace Rabbita.Entity.Worker;

using System.Reflection;

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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        while (!stoppingToken.IsCancellationRequested){
            await using var dbContext = _scopeFactory.CreateScope().ServiceProvider.GetService<WorkerDbContext>();
            var query = dbContext.Messages.Where(_ => !_.IsSent);

            if (_eventBus == null)
                query = query.Where(_ => _.MessageType != MessageType.Event);
            if (_commandBus == null)
                query = query.Where(_ => _.MessageType != MessageType.Command);

            query = query.OrderBy(_ => _.Order);

            try{
                var count = await query.CountAsync(stoppingToken);
                if (count == 0){
                    await Task.Delay(millisecondsDelay: 1500, cancellationToken: stoppingToken);
                    continue;
                }

                var num = (Int32) (Math.Log(count, 2) * 5);

                foreach (var messageInfo in query.Take((5 * num) + 1)){
                    switch (messageInfo.MessageType){
                        case MessageType.Event:
                            if (messageInfo.Body is not null)
                            {
                                await _eventBus?.SendAsync(_serializer.Deserialize<IEvent>(messageInfo.Body));
                            }

                            messageInfo.MarkAsSent();
                            break;

                        case MessageType.Command:
                            if (messageInfo.Body is not null){
                                await _commandBus?.SendAsync(_serializer.Deserialize<ICommand>(messageInfo.Body));
                            }

                            messageInfo.MarkAsSent();
                            break;
                    }

                    if (dbContext.Entry(messageInfo).State == EntityState.Unchanged)
                        dbContext.Attach(messageInfo);
                }

                await dbContext.SaveChangesAsync(stoppingToken);

                await Task.Delay(millisecondsDelay: 150 - (Int32) Math.Log(count, 2) * 2, cancellationToken: stoppingToken);
            }
            catch (Exception ex){
                _logger.LogError(ex.Message, ex);
                await Task.Delay(millisecondsDelay: 2500, cancellationToken: stoppingToken);
            }
        }
    }
}