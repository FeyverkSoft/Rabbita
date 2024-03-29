﻿using System;

using Microsoft.Extensions.DependencyInjection;

using Rabbita.Core;
using Rabbita.Core.FluentExtensions;
using Rabbita.Core.HandlerRegistry;
using Rabbita.Core.Infrastructure;

namespace Rabbita.InProc.FluentExtensions
{
    public static class EventProcessorBuilder
    {
        public static IServiceCollection AddEventProcessor(this IServiceCollection services, Action<IEventHandlerRegistry> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var queryHandlerRegistry = new EventHandlerRegistry();
            action(queryHandlerRegistry);

            foreach (var registeredHandler in queryHandlerRegistry.RegisteredHandlers){
                services.AddScoped(registeredHandler);
            }

            services.AddSingleton<IEventHandlerRegistry>(queryHandlerRegistry);
            services.AddHostedService<InternalEventProcessor>();
            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddSingleton<AsyncConcurrentQueue<IEvent>>();
            services.AddScoped<IEventBus, InProcEventBus>();
            return services;
        }
    }
}