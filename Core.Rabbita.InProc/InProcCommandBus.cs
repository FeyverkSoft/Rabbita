using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Rabbita.Infrastructure;

namespace Core.Rabbita.InProc
{
    internal sealed class InProcCommandBus : ICommandBus
    {
        private AsyncConcurrentQueue<ICommand> Queue { get; }

        public InProcCommandBus(AsyncConcurrentQueue<ICommand> queue)
        {
            Queue = queue;
        }

        public async Task Send(ICommand message)
        {
            await Queue.EnqueueAsync(message);
        }

        public async Task Send(IEnumerable<ICommand> messages)
        {
            foreach (var message in messages){
                await Queue.EnqueueAsync(message);
            }
        }
    }
}