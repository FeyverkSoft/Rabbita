namespace Rabbita.Core.Command;

/// <summary>
/// Базовый интерфейс шины команд
/// </summary>
public interface ICommandBus : IBus<ICommand>
{
}