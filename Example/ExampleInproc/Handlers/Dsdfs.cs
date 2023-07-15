using Rabbita.Core.Command;
using Rabbita.Core.Event;

namespace Example.Handlers
{
    public record Dsdfs : IEvent
    {
        public String Id { get; set; }
    }

    public record Dsdfs1 : IEvent
    {
        public String Id { get; set; }
    }

    public record Dsdfs2 : IEvent
    {
        public String Id { get; set; }
    }

    public record CDsdfs : ICommand
    {
        public String Id { get; set; }
    }

    public record CDsdfs1 : ICommand
    {
        public String Id { get; set; }
    }

    public record CDsdfs2 : ICommand
    {
        public String Id { get; set; }
    }
}