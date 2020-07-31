using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Rabbita.Entity.Migration;

namespace Rabbita.Entity.MariaDbTarget
{
    public static class MessageMigrationBuilder
    {
        public static IServiceCollection AddRabbitaDbPersistentMigrator(this IServiceCollection services, [NotNull] Action<MessagingDbOptions> configure)
        {
            var options = new MessagingDbOptions();
            configure.Invoke(options);
            services.AddSingleton(options);
            services.AddSingleton<IDbMigrationService, MariaDbMigrationService>();
            return services;
        }
    }
}