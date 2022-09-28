namespace Rabbita.Core.MessageSerializer;

using System.Diagnostics.CodeAnalysis;

using Message;

/// <summary>
/// Базовый интерфес сериализаторо и десериалитора сообщений
/// </summary>
public interface IMessageSerializer
{
    String Serialize([NotNull] in IMessage message);

    T? Deserialize<T>([NotNull] in String @object) where T : IMessage;
}