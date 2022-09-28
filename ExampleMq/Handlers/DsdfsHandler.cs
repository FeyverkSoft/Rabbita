using System;
using System.Threading;
using System.Threading.Tasks;

using Rabbita.Core;
using Rabbita.Core.Event;
using Rabbita.Core.Message;

namespace Example.Handlers
{
    public sealed class DsdfsHandler : IEventHandler<Dsdfs>,
        IExceptionHandler<Exception>
    {
        public DsdfsHandler()
        {
        }

        public async Task HandleAsync(Dsdfs @event, CancellationToken cancellationToken)
        {
            var dd = @event;
            return;
        }

        public async Task ExceptionHandleAsync(IMessage message, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}