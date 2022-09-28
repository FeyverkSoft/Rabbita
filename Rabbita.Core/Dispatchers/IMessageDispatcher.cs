namespace Rabbita.Core.Dispatchers;

public interface IMessageDispatcher
{
    Task Dispatch(MessageDispatchInfo messageInfo, CancellationToken cancellationToken);
}