using Microsoft.EntityFrameworkCore;

namespace Rabbita.Entity.FluentExtensions
{
    public sealed class RabbitaPersistentOptions : DbContextOptionsBuilder
    {
        public IEntityMessagesExtractor? EntityMessagesExtractor { get; set; }
    }
}