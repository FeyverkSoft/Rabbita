﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rabbita.Core
{
    public interface IBus<in T> where T : notnull, IMessage
    {
        public Task Send(T message);
        public Task Send(IEnumerable<T> messages);
    }
}
