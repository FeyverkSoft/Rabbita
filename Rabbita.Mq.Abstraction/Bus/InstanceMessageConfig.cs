using System;
using System.Collections.Generic;

namespace Rabbita.Mq.Abstraction.Bus;

public record InstanceMessageConfig
{
    public HashSet<String> Queues { get; } = new();
    public HashSet<Exchanger> Exchangers { get; }= new();
}