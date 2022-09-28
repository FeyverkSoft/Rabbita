using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbita.Mq.Bus;

internal interface IMessageSender
{
    Task Send([NotNull] BusMessageInfo message, CancellationToken cancellationToken);
}