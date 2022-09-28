namespace Rabbita.Core.Event;

using Message;

public interface IEventHandler
{
}

/// <summary>
/// интерфес для обработчиков событий
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IEventHandler<in T> : IHandler<T>,
    IEventHandler where T : notnull, IEvent, IMessage
{
}