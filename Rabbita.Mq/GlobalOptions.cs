using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Rabbita.Mq.Abstraction;

namespace Rabbita.Mq;

public class GlobalOptions :
    IEventGlobalOptions,
    ICommandGlobalOptions
{
    private readonly Dictionary<String, InstanceConfig> _instanceConfigs = new();

    public Boolean EnableMultiTypeQueues { get; set; }

    public IGlobalOptions AddInstance(String instanceName, Action<InstanceConfig> instanceOptions)
    {
        if (_instanceConfigs.ContainsKey(instanceName))
        {
            throw new ArgumentException($"instance named {instanceName} is already registered", nameof(instanceName));
        }

        var instanceConfig = new InstanceConfig();
        instanceOptions(instanceConfig);
        _instanceConfigs.Add(instanceName, instanceConfig);
        return this;
    }

    public IGlobalOptions AddDefaultInstance(Action<InstanceConfig> instanceOptions)
    {
        return AddInstance(RabbitaConst.DefaultInstanceName, instanceOptions);
    }

    public InstanceConfig GetConfig([NotNull] String instanceName)
    {
        return _instanceConfigs.TryGetValue(instanceName, out var instanceConfig) ? instanceConfig : _instanceConfigs[RabbitaConst.DefaultInstanceName];
    }
}