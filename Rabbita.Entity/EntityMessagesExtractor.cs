using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Rabbita.Core;
using Rabbita.Entity.Entity;

namespace Rabbita.Entity
{
    internal sealed class EntityMessagesExtractor : IEntityMessagesExtractor
    {
         MessageSerializer.Serialize(message);
        public async IAsyncEnumerable<MessageInfo> Extract(EntityEntry? entityEntry, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (entityEntry == null)
                yield break;

            await foreach (var @event in ExtractEvents(entityEntry, cancellationToken)){
                yield return @event;
            }

            await foreach (var command in ExtractCommands(entityEntry, cancellationToken)){
                yield return command;
            }
        }

        private async IAsyncEnumerable<MessageInfo> ExtractEvents(EntityEntry? entityEntry, CancellationToken cancellationToken)
        {
            if (entityEntry == null)
                yield break;

            var entity = entityEntry.Entity;
            if (entity == null)
                yield break;

            var eventMemberName = entityEntry.Metadata.GetAnnotation(RabbitaMagicConst.EventMemberName);
            
            if (eventMemberName?.Value != null){
                var @event = (IEvent) entity.GetType().GetProperty(eventMemberName.Value.ToString()).GetValue(entity);
                yield return new MessageInfo(Guid.NewGuid(),, 0);
            }

            var eventsMemberName = entityEntry.Metadata.GetAnnotation(RabbitaMagicConst.EventsMemberName);
            if (eventsMemberName?.Value != null){
                var events = (IEnumerable<IEvent>) entity.GetType().GetProperty(eventsMemberName.Value.ToString()).GetValue(entity);
                foreach (var @event in events){
                    yield return new MessageInfo(Guid.NewGuid(),, 0);
                }
            }
        }

        private async IAsyncEnumerable<MessageInfo> ExtractCommands(EntityEntry? entityEntry, CancellationToken cancellationToken)
        {
            if (entityEntry == null)
                yield break;

            var entity = entityEntry.Entity;
            if (entity == null)
                yield break;
        }
    }
}