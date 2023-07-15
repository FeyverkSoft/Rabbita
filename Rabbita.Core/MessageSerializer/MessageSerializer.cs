namespace Rabbita.Core.MessageSerializer;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

using Message;

public sealed class JsonMessageSerializer : IMessageSerializer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public JsonMessageSerializer()
    {
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
        };

        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public String Serialize<T>([NotNull] in T message) where T : IMessage
    {
        return JsonSerializer.Serialize((object)message, _jsonSerializerOptions);
    }

    public T? Deserialize<T>([NotNull] in String @object) where T : IMessage
    {
        return JsonSerializer.Deserialize<T>(@object, _jsonSerializerOptions);
    }
}