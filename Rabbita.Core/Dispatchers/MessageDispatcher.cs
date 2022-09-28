namespace Rabbita.Core.Dispatchers;

using Microsoft.Extensions.DependencyInjection;

using Message;
using MessageSerializer;

public sealed class MessageDispatcher : IMessageDispatcher
{
    private readonly IDeserializerResolver _deserializerResolver;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMessageHandlerResolver _messageHandlerResolver;

    public MessageDispatcher(
        IDeserializerResolver deserializerResolver,
        IServiceScopeFactory serviceScopeFactory,
        IMessageHandlerResolver messageHandlerResolver)
    {
        _deserializerResolver = deserializerResolver;
        _serviceScopeFactory = serviceScopeFactory;
        _messageHandlerResolver = messageHandlerResolver;
    }

    public async Task Dispatch(MessageDispatchInfo messageInfo, CancellationToken cancellationToken)
    {
        var messageSerializer = _deserializerResolver.Resolve(messageInfo);
        var deserialize = messageSerializer.GetType().GetMethod(nameof(messageSerializer.Deserialize)).MakeGenericMethod(messageInfo.MessageType);
        var message = (IMessage)deserialize.Invoke(messageSerializer, new[] { messageInfo.Payload })!;
        using var scope = _serviceScopeFactory.CreateScope();
        var handlerType = _messageHandlerResolver.Resolve(message);
        var handler = scope.ServiceProvider.GetService(handlerType);
        var method = handler.GetType().GetMethod(nameof(IHandler<IMessage>.HandleAsync));

        await ((Task)method.Invoke(handler, new Object[] { message, cancellationToken })).ConfigureAwait(false);
    }
}