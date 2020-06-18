using System;

using Microsoft.Extensions.DependencyInjection;

using Rabbita.Core;
using Rabbita.Core.FluentExtensions;
using Rabbita.Core.HandlerRegistry;
using Rabbita.Core.Infrastructure;

namespace Rabbita.InProc.FluentExtensions
{
    public static class ExceptionProcessorBuilder
    {
        public static IServiceCollection AddExceptionProcessor(this IServiceCollection services, Action<IExceptionHandlerRegistry> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var exceptionHandlerRegistry = new ExceptionHandlerRegistry();
            action(exceptionHandlerRegistry);

            foreach (var registeredHandler in exceptionHandlerRegistry.RegisteredHandlers){
                services.AddScoped(registeredHandler);
            }

            services.AddSingleton<IExceptionHandlerRegistry>(exceptionHandlerRegistry);
            return services;
        }
    }
}