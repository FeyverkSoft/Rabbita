using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rabbita.Entity.Worker;

namespace Rabbita.Entity.FluentExtensions
{
    public static class RabbitaPersistentBuilder
    {
        public static IServiceCollection AddRabbitaPersistent(this IServiceCollection serviceCollection,
            [NotNull] Action<RabbitaPersistentOptions> optionsBuilder)
        {
            var options = new RabbitaPersistentOptions();
            optionsBuilder(options);

            if (options.EntityMessagesExtractor == null)
                serviceCollection.AddSingleton<IEntityMessagesExtractor, EntityMessagesExtractor>();
            else
                serviceCollection.AddSingleton<IEntityMessagesExtractor>(options.EntityMessagesExtractor);

            serviceCollection.AddDbContext<WorkerDbContext>(optionsBuilder as Action<DbContextOptionsBuilder>);
            serviceCollection.AddHostedService<MessagePullingWorker>();

            return serviceCollection;
        }
    }
}