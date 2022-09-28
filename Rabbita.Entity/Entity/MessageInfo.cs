namespace Rabbita.Entity.Entity;

/// <summary>
/// Объект отражающий в бд сообщение готовое к отправки
/// </summary>
[Table("__RabbitaMessage")]
[DataObject]
public sealed class MessageInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [DataMember]
    public Guid Id { get; private set; }

    /// <summary>
    /// Дата создания сообщения
    /// </summary>
    [DataMember]
    public DateTime CreateDate { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата обновления сообщения
    /// </summary>
    [DataMember]
    public DateTime UpdateDate { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Порядок отправки сообщения
    /// </summary>
    [DataMember]
    public Int32 Order { get; private set; } = 0;

    /// <summary>
    /// Тип сообщения (с указанием сборки итд)
    /// </summary>
    [StringLength(512)]
    [DataMember]
    public String Type { get; private set; }

    /// <summary>
    /// Тело к отправке
    /// </summary>
    [StringLength(4096)]
    [DataMember]
    public String Body { get; private set; }

    /// <summary>
    /// Заголовки сообщения
    /// Если брокер поддерживает заголовки, то они будут отправлены
    /// </summary>    
    [StringLength(4096)]
    [DataMember]
    public String Headers { get; private set; } = "{}";

    /// <summary>
    /// Признак того что сообщение было успешно отправлено в брокер
    /// </summary>
    [DataMember]
    public Boolean IsSent { get; private set; } = false;

    /// <summary>
    /// эвент/команда
    /// </summary>
    [DataMember]
    [StringLength(32)]
    public String MessageType { get; set; }

    protected MessageInfo()
    {
    }

    public MessageInfo(Guid id, String messageType, String type, String body, Int32 order = 0)
    {
        Id = id;
        Order = order;
        Type = type;
        MessageType = messageType;
        Body = body;
    }

    /// <summary>
    /// Отметить как отпраленное в брокер
    /// </summary>
    internal void MarkAsSent()
    {
        IsSent = true;
        UpdateDate = DateTime.UtcNow;
    }
}

/// <summary>
/// Тип сообщения
/// </summary>
public static class MessageType
{
    public const String Event = "Event";
    public const String Command = "Command";
}