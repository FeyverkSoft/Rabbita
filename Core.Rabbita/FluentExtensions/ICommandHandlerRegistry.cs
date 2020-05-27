using System;

namespace Core.Rabbita.FluentExtensions
{
    public interface ICommandHandlerRegistry
    {
        ICommandHandlerRegistry Register<T>() where T : ICommandHandler;
        Type GetHandlerFor(ICommand @command);
    }
}
