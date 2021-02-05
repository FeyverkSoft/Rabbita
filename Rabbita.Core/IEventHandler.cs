using System.Threading;
using System.Threading.Tasks;

namespace Rabbita.Core
{
    public interface IEventHandler
    {
    }

    /// <summary>
    /// интерфес для обработчиков событий
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventHandler<in T> : IEventHandler where T : notnull, IEvent, IMessage
    {
        public Task Handle(T @event, CancellationToken cancellationToken);
    }
}