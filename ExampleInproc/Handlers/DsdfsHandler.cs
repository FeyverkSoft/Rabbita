using Rabbita.Core;
using Rabbita.Core.Event;
using Rabbita.Core.Message;

namespace Example.Handlers
{
    public sealed class DsdfsHandler : IEventHandler<Dsdfs>,
        IExceptionHandler<Exception>,
        IExceptionHandler<NotImplementedException>,
        IExceptionHandler<NotSupportedException>
    {
        public DsdfsHandler()
        {
        }

        public async Task HandleAsync(Dsdfs @event, CancellationToken cancellationToken)
        {
            var dd = @event;
            Console.WriteLine("Dsdfs: Hello World!");
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            throw new NotSupportedException();
            return;
        }

        public async Task ExceptionHandleAsync(IMessage message, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task ExceptionHandleAsync(IMessage message, NotImplementedException exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task ExceptionHandleAsync(IMessage message, NotSupportedException exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}