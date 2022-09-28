namespace Rabbita.Mq.SerializerResolver;

/*
/// <summary>
/// Резолвер десериализатора
/// </summary>
internal sealed class DefaultDeserializerResolver : IDeserializerResolver
{
    private readonly IReadOnlyDictionary<Type, IMessageSerializer?> _messageConfigs;
    private readonly IMessageSerializer _defaultSerializer;

    public DefaultDeserializerResolver(
        IMessageSerializer defaultSerializer,
        IConsumerBinderOptions options)
    {
        _messageConfigs =
            new ReadOnlyDictionary<Type, IMessageSerializer?>(options.RegisteredConsumerConfigs
                .Where(_ => _.Serializer is not null)
                .ToDictionary(
                    keys => keys.MessageType,
                    values => values.Serializer));
        _defaultSerializer = defaultSerializer;
    }

    /// <summary>
    /// Выполняет резолв сериализатора для сообщения
    /// </summary>
    public IMessageSerializer Resolve([NotNull] MessageInfo messageInfo)
    {
        return (_messageConfigs.TryGetValue(message.MessageType, out var serializer) ? serializer : null) ?? _defaultSerializer;
    }
  
}
*/