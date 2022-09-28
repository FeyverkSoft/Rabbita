using System;
using System.Collections.Generic;

using Rabbita.Mq.Abstraction.Bus;

namespace Rabbita.Mq.Bus;

public record BusMessageInfo
{
    /// <summary>
    /// Тип сообщения
    /// </summary>
    public Type MessageType { get; }
        
    /// <summary>
    /// Ключ сообщения
    /// </summary>
    public String? Key { get; private set; }

    /// <summary>
    /// Само сообщение
    /// </summary>
    public String Payload { get; }
        
    /// <summary>
    /// Заголовки для сообщения
    /// </summary>
    public IReadOnlyDictionary<String, String> Headers { get; private set; } = new Dictionary<String, String>();

    public String? ExchangerName { get; }
    public ExchangeType ExchangerType{ get; }
    public String? QueueName { get; set; }

}