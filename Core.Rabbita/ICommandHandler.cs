using System.Threading;
using System.Threading.Tasks;

namespace Core.Rabbita
{
    public interface ICommandHandler { }

    public interface ICommandHandler<in T> : ICommandHandler where T : ICommand, IMessage
    {
        public Task Handle(T message, CancellationToken cancellationToken);
    }
}