using System;
using Microsoft.Extensions.DependencyInjection;

namespace Rabbita.Entity.FluentExtensions
{
    public static class RabbitaPersistentBuilder
    {
        public static IServiceCollection AddRabbitaPersistent(this IServiceCollection serviceCollection, Action<RabbitaPersistentOptions> optionsBuilder)
        {
            var options = new RabbitaPersistentOptions();
            optionsBuilder(options);

            if (options.EntityMessagesExtractor == null)
                serviceCollection.AddRabbitaPersistent();
            else
                serviceCollection.AddSingleton<IEntityMessagesExtractor>(options.EntityMessagesExtractor);

            return serviceCollection;
        }

        public static IServiceCollection AddRabbitaPersistent(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IEntityMessagesExtractor, EntityMessagesExtractor>();
            return serviceCollection;
        }
    }
}
