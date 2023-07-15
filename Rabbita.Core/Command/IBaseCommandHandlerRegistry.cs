namespace Rabbita.Core.Command;

public interface IBaseCommandHandlerRegistry
{
    (Type Handler, ICommandConsumerConfig Config) GetHandlerFor(ICommand @command);
}