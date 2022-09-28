using System;

namespace Rabbita.Mq.Abstraction.Bus;

public record Exchanger(String ExchangerName, ExchangeType ExchangerType)
{
    public String ExchangerName { get; } = ExchangerName ?? throw new NullReferenceException(nameof(ExchangerName));
    public ExchangeType ExchangerType{ get; } = ExchangerType;
}