using System;
using System.Collections.Generic;

using Rabbita.Core.Command;
using Rabbita.Core.Event;
using Rabbita.Core.Message;
using Rabbita.Mq.Abstraction.Bus;

namespace Rabbita.Mq.Bus;

public record MessageKeyBinder :
    IMessageKeyBinder<IMessage>,
    ICommandKeyBinder,
    IEventKeyBinder
{
    private IDictionary<Type, IMessageConfig> MessageConfigs { get; } = new Dictionary<Type, IMessageConfig>();

    public IMessageConfig ResolveConfig(IMessage message)
    {
        if (MessageConfigs.TryGetValue(message.GetType(), out var conf))
            return conf;
        throw new Exception($"Message config of type {message.GetType().Name} not found");
    }

    IMessageConfigBuilder<T> IMessageKeyBinder<IMessage>.BindMessage<T>()
    {
        var type = typeof(T);
        if (MessageConfigs.ContainsKey(type))
            throw new InvalidOperationException($"Key for {type} already added");

        IMessageConfig messageConfig = new MessageConfig();
        MessageConfigs.Add(type, messageConfig);
        return new MessageConfigBuilder<T>(messageConfig);
    }

    IMessageConfigBuilder<T> IMessageKeyBinder<ICommand>.BindMessage<T>()
    {
        return ((IMessageKeyBinder<IMessage>)this).BindMessage<T>();
    }

    IMessageConfigBuilder<T> IMessageKeyBinder<IEvent>.BindMessage<T>()
    {
        return ((IMessageKeyBinder<IMessage>)this).BindMessage<T>();
    }
}