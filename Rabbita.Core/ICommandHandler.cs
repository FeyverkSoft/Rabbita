using System.Threading;
using System.Threading.Tasks;

namespace Rabbita.Core
{
    /// <summary>
    /// Интерфес обработчика команд
    /// </summary>
    public interface ICommandHandler { }

    /// <summary>
    /// Интерфес для обработчика команд
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommandHandler<in T> : ICommandHandler where T : notnull, ICommand, IMessage
    {
        public Task Handle(T command, CancellationToken cancellationToken);
    }
}