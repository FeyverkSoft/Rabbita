using System;
using System.Diagnostics.CodeAnalysis;

using Rabbita.Core.Message;
using Rabbita.Mq.Abstraction.Bus;

namespace Rabbita.Mq.Bus;

public class InstanceConfigBuilder<T> : IInstanceBusConfigBuilder<T> where T : IMessage
{
    private readonly InstanceMessageConfig _instanceConfig;

    public InstanceConfigBuilder(InstanceMessageConfig instanceMessageConfig)
    {
        _instanceConfig = instanceMessageConfig ?? throw new NullReferenceException($"{instanceMessageConfig} cannot be null");
    }

    public IInstanceBusConfigBuilder<T> ToQueue([NotNull] String queueName)
    {
        if (_instanceConfig.Queues.Contains(queueName))
            throw new InvalidOperationException($"{nameof(queueName)} for {typeof(T)} already bind");

        _instanceConfig.Queues.Add(queueName);
        return this;
    }

    public IInstanceBusConfigBuilder<T> ToExchanger([NotNull] String exchangerName, [NotNull] ExchangeType exchangerType)
    {
        var exchanger = new Exchanger(exchangerName, exchangerType);
        if (_instanceConfig.Exchangers.Contains(exchanger))
            throw new InvalidOperationException($"{nameof(exchangerName)} for {typeof(T)} already bind");

        _instanceConfig.Exchangers.Add(exchanger);
        return this;
    }
}