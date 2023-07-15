namespace Rabbita.Core.Command;

public interface ICommandConsumerConfig
{
    (Type? handler, String? matchExceptionType) GetExceptionHandlerFor(Exception exception);
}