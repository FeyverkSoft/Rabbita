using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;

using Rabbita.Entity.Entity;

namespace Rabbita.Entity.Worker
{
    internal sealed class WorkerDbContext : DbContext
    {
        internal DbSet<MessageInfo> Messages { get; set; }

        public WorkerDbContext([NotNull] DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MessageInfo>();
        }
    }
}