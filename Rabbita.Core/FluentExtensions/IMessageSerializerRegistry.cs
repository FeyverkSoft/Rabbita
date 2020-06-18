using Microsoft.Extensions.DependencyInjection;
using Rabbita.Core.MessageSerializer;

namespace Rabbita.Core.FluentExtensions
{
    public static class MessageSerializerBuilder
    {
        public static IServiceCollection AddMessageSerializer<T>(this IServiceCollection services)
            where T : notnull, IMessageSerializer, new()
        {
            services.AddSingleton<IMessageSerializer>(new T());
            return services;
        }
    }
}