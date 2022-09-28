using System.Collections.Generic;
using System.Reflection;

using Rabbita.Core.MessageSerializer;

namespace Rabbita.Mq.Abstraction.Bus;

public interface IMessageConfig
{
    PropertyInfo? Key { get; }
    IMessageSerializer? Serializer { get; }
    HashSet<InstanceMessageConfig> Instances { get; }
}