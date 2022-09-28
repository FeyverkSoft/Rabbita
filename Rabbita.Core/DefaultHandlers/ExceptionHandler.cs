namespace Rabbita.Core.DefaultHandlers;

using Microsoft.Extensions.Logging;

using Message;

public sealed class ExceptionHandler : IExceptionHandler<Exception>
{
    private readonly ILoggerFactory _loggerFactory;

    public ExceptionHandler(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public async Task ExceptionHandleAsync(IMessage message, Exception exception, CancellationToken cancellationToken)
    {
        _loggerFactory.CreateLogger<ExceptionHandler>().LogError(exception, $"{message.GetType()}: {exception.Message}");
    }
}