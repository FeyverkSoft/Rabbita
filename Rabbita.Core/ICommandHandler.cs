using System.Threading;
using System.Threading.Tasks;

namespace Rabbita.Core
{
    public interface ICommandHandler { }

    public interface ICommandHandler<in T> : ICommandHandler where T : notnull, ICommand, IMessage
    {
        public Task Handle(T command, CancellationToken cancellationToken);
    }
}