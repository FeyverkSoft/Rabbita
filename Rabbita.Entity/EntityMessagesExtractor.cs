using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

using Microsoft.EntityFrameworkCore.ChangeTracking;

using Rabbita.Core;
using Rabbita.Core.MessageSerializer;
using Rabbita.Entity.Entity;

namespace Rabbita.Entity
{
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

        private async IAsyncEnumerable<MessageInfo> ExtractEvents(EntityEntry? entityEntry,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (entityEntry == null)
                yield break;

            var entity = entityEntry.Entity;
            if (entity == null)
                yield break;

            var eventMemberName = entityEntry.Metadata.FindAnnotation(RabbitaMagicConst.EventMemberName);

            if (eventMemberName?.Value != null)
            {
                var @event = (IEvent)entity.GetType().GetProperty(eventMemberName.Value.ToString()).GetValue(entity);
                yield return new MessageInfo(Guid.NewGuid(), Serializer.Serialize(@event), 0);
            }

            var eventsMemberName = entityEntry.Metadata.FindAnnotation(RabbitaMagicConst.EventsMemberName);
            if (eventsMemberName?.Value == null)
                yield break;

            var events = (IEnumerable<IEvent>)entity.GetType().GetProperty(eventsMemberName.Value.ToString()).GetValue(entity);
            var order = 0;
            foreach (var @event in events)
            {
                yield return new MessageInfo(Guid.NewGuid(), Serializer.Serialize(@event), order++);
            }
        }

        private async IAsyncEnumerable<MessageInfo> ExtractCommands(EntityEntry? entityEntry,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (entityEntry == null)
                yield break;

            var entity = entityEntry.Entity;
            if (entity == null)
                yield break;

            var commandMemberName = entityEntry.Metadata.FindAnnotation(RabbitaMagicConst.CommandMemberName);

            if (commandMemberName?.Value != null)
            {
                var command = (IEvent)entity.GetType().GetProperty(commandMemberName.Value.ToString()).GetValue(entity);
                yield return new MessageInfo(Guid.NewGuid(), Serializer.Serialize(command), 0);
            }

            var commandsMemberName = entityEntry.Metadata.FindAnnotation(RabbitaMagicConst.CommandsMemberName);
            if (commandsMemberName?.Value == null)
                yield break;

            var commands = (IEnumerable<ICommand>)entity.GetType().GetProperty(commandsMemberName.Value.ToString()).GetValue(entity);
            var order = 0;
            foreach (var command in commands)
            {
                yield return new MessageInfo(Guid.NewGuid(), Serializer.Serialize(command), order++);
            }
        }
    }
}