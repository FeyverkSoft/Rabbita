using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Rabbita.Entity.Entity
{
    [Table("__RabbitaMessage")]
    [DataObject]
    public sealed class MessageInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DataMember]
        public Guid Id { get; private set; }

        [DataMember]
        public DateTime CreateDate { get; private set; } = DateTime.UtcNow;

        [DataMember]
        public DateTime UpdateDate { get; private set; } = DateTime.UtcNow;

        [DataMember]
        public Int32 Order { get; private set; } = 0;

        [StringLength(512)]
        [DataMember]
        public String Type { get; private set; }

        [StringLength(4096)]
        [DataMember]
        public String Body { get; private set; }

        [DataMember]
        public Boolean IsSent { get; private set; } = false;

        [DataMember]
        [StringLength(32)]
        public String MessageType { get; set; }

        protected MessageInfo() { }

        public MessageInfo(Guid id, String messageType, String type, String body, Int32 order = 0)
        {
            Id = id;
            Order = order;
            Type = type;
            MessageType = messageType;
            Body = body;
        }

        internal void MarkAsSent()
        {
            IsSent = true;
            UpdateDate = DateTime.UtcNow;
        }
    }
    public static class MessageType
    {
        public const String Event = "Event";
        public const String Command = "Command";
    }
}