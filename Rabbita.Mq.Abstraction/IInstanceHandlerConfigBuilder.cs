using System;
using System.Diagnostics.CodeAnalysis;

using Rabbita.Core.Event;

namespace Rabbita.Mq.Abstraction;

public interface IInstanceHandlerConfigBuilder<T> where T : IEventHandler
{
    IInstanceHandlerConfigBuilder<T> FromQueue([NotNull] String queueName);
}