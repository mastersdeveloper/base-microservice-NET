using BASE.MICRONET.Cross.Event.Dir.Commands;
using System.Threading.Tasks;

namespace BASE.MICRONET.Cross.Event.Dir.Bus
{
    public interface IEventBus
    {
        Task SendCommand<T>(T command) where T : Command;

        void Publish<T>(T @event) where T : BASE.MICRONET.Cross.Event.Dir.Events.Event;

        void Subscribe<T, TH>()
            where T : BASE.MICRONET.Cross.Event.Dir.Events.Event
            where TH : IEventHandler<T>;
    }
}
