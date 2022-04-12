using BASE.MICRONET.Cross.Event.Dir.Bus;
using MediatR; // permite trabajar el patron Mediador de manera rapida y sencilla
using BASE.MICRONET.Deposit.Messages.Commands;
using BASE.MICRONET.Deposit.Messages.Events;
using System.Threading;
using System.Threading.Tasks;

namespace BASE.MICRONET.Deposit.Messages.CommandHandlers
{
    //IRequestHandler: Manejador de solicitudes
    public class TransactionCommandHandler : IRequestHandler<TransactionCreateCommand, bool>
    {
        private readonly IEventBus _bus;
        public TransactionCommandHandler(IEventBus bus)
        {
            _bus = bus;
        }

        public Task<bool> Handle(TransactionCreateCommand request, CancellationToken cancellationToken)
        {
            //La publicacion solo trabaja en base a eventos 
            _bus.Publish(new TransactionCreatedEvent(
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
