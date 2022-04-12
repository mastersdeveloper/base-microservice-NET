using BASE.MICRONET.Cross.Event.Dir.Events;
using System.Threading.Tasks;

namespace BASE.MICRONET.Cross.Event.Dir.Bus
{
    public interface IEventHandler<in TEvent> : IEventHandler
         where TEvent : BASE.MICRONET.Cross.Event.Dir.Events.Event
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandler
    {

    }
}
