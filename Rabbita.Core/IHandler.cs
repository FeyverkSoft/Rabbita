namespace Rabbita.Core;

using Message;

public interface IHandler<in T> where T : notnull, IMessage
{
    Task HandleAsync(T @event, CancellationToken cancellationToken);
}