using System;

namespace Rabbita.Core.FluentExtensions
{
    public interface ICommandHandlerRegistry
    {
        ICommandHandlerRegistry Register<T>() where T : ICommandHandler;
        Type GetHandlerFor(ICommand @command);
    }
}
