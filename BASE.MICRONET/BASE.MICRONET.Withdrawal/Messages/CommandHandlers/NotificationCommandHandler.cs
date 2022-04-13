
using BASE.MICRONET.Cross.Event.Dir.Bus;
using BASE.MICRONET.Withdrawal.Messages.Commands;
using BASE.MICRONET.Withdrawal.Messages.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BASE.MICRONET.Withdrawal.Messages.CommandHandlers
{
    public class NotificationCommandHandler : IRequestHandler<NotificationCreateCommand, bool>
    {
        private readonly IEventBus _bus;
        public NotificationCommandHandler(IEventBus bus)
        {
            _bus = bus;
        }

        public Task<bool> Handle(NotificationCreateCommand request, CancellationToken cancellationToken)
        {
            _bus.Publish(new NotificationCreatedEvent(
                   request.IdTransaction,
                   request.Amount,
                   request.Type,
                   request.CreationDate,
                   request.AccountId
               ));

            return Task.FromResult(true);
        }
    }
}
