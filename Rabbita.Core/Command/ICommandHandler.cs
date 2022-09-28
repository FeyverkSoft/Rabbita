namespace Rabbita.Core.Command;

using Message;

/// <summary>
/// Интерфес обработчика команд
/// </summary>
public interface ICommandHandler
{
}

/// <summary>
/// Интерфес для обработчика команд
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICommandHandler<in T> : IHandler<T>, ICommandHandler
    where T : notnull, ICommand, IMessage
{
}