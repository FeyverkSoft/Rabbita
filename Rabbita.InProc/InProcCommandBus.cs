﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Rabbita.Core;
using Rabbita.Core.Infrastructure;


namespace Rabbita.InProc
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
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            await Queue.EnqueueAsync(message);
        }

        public async Task Send(IEnumerable<ICommand> messages)
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