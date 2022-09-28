namespace Rabbita.Core.Event;

/// <summary>
/// Шина для отправки событий
/// </summary>
public interface IEventBus: IBus<IEvent>
{
}