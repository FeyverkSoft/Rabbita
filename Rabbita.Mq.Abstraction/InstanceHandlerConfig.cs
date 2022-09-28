using System;
using System.Collections.Generic;

namespace Rabbita.Mq.Abstraction;

public record InstanceHandlerConfig
{
    public HashSet<String> Queues { get; } = new();
}