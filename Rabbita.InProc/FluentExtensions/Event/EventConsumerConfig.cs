namespace Rabbita.InProc.FluentExtensions.Event;

using Core.Event;

internal sealed class EventConsumerConfig : IEventConsumerConfig
{
    /// <summary>
    /// Список обработчиков ошибок
    /// Ключ, название типа ошибки, значение, тип обработчика
    /// </summary>
    internal Dictionary<String, Type> ExceptionHandlers { get; private init; } = new();

    public (Type? handler, String? matchExceptionType) GetExceptionHandlerFor(Exception exception)
    {
        var ext = exception.GetType();

        String? matchExceptionType;
        Type? type;
        var i = 0;

        do
        {
            matchExceptionType = ext.FullName; // не знаю когда такое может произойти, но...
            type = GetHandler(matchExceptionType);
            i++;
            ext = ext.BaseType;
        } while (type is null && ext?.BaseType is not null && i < 10);


        return (type, matchExceptionType);
    }

    Type? GetHandler(String fullName)
    {
        return ExceptionHandlers.TryGetValue(fullName, out var type) ? type : null;
    }
}