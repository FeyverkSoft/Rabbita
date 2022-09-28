namespace Rabbita.Core.MessageSerializer;

using Dispatchers;

public interface IDeserializerResolver
{
    /// <summary>
    /// Выполняет резолв сериализатора для сообщения
    /// </summary>
    IMessageSerializer Resolve([NotNull] MessageDispatchInfo message);
}