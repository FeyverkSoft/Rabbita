using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;
using Rabbita.Core.MessageSerializer;

namespace Rabbita.Core.FluentExtensions
{
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
}