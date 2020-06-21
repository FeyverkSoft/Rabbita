using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rabbita.Core;

namespace Rabbita.Entity.FluentExtensions
{
    public static class RabbitaEntityTypeBuilder
    {
        /// <summary>
        ///     Indicates that this property contains a domain event
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        /// <param name="propertyExpression">
        ///     A lambda expression representing a property that should be treated as an event
        ///     (<c>blog => blog.Event</c>).
        /// </param>
        /// <returns></returns>
        /// <exception cref = "InvalidOperationException">
        ///     Event handler of type IEvent is already configured for the {Name} member.
        /// </exception>
        public static EntityTypeBuilder<TEntity> IsEvent<TEntity>(this EntityTypeBuilder<TEntity> builder,
            [NotNull] Expression<Func<TEntity, IEvent>> propertyExpression) where TEntity : class
        {
            var memberExpression = (MemberExpression) propertyExpression.Body;
            if (builder.Metadata.FindAnnotation(RabbitaMagicConst.EventMemberName) != null)
                throw new InvalidOperationException(
                    $"Event handler of type {nameof(IEvent)} is already configured for the \"{memberExpression.Member.Name}\" member.");
            if (builder.Metadata.FindAnnotation(RabbitaMagicConst.EventsMemberName) != null)
                throw new InvalidOperationException(
                    $"Events handler of type {nameof(IEnumerable<IEvent>)} is already configured for the \"{memberExpression.Member.Name}\" member.");
            builder = builder.Ignore(memberExpression.Member.Name);
            builder.Metadata.AddAnnotation(RabbitaMagicConst.EventMemberName, memberExpression.Member.Name);
            return builder;
        }

        /// <summary>
        ///     Indicates that this property contains a domain events
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        /// <param name="propertyExpression">
        ///     A lambda expression representing a property that should be treated as an events
        ///     (<c>blog => blog.Event</c>).
        /// </param>
        /// <returns></returns>
        /// <exception cref = "InvalidOperationException">
        ///     Event handler of type IEnumerable IEvent is already configured for the {Name} member.
        /// </exception>
        public static EntityTypeBuilder<TEntity> IsEvents<TEntity>(this EntityTypeBuilder<TEntity> builder,
            [NotNull] Expression<Func<TEntity, IEnumerable<IEvent>>> propertyExpression) where TEntity : class
        {
            var memberExpression = (MemberExpression) propertyExpression.Body;
            if (builder.Metadata.FindAnnotation(RabbitaMagicConst.EventMemberName) != null)
                throw new InvalidOperationException(
                    $"Event handler of type {nameof(IEvent)} is already configured for the \"{memberExpression.Member.Name}\" member.");
            if (builder.Metadata.FindAnnotation(RabbitaMagicConst.EventsMemberName) != null)
                throw new InvalidOperationException(
                    $"Events handler of type {nameof(IEnumerable<IEvent>)} is already configured for the \"{memberExpression.Member.Name}\" member.");
            builder = builder.Ignore(memberExpression.Member.Name);
            builder.Metadata.AddAnnotation(RabbitaMagicConst.EventsMemberName, memberExpression.Member.Name);
            return builder;
        }

        /// <summary>
        ///     Indicates that this property contains a domain command
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        /// <param name="propertyExpression">
        ///     A lambda expression representing a property that should be treated as an command
        ///     (<c>blog => blog.Event</c>).
        /// </param>
        /// <returns></returns>
        /// <exception cref = "InvalidOperationException">
        ///     Command handler of type ICommand is already configured for the {Name} member.
        /// </exception>
        public static EntityTypeBuilder<TEntity> IsCommand<TEntity>(this EntityTypeBuilder<TEntity> builder,
            [NotNull] Expression<Func<TEntity, ICommand>> propertyExpression) where TEntity : class
        {
            var memberExpression = (MemberExpression) propertyExpression.Body;
            if (builder.Metadata.FindAnnotation(RabbitaMagicConst.CommandMemberName) != null)
                throw new InvalidOperationException(
                    $"Command handler of type {nameof(ICommand)} is already configured for the \"{memberExpression.Member.Name}\" member.");
            if (builder.Metadata.FindAnnotation(RabbitaMagicConst.CommandsMemberName) != null)
                throw new InvalidOperationException(
                    $"Commands handler of type {nameof(IEnumerable<ICommand>)} is already configured for the \"{memberExpression.Member.Name}\" member.");
            builder = builder.Ignore(memberExpression.Member.Name);
            builder.Metadata.AddAnnotation(RabbitaMagicConst.CommandMemberName, memberExpression.Member.Name);
            return builder;
        }

        /// <summary>
        ///     Indicates that this property contains a domain commands
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        /// <param name="propertyExpression">
        ///     A lambda expression representing a property that should be treated as an commands
        ///     (<c>blog => blog.Event</c>).
        /// </param>
        /// <returns></returns>
        /// <exception cref = "InvalidOperationException">
        ///     Commands handler of type IEnumerable ICommand is already configured for the {Name} member.
        /// </exception>
        public static EntityTypeBuilder<TEntity> IsCommands<TEntity>(this EntityTypeBuilder<TEntity> builder,
            [NotNull] Expression<Func<TEntity, IEnumerable<ICommand>>> propertyExpression) where TEntity : class
        {
            var memberExpression = (MemberExpression) propertyExpression.Body;
            if (builder.Metadata.FindAnnotation(RabbitaMagicConst.CommandMemberName) != null)
                throw new InvalidOperationException(
                    $"Command handler of type {nameof(ICommand)} is already configured for the \"{memberExpression.Member.Name}\" member.");
            if (builder.Metadata.FindAnnotation(RabbitaMagicConst.CommandsMemberName) != null)
                throw new InvalidOperationException(
                    $"Commands handler of type {nameof(IEnumerable<ICommand>)} is already configured for the \"{memberExpression.Member.Name}\" member.");
            builder = builder.Ignore(memberExpression.Member.Name);
            builder.Metadata.AddAnnotation(RabbitaMagicConst.CommandsMemberName, memberExpression.Member.Name);
            return builder;
        }
    }
}