namespace Rabbita.Core
{
    /// <summary>
    /// Базовый интерфейс шины команд
    /// </summary>
    public interface ICommandBus : IBus<ICommand>
    {
    }
}
