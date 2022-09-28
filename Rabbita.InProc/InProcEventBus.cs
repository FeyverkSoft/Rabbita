namespace Rabbita.InProc;

using Core.Event;

internal sealed class InProcEventBus : IEventBus
{
    private AsyncConcurrentQueue<IEvent> Queue { get; }

    public InProcEventBus(AsyncConcurrentQueue<IEvent> queue)
    {
        Queue = queue;
    }

    public async Task SendAsync(IEvent message)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        await Queue.EnqueueAsync(message);
    }

    public async Task SendAsync(IEnumerable<IEvent> messages)
    {
        if (messages == null)
            throw new ArgumentNullException(nameof(messages));

        foreach (var message in messages)
        {
            await Queue.EnqueueAsync(message);
        }
    }
}