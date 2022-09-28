using System;
using System.Diagnostics.CodeAnalysis;

using Rabbita.Core.Message;

namespace Rabbita.Mq.Abstraction.Bus;

public interface IInstanceBusConfigBuilder<T> where T : IMessage
{
    IInstanceBusConfigBuilder<T> ToQueue([NotNull] String queueName);
    IInstanceBusConfigBuilder<T> ToExchanger([NotNull] String exchangerName, [NotNull] ExchangeType exchangerType);
}