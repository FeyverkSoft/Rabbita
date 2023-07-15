using Rabbita.Core;
using Rabbita.Core.Message;

namespace Example.Handlers
{
    using Rabbita.Core.Command;

    public sealed class CDsdfsHandler : ICommandHandler<CDsdfs>,
        IExceptionHandler<Exception>,
        IExceptionHandler<NotImplementedException>
    {
        public CDsdfsHandler()
        {
            Console.WriteLine("CDsdfsHandler");
        }

        public async Task HandleAsync(CDsdfs @event, CancellationToken cancellationToken)
        {
            var dd = @event;
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            Console.WriteLine("CDsdfs: Hello World!");
            return;
        }

        public async Task ExceptionHandleAsync(IMessage message, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine("CDsdfs ExceptionHandleAsync: Exception");
        }

        public async Task ExceptionHandleAsync(IMessage message, NotImplementedException exception, CancellationToken cancellationToken)
        {
            Console.WriteLine("CDsdfs ExceptionHandleAsync: NotImplementedException");
        }
    }
}