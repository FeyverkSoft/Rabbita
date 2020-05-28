using System.Threading;
using System.Threading.Tasks;

namespace Rabbita.Core
{
    public interface IEventHandler { }

    public interface IEventHandler<in T> : IEventHandler where T : IEvent, IMessage
    {
        public Task Handle(T message, CancellationToken cancellationToken);
    }
}