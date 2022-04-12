using BASE.MICRONET.Cross.Event.Dir.Bus;
using MediatR;
using BASE.MICRONET.Deposit.Messages.Commands;
using BASE.MICRONET.Deposit.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BASE.MICRONET.Deposit.Messages.CommandHandlers
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
                    request.MessageBody,
                    request.Address,
                    request.AccountId
                 ));
            return Task.FromResult(true);
        }
    }
}
