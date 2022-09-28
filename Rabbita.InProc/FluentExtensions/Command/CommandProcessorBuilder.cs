namespace Rabbita.InProc.FluentExtensions.Command;

using Microsoft.Extensions.DependencyInjection;

using Core.Command;

public static class CommandProcessorBuilder
{
    public static IServiceCollection AddCommandProcessor(this IServiceCollection services, Action<ICommandHandlerRegistry> action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        var queryHandlerRegistry = new CommandHandlerRegistry();
        action(queryHandlerRegistry);

        foreach (var registeredHandler in queryHandlerRegistry.RegisteredHandlers)
        {
            services.AddScoped(registeredHandler.Handler);
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