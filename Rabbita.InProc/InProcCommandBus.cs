namespace Rabbita.InProc;

using Core.Command;

internal sealed class InProcCommandBus : ICommandBus
{
    private AsyncConcurrentQueue<ICommand> Queue { get; }

    public InProcCommandBus(AsyncConcurrentQueue<ICommand> queue)
    {
        Queue = queue;
    }

    public async Task SendAsync(ICommand message)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        await Queue.EnqueueAsync(message);
    }

    public async Task SendAsync(IEnumerable<ICommand> messages)
    {
        if (messages == null)
            throw new ArgumentNullException(nameof(messages));

        foreach (var message in messages)
        {
            await Queue.EnqueueAsync(message);
        }
    }
}