using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Rabbita
{
    public interface IBus<in T> where T : IMessage
    {
        public Task Send(T message);
        public Task Send(IEnumerable<T> messages);
    }
}
