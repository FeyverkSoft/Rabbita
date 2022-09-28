namespace Rabbita.Entity;

using Microsoft.EntityFrameworkCore.ChangeTracking;

using Entity;

public interface IEntityMessagesExtractor
{
    IAsyncEnumerable<MessageInfo> Extract(EntityEntry? entityEntry, [EnumeratorCancellation] CancellationToken cancellationToken = default);
}