namespace Rabbita.Core.Event;

using Message;

/// <summary>
/// Интерфейс маркер, указывающий то что объект является событием для шины
/// </summary>
public interface IEvent : IMessage
{
}