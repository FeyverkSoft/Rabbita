using System;
using Core.Rabbita.Core;
using Core.Rabbita.FluentExtensions;
using Core.Rabbita.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Rabbita.InProc.FluentExtensions
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