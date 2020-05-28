using System.Threading;
using System.Threading.Tasks;

namespace Rabbita.Core
{
    public interface ICommandHandler { }

    public interface ICommandHandler<in T> : ICommandHandler where T : ICommand, IMessage
    {
        public Task Handle(T message, CancellationToken cancellationToken);
    }
}