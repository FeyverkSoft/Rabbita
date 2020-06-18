using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rabbita.Entity.Entity
{

    [Table("__RabbitaMessage")]
    public sealed class MessageInfo
    {
        [Required]
        [Key]
        public Guid Id { get; }
        public DateTime CreateDate { get; } = DateTime.UtcNow;
        public DateTime UpdateDate { get; } = DateTime.UtcNow;
        public Int32 Order { get; } = 0;
        public String Type { get; }
        public String Body { get; }
        public Boolean IsSent { get; } = false;

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
