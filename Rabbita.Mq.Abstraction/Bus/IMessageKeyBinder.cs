using Rabbita.Core.Command;
using Rabbita.Core.Event;
using Rabbita.Core.Message;

namespace Rabbita.Mq.Abstraction.Bus;

public interface IMessageKeyBinder<in TAdditional>
{
    IMessageConfigBuilder<T> BindMessage<T>() where T : IMessage, TAdditional;
}

public interface ICommandKeyBinder : IMessageKeyBinder<ICommand>
{
}

public interface IEventKeyBinder : IMessageKeyBinder<IEvent>
{
}