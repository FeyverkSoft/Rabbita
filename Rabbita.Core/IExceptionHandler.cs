namespace Rabbita.Core;

using Message;

public interface IExceptionHandler { }

public interface IExceptionHandler<in T> : IExceptionHandler where T : notnull, Exception
{
    public Task ExceptionHandleAsync(IMessage message, T exception, CancellationToken cancellationToken);
}