using Microsoft.Extensions.DependencyInjection;
using Rabbita.Core.MessageSerializer;

namespace Rabbita.Core.FluentExtensions
{
    public static class MessageSerializerBuilder
    {
        public static IServiceCollection AddRabbitaSerializer<T>(this IServiceCollection services)
            where T : notnull, IMessageSerializer, new()
        {
            services.AddSingleton<IMessageSerializer>(new T());
            return services;
        }
        public static IServiceCollection AddRabbitaSerializer(this IServiceCollection services)
        {
            services.AddSingleton<IMessageSerializer>(new MessageSerializer.MessageSerializer());
            return services;
        }
    }
}