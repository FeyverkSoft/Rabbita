using System.Diagnostics.CodeAnalysis;

using Rabbita.Core.Message;
using Rabbita.Core.MessageSerializer;
using Rabbita.Mq.Bus;

namespace Rabbita.Mq.SerializerResolver;

/// <summary>
/// Резолвер сериализатора
/// </summary>
internal sealed class DefaultSerializerResolver : ISerializerResolver
{
    private readonly MessageKeyBinder _messageKeyBinder;
    private readonly IMessageSerializer _defaultSerializer;

    public DefaultSerializerResolver(IMessageSerializer defaultSerializer, MessageKeyBinder mkb)
    {
        _messageKeyBinder = mkb;
        _defaultSerializer = defaultSerializer;
    }

    public IMessageSerializer Resolve([NotNull] IMessage message)
    {
        return _messageKeyBinder.ResolveConfig(message).Serializer ?? _defaultSerializer;
    }
}