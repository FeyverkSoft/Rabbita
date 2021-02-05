namespace Rabbita.Core
{
    /// <summary>
    /// Шина для отправки событий
    /// </summary>
    public interface IEventBus: IBus<IEvent>
    {
    }
}
