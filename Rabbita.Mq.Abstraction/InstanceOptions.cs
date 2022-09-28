using System;

namespace Rabbita.Mq.Abstraction;

public record InstanceConfig
{
    public String ConnectionString { get; set; }
}