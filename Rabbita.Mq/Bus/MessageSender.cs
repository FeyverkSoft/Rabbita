using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Rabbita.Mq.Bus;

internal sealed class MessageSender : IMessageSender
{
    private readonly ILogger<MessageSender> _logger;
    private readonly Lazy<IModel> _publishChannel;

    public MessageSender(ILogger<MessageSender> logger,
        IConnectionFactory connectionFactory,
        IList<AmqpTcpEndpoint> endpoints)
    {
        _logger = logger;
        Lazy<IConnection> connection1 = new(() =>
        {
            var tryCount = 0;

            while (tryCount != 20)
            {
                var timeOut = TimeSpan.FromSeconds(tryCount + 1);
                tryCount++;
                try
                {
                    var connection = connectionFactory.CreateConnection(endpoints);
                    logger.LogInformation($"Connection is successful; {connection}");
                    return connection;
                }
                catch (BrokerUnreachableException e)
                {
                    logger.LogError($"Can't connect to the RabbitMq bus. Trying to reconnect in {timeOut} | Try[{tryCount}]. Exception {e}");
                    Task.Delay(timeOut).ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }

            throw new Exception("No connection to the event bus");
        });
        _publishChannel = new Lazy<IModel>(() => connection1.Value.CreateModel());
    }

    /// <summary>
    /// отправить сообщение в MQ
    /// </summary>
    public async Task Send([NotNull] BusMessageInfo message, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogTrace(
                $"Sending {message.MessageType}, key `{message.Key}` to `{message.ExchangerName ?? message.QueueName}, Direct type {message.ExchangerType}`");
            IBasicProperties props = _publishChannel.Value.CreateBasicProperties();
            props.ContentType = "text/plain";
            props.DeliveryMode = 2;
            props.Expiration = "36000000";
            foreach (var (key, value) in message.Headers)
            {
                props.Headers.Add(key, value);
            }

            var messageBodyBytes = Encoding.UTF8.GetBytes(message.Payload);

            _publishChannel.Value.ExchangeDeclare(message.ExchangerName, MapExchangeType(message));
            _publishChannel.Value.QueueDeclare(queue: message.QueueName, durable: true, exclusive: false, autoDelete: false);
            _publishChannel.Value.QueueBind(queue: message.QueueName, exchange: message.ExchangerName, routingKey: message.Key);

            _publishChannel.Value.BasicPublish(exchange: message.QueueName, routingKey: message.Key, body: messageBodyBytes);

            _logger.LogTrace($"Message sent `{message.Key}`");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                $"Failed to send message: {ex.Message}, {message.MessageType.FullName}, key `{message.Key}` to `{message.QueueName}, Direct type {message.ExchangerType}, {message.Payload}");
            throw new Exception($"Failed to produce message: {ex.Message}", ex);
        }
    }

    private static String MapExchangeType(BusMessageInfo messageInfo) => messageInfo.ExchangerType switch
    {
        Abstraction.Bus.ExchangeType.Direct => ExchangeType.Direct,
        Abstraction.Bus.ExchangeType.Fanout => ExchangeType.Fanout,
        Abstraction.Bus.ExchangeType.Topic => ExchangeType.Topic,
        Abstraction.Bus.ExchangeType.Header => ExchangeType.Headers,
        Abstraction.Bus.ExchangeType.ConsistentHash => "consistenthash",
        _ => throw new ArgumentOutOfRangeException()
    };
}