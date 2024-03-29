﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Rabbita.Core;
using Rabbita.Core.Infrastructure;

namespace Rabbita.InProc
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
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            await Queue.EnqueueAsync(message);
        }

        public async Task Send(IEnumerable<IEvent> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            foreach (var message in messages)
            {
                await Queue.EnqueueAsync(message);
            }
        }
    }
}