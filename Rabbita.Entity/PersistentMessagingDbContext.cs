using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Rabbita.Entity.Entity;

namespace Rabbita.Entity
{
    public abstract class PersistentMessagingDbContext : DbContext
    {
        private IEntityMessagesExtractor EntityMessagesExtractor { get; }
        private DbSet<MessageInfo> Messages { get; set; }

        public PersistentMessagingDbContext([NotNull] DbContextOptions options) : base(options)
        {
            var serviceProvider = options.GetExtension<CoreOptionsExtension>().ApplicationServiceProvider;
            EntityMessagesExtractor = serviceProvider.GetService<IEntityMessagesExtractor>() ??
                                      throw new InvalidOperationException(
                                          $"Service {nameof(IEntityMessagesExtractor)} wasn't registered in service collections");
        }

        public override async Task<Int32> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var ce = ChangeTracker.Entries()
                .Where(_ => _.State == EntityState.Added || _.State == EntityState.Modified);

            await using var transaction = await Database.BeginTransactionAsync(cancellationToken);

            foreach (var entityEntry in ce){
                await foreach (MessageInfo messageInfo in EntityMessagesExtractor.Extract(entityEntry, cancellationToken)){
                    await Messages.AddAsync(messageInfo, cancellationToken);
                }
            }

            await base.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return 0;
        }
    }
}