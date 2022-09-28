namespace Rabbita.Core;

using Message;

/// <summary>
/// Базовый интерфейс шины
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBus<in T> where T : notnull, IMessage
{
    /// <summary>
    /// Отправить сообщение в шину
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Task SendAsync(T message);

    /// <summary>
    /// Отправить список сообщений в шину
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    public Task SendAsync(IEnumerable<T> messages);
}