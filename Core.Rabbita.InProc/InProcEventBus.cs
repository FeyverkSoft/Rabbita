using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Rabbita.Infrastructure;

namespace Core.Rabbita.InProc
{
    internal sealed class InProcEventBus : IEventBus
    {
        private AsyncConcurrentQueue<IEvent> Queue { get; }

        public InProcEventBus(AsyncConcurrentQueue<IEvent> queue)
        {
            Queue = queue;
        }

        public async Task Send(IEvent message)
        {
            await Queue.EnqueueAsync(message);
        }

        public async Task Send(IEnumerable<IEvent> messages)
        {
            foreach (var message in messages){
                await Queue.EnqueueAsync(message);
            }
        }
    }
}