using BASE.MICRONET.Cross.Event.Dir.Events;
using System;

namespace BASE.MICRONET.Cross.Event.Dir.Commands
{
    public abstract class Command : Message
    {
        public DateTime Timestamp { get; protected set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }
    }
}
