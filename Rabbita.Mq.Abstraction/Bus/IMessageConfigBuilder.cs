using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using Rabbita.Core.Message;
using Rabbita.Core.MessageSerializer;

namespace Rabbita.Mq.Abstraction.Bus;

public interface IMessageConfigBuilder<T> where T : IMessage
{
    IMessageConfigBuilder<T> HasKey([NotNull] Expression<Func<T, Object>> expression);
    IMessageConfigBuilder<T> AddSerializer([NotNull] IMessageSerializer messageSerializer);
    IMessageConfigBuilder<T> ToInstance([NotNull] String instanceName, Action<IInstanceBusConfigBuilder<T>> instanceRouting);
    IMessageConfigBuilder<T> ToDefaultInstance(Action<IInstanceBusConfigBuilder<T>> instanceRouting) => ToInstance(RabbitaConst.DefaultInstanceName, instanceRouting);
}