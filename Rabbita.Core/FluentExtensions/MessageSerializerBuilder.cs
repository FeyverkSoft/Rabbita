namespace Rabbita.Core.FluentExtensions;

using Microsoft.Extensions.DependencyInjection;

using MessageSerializer;

public static class MessageSerializerBuilder
{
    public static IServiceCollection AddRabbitaDefaultSerializer<T>([NotNull] this IServiceCollection services)
        where T : notnull, IMessageSerializer, new()
    {
        services.AddSingleton<IMessageSerializer>(new T());
        return services;
    }

    public static IServiceCollection AddRabbitaDefaultSerializer([NotNull] this IServiceCollection services)
    {
        services.AddSingleton<IMessageSerializer>(new JsonMessageSerializer());
        return services;
    }
}