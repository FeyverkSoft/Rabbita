namespace Rabbita.Core.MessageSerializer;

using System.Diagnostics.CodeAnalysis;

using Message;

public interface ISerializerResolver
{
    /// <summary>
    /// Выполняет резолв сериализатора для сообщения
    /// </summary>
    /// <param name="message"></param>
    IMessageSerializer Resolve([NotNull] IMessage message);
}