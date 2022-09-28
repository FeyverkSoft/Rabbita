using System.Collections.Generic;
using System.Reflection;

using Rabbita.Core.MessageSerializer;
using Rabbita.Mq.Abstraction.Bus;

namespace Rabbita.Mq.Bus;

public record MessageConfig : IMessageConfig
{
    public PropertyInfo? Key { get; internal set; }
    public IMessageSerializer? Serializer { get; internal set; }
    public HashSet<InstanceMessageConfig> Instances { get; } = new();
}