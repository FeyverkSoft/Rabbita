using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Rabbita.Entity
{
    public abstract class MessagingDbContext : DbContext
    {
        public MessagingDbContext([NotNull] DbContextOptions options) : base(options) { }

        public override async Task<Int32> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var ce = ChangeTracker.Entries()
                .Where(_ => _.State == EntityState.Added || _.State == EntityState.Modified);

            await using (var transaction = await Database.BeginTransactionAsync(cancellationToken))
            {
                foreach (var entityEntry in ce)
                {
                    var entity = entityEntry.Entity;
                    var eventMemberName = entityEntry.Metadata.GetAnnotation(RabbitaMagicConst.EventMemberName);
                    var eventsMemberName = entityEntry.Metadata.GetAnnotation(RabbitaMagicConst.EventsMemberName);
                }

                await base.SaveChangesAsync(cancellationToken);
                transaction.Commit();
            }

            return 0;
        }
    }
}
