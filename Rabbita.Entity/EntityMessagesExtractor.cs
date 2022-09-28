namespace Rabbita.Entity;

using Microsoft.EntityFrameworkCore.ChangeTracking;

using Core.Event;
using Core.Message;
using Core.MessageSerializer;
using Core.Command;

using Entity;

public sealed class EntityMessagesExtractor : IEntityMessagesExtractor
{
    private IMessageSerializer Serializer { get; }

    public EntityMessagesExtractor(IMessageSerializer serializer)
    {
        Serializer = serializer;
    }

    public async IAsyncEnumerable<MessageInfo> Extract(EntityEntry? entityEntry, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (entityEntry == null)
            yield break;

        await foreach (var extractEvent in ExtractEvents(entityEntry, cancellationToken))
        {
            yield return extractEvent;
        }

        await foreach (var extractCommand in ExtractCommands(entityEntry, cancellationToken))
        {
            yield return extractCommand;
        }
    }

    private async IAsyncEnumerable<MessageInfo> ExtractEvents(EntityEntry? entityEntry, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (entityEntry is null)
            yield break;

        var entity = entityEntry.Entity;
        if (entity is null)
            yield break;

        cancellationToken.ThrowIfCancellationRequested();

        var eventMemberName = entityEntry.Metadata.FindAnnotation(RabbitaMagicConst.EventMemberName);

        if (eventMemberName?.Value != null)
        {
            var @event = (IEvent)entity.GetType().GetProperty(eventMemberName.Value.ToString()).GetValue(entity);
            yield return new MessageInfo(
                id: Guid.NewGuid(),
                messageType: MessageType.Event,
                type: GetType(@event),
                body: Serializer.Serialize(@event),
                order: 0);
        }

        var eventsMemberName = entityEntry.Metadata.FindAnnotation(RabbitaMagicConst.EventsMemberName);
        if (eventsMemberName?.Value == null)
            yield break;

        var events = (IEnumerable<IEvent>)entity.GetType().GetProperty(eventsMemberName.Value.ToString()).GetValue(entity);
        var order = 0;
        foreach (var @event in events)
        {
            yield return new MessageInfo(
                id: Guid.NewGuid(),
                messageType: MessageType.Event,
                type: GetType(@event),
                body: Serializer.Serialize(@event),
                order: order++);
        }
    }

    private async IAsyncEnumerable<MessageInfo> ExtractCommands(EntityEntry? entityEntry, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (entityEntry is null)
            yield break;

        var entity = entityEntry.Entity;
        if (entity is null)
            yield break;

        cancellationToken.ThrowIfCancellationRequested();

        var commandMemberName = entityEntry.Metadata.FindAnnotation(RabbitaMagicConst.CommandMemberName);

        if (commandMemberName?.Value != null)
        {
            var command = (IEvent)entity.GetType().GetProperty(commandMemberName.Value.ToString())?.GetValue(entity);
            yield return new MessageInfo(
                id: Guid.NewGuid(),
                messageType: MessageType.Command,
                type: GetType(command),
                body: Serializer.Serialize(command),
                order: 0);
        }

        var commandsMemberName = entityEntry.Metadata.FindAnnotation(RabbitaMagicConst.CommandsMemberName);
        if (commandsMemberName?.Value is null)
            yield break;

        if (entity.GetType().GetProperty(commandsMemberName.Value.ToString())?.GetValue(entity) is not IEnumerable<ICommand> commands)
            yield break;

        var order = 0;
        foreach (var command in commands)
        {
            yield return new MessageInfo(
                id: Guid.NewGuid(),
                messageType: MessageType.Command,
                type: GetType(command),
                body: Serializer.Serialize(command),
                order: order++);
        }
    }

    private String GetType(IMessage message)
    {
        return message.GetType().Name;
    }
}