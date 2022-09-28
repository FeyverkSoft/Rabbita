namespace Rabbita.InProc.FluentExtensions.Command;

using System.Linq;

using Core.Command;
using Core.Helpers;

public sealed class CommandHandlerRegistry : ICommandHandlerRegistry
{
    private readonly Dictionary<Type, (Type Handler, ICommandConsumerConfig Config)> _handlers = new();
    public IEnumerable<(Type Handler, ICommandConsumerConfig Config)> RegisteredHandlers => _handlers.Values;

    public ICommandConsumerConfigBuilder<T> Register<T>() where T : ICommandHandler
    {
        var type = typeof(T);
        var supportedQueryTypes = type.GetGenericInterfaces(typeof(ICommandHandler<>));

        if (supportedQueryTypes.Count == 0)
            throw new ArgumentException("The handler must implement the ICommandHandler<> interface.");
        if (_handlers.Keys.Any(registeredType => supportedQueryTypes.Contains(registeredType)))
            throw new ArgumentException("The command handled by the received handler already has a registered handler.");

        var config = new CommandConsumerConfig();

        foreach (var commandsType in supportedQueryTypes)
            _handlers.TryAdd(commandsType, (type, config));

        return new CommandConsumerConfigBuilder<T>(config);
    }

    public (Type Handler, ICommandConsumerConfig Config) GetHandlerFor(ICommand @command)
    {
        if (@command == null)
            throw new ArgumentException("The command can't be null");
        if (!_handlers.TryGetValue(@command.GetType(), out var type))
            throw new KeyNotFoundException("Not found Hanlder");
        return type;
    }
}