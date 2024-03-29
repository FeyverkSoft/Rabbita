﻿using System;
using System.Collections.Generic;
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
        internal DbSet<MessageInfo> Messages { get; set; }

        public PersistentMessagingDbContext([NotNull] DbContextOptions options) : base(options)
        {
            var serviceProvider = options.GetExtension<CoreOptionsExtension>().ApplicationServiceProvider;
            EntityMessagesExtractor = serviceProvider.GetService<IEntityMessagesExtractor>() ??
                                      throw new InvalidOperationException(
                                          $"Service {nameof(IEntityMessagesExtractor)} wasn't registered in service collections; Please use AddRabbitaPersistent");
        }

        public override async Task<Int32> SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            var ce = ChangeTracker.Entries()
                .Where(_ => _.State == EntityState.Added || _.State == EntityState.Modified);

            var result = new List<MessageInfo>();
            foreach (var entityEntry in ce){
                await foreach (var messageInfo in EntityMessagesExtractor.Extract(entityEntry, cancellationToken)){
                    var entry = Entry(messageInfo);

                    if (entry.State == EntityState.Detached)
                        result.Add(messageInfo);
                }
            }

            await Messages.AddRangeAsync(result, cancellationToken);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override async Task<Int32> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await SaveChangesAsync(true, cancellationToken);
        }

        public override Int32 SaveChanges()
        {
            return SaveChangesAsync().Result;
        }

        public override Int32 SaveChanges(Boolean acceptAllChangesOnSuccess)
        {
            return SaveChangesAsync(acceptAllChangesOnSuccess).Result;
        }
    }
}