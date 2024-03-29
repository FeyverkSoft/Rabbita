﻿using System.Threading;
using System.Threading.Tasks;

namespace Rabbita.Core
{
    public interface IMessageDispatcher<in T> where T : notnull, IMessage
    {
        Task Dispatch(T message, CancellationToken cancellationToken);
    }
}
