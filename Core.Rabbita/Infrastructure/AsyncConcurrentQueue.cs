using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Core.Rabbita.InProc")]
namespace Core.Rabbita.Infrastructure
{
    internal sealed class AsyncConcurrentQueue<T> : IReadOnlyCollection<T>
    {
        private ConcurrentQueue<T> InternalQueue { get; }
        private SemaphoreSlim Semaphore { get; }

        public Int32 Count => InternalQueue.Count;

        public AsyncConcurrentQueue()
        {
            Semaphore = new SemaphoreSlim(0);
            InternalQueue = new ConcurrentQueue<T>();
        }

        public async Task<T> DequeueAsync(CancellationToken token = default)
        {
            await Semaphore.WaitAsync(token).ConfigureAwait(false);

            if (!InternalQueue.TryDequeue(out var command))
            {
                throw new InvalidOperationException("Internal queue is empty");
            }

            return command;
        }

        public async Task EnqueueAsync(T item)
        {
            InternalQueue.Enqueue(item);
            Semaphore.Release();
        }

        public IEnumerator<T> GetEnumerator() => InternalQueue.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => InternalQueue.GetEnumerator();
    }
}
