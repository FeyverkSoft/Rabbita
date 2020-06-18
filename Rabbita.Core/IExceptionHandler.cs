using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbita.Core
{
    public interface IExceptionHandler { }

    public interface IExceptionHandler<in T> : IExceptionHandler where T : notnull, Exception
    {
        public Task Handle(IMessage message, T exception, CancellationToken cancellationToken);
    }
}