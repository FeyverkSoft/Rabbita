using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Rabbita.Core.DafaultHandlers
{
    public sealed class ExceptionHandler : IExceptionHandler<Exception>
    {
        private readonly ILoggerFactory _loggerFactory;

        public ExceptionHandler(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public async Task Handle(IMessage message, Exception exception, CancellationToken cancellationToken)
        {
            _loggerFactory.CreateLogger<ExceptionHandler>().LogError(exception, $"{message.GetType()}: {exception.Message}");
        }
    }
}