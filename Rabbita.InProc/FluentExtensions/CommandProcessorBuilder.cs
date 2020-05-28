using System;

using Microsoft.Extensions.DependencyInjection;

using Rabbita.Core;
using Rabbita.Core.FluentExtensions;
using Rabbita.Core.HandlerRegistry;
using Rabbita.Core.Infrastructure;

namespace Rabbita.InProc.FluentExtensions
{
    public static class CommandProcessorBuilder
    {
        public static IServiceCollection AddCommandProcessor(this IServiceCollection services, Action<ICommandHandlerRegistry> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var queryHandlerRegistry = new CommandHandlerRegistry();
            action(queryHandlerRegistry);

            foreach (var registeredHandler in queryHandlerRegistry.RegisteredHandlers){
                services.AddScoped(registeredHandler);
            }

            services.AddSingleton<ICommandHandlerRegistry>(queryHandlerRegistry);
            services.AddHostedService<InternalCommandProcessor>();
            return services;
        }

        public static IServiceCollection AddCommandBus(this IServiceCollection services)
        {
            services.AddSingleton<AsyncConcurrentQueue<ICommand>>();
            services.AddScoped<ICommandBus, InProcCommandBus>();
            return services;
        }
    }
}