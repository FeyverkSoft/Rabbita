namespace Rabbita.Mq.FluentExtensions.Event
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Extensions.DependencyInjection;

    using Rabbita.Mq.Abstraction;
    using Rabbita.Mq.Abstraction.Bus;
    using Rabbita.Mq.Bus;

    public static class EventProcessorBuilder
    {
        public static IServiceCollection AddEventProcessor(this IServiceCollection services, [NotNull] Action<IGlobalOptions> options, [NotNull] Action<IEventHandlerRegistry> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var queryHandlerRegistry = new EventHandlerRegistry();
            action(queryHandlerRegistry);

            foreach (var registeredHandler in queryHandlerRegistry.RegisteredHandlers)
            {
                services.AddScoped(registeredHandler.Handler);
            }

            services.AddSingleton<IEventHandlerRegistry>(queryHandlerRegistry);
            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, [NotNull] Action<IEventGlobalOptions> optionsBuilder,
            Action<IEventKeyBinder> messageKeyBinder)
        {
            var globalOptions = new GlobalOptions();
            optionsBuilder(globalOptions);
            services.AddSingleton<IEventGlobalOptions>(globalOptions);

            var messageKey = new MessageKeyBinder();
            messageKeyBinder(messageKey);
            services.AddSingleton<IEventKeyBinder>(messageKey);
            services.AddSingleton<IEventKeyBinder>(messageKey);

            return services;
        }

        public static IServiceCollection AddCommandBus(this IServiceCollection services, [NotNull] Action<ICommandGlobalOptions> optionsBuilder,
            Action<ICommandKeyBinder> messageKeyBinder)
        {
            var globalOptions = new GlobalOptions();
            optionsBuilder(globalOptions);
            services.AddSingleton<ICommandGlobalOptions>(globalOptions);

            var messageKey = new MessageKeyBinder();
            messageKeyBinder(messageKey);
            services.AddSingleton<ICommandKeyBinder>(messageKey);

            return services;
        }
    }
}