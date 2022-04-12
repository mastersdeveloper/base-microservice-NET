using BASE.MICRONET.Cross.Event.Dir.Bus;
using BASE.MICRONET.History.Messages.Events;
using BASE.MICRONET.History.Models;
using BASE.MICRONET.History.Services;
using System.Threading.Tasks;

namespace BASE.MICRONET.History.Messages.EventHandlers
{
    public class TransactionEventHandler : IEventHandler<TransactionCreatedEvent>
    {
        private readonly IHistoryService _historyService;

        public TransactionEventHandler(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        public Task Handle(TransactionCreatedEvent @event)
        {
            _historyService.Add(new HistoryTransaction()
            {
                IdTransaction = @event.IdTransaction,
                Amount = @event.Amount,
                Type = @event.Type,
                CreationDate = @event.CreationDate,
                AccountId = @event.AccountId

            });
            return Task.CompletedTask;
        }
    }
}
