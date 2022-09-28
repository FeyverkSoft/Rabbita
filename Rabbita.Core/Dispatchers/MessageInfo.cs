namespace Rabbita.Core.Dispatchers;

/// <summary>
/// Служебный объект для хранения состояния сообщения до его отправки
/// </summary>
public record MessageDispatchInfo
{
    /// <summary>
    /// Тип сообщения
    /// </summary>
    public Type MessageType { get; }

    /// <summary>
    /// Ключ сообщения
    /// </summary>
    public Dictionary<String, String>? Headers { get; }

    /// <summary>
    /// Само сообщение
    /// </summary>
    public String Payload { get; }

    public MessageDispatchInfo(Type messageType, String payload, Dictionary<String, String>? headers)
    {
        MessageType = messageType;
        Headers = headers;
        Payload = payload;
    }
}