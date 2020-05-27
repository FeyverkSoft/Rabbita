using System.Threading;
using System.Threading.Tasks;

namespace Core.Rabbita
{
    public interface IMessageDispatcher<in T> where T : IMessage
    {
        Task Dispatch(T message, CancellationToken cancellationToken);
    }
}
