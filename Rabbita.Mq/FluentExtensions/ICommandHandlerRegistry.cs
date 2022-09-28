using System;

using Rabbita.Core.Command;

namespace Rabbita.Mq.FluentExtensions;

public interface ICommandHandlerRegistry
{
    ICommandHandlerRegistry Register<T>() where T : ICommandHandler;
    Type GetHandlerFor(ICommand @command);
}