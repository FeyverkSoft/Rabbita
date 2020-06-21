using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Rabbita.Entity.Entity
{
    [Table("__RabbitaMessage")]
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
        public String Type { get; private set; }

        [StringLength(4096)]
        public String Body { get; private set; }

        [DataMember]
        public Boolean IsSent { get; private set; } = false;

        protected MessageInfo() { }

        public MessageInfo(Guid id, String body, Int32 order = 0)
        {
            Id = id;
            Order = order;
            Type = body.GetType().FullName;
            Body = body;
        }
    }
}