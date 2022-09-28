using System;

namespace Rabbita.Mq.Abstraction;

public interface IGlobalOptions
{
    Boolean EnableMultiTypeQueues { get; set; }
    IGlobalOptions AddInstance(String instanceName, Action<InstanceConfig> instanceOptions);
    IGlobalOptions AddDefaultInstance(Action<InstanceConfig> instanceOptions);
}

public interface IEventGlobalOptions : IGlobalOptions
{
}

public interface ICommandGlobalOptions : IGlobalOptions
{
}