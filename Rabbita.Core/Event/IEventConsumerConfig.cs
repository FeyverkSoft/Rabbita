namespace Rabbita.Core.Event;

public interface IEventConsumerConfig
{
    (Type? handler, String? matchExceptionType) GetExceptionHandlerFor(Exception exception);
}