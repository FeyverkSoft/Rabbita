using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using Rabbita.Core.Helpers;
using Rabbita.Core.Message;
using Rabbita.Core.MessageSerializer;
using Rabbita.Mq.Abstraction;
using Rabbita.Mq.Abstraction.Bus;

namespace Rabbita.Mq.Bus;

public class MessageConfigBuilder<T> : IMessageConfigBuilder<T> where T : IMessage
{
    private readonly MessageConfig _messageConfig;

    public MessageConfigBuilder([NotNull] IMessageConfig messageConfig)
    {
        _messageConfig = (MessageConfig)messageConfig;
    }

    public IMessageConfigBuilder<T> HasKey([NotNull] Expression<Func<T, Object>> expression)
    {
        if (_messageConfig.Key is not null)
            throw new InvalidOperationException($"Key for {typeof(T)} already bind");

        _messageConfig.Key = expression.GetPropertyAccess();
        return this;
    }

    public IMessageConfigBuilder<T> AddSerializer([NotNull] IMessageSerializer messageSerializer)
    {
        if (_messageConfig.Serializer is not null)
            throw new InvalidOperationException($"{nameof(messageSerializer)} for {typeof(T)} already bind");

        _messageConfig.Serializer = messageSerializer;
        return this;
    }

    public IMessageConfigBuilder<T> ToInstance([NotNull] String instanceName, Action<IInstanceBusConfigBuilder<T>> instanceRouting)
    {
        var instance = new InstanceMessageConfig();
        instanceRouting(new InstanceConfigBuilder<T>(instance));

        if (_messageConfig.Instances.Contains(instance))
            throw new InvalidOperationException($"{nameof(instanceName)} for {typeof(T)} already bind");

        _messageConfig.Instances.Add(instance);
        return this;
    }

    public IMessageConfigBuilder<T> ToDefaultInstance(Action<IInstanceBusConfigBuilder<T>> instanceRouting) =>
        ToInstance(RabbitaConst.DefaultInstanceName, instanceRouting);
}