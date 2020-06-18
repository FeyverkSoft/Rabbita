using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rabbita.Entity.Entity;

namespace Rabbita.Entity
{
    public interface IEntityMessagesExtractor
    {
        IAsyncEnumerable<MessageInfo> Extract(EntityEntry? entityEntry, [EnumeratorCancellation] CancellationToken cancellationToken = default);
    }
}