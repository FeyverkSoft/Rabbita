namespace Rabbita.Core.Dispatchers;

using Message;

public interface IMessageHandlerResolver
{
    Type Resolve(IMessage messageType);
}