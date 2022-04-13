using Microsoft.AspNetCore.Mvc;
using BASE.MICRONET.Withdrawal.DTOs;
using BASE.MICRONET.Withdrawal.Services;
using System;
using BASE.MICRONET.Cross.Event.Dir.Bus;
using BASE.MICRONET.Withdrawal.Messages.Commands;

namespace BASE.MICRONET.Withdrawal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IEventBus _bus;

        public TransactionController(ITransactionService transactionService, IEventBus bus)
        {
            _transactionService = transactionService;
            _bus = bus;
        }

        [HttpPost("Withdrawal")]
        public IActionResult Withdrawal([FromBody] TransactionRequest request)
        {
            Models.Transaction transaction = new Models.Transaction()
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                CreationDate = DateTime.Now.ToShortDateString(),
                Type = "Withdrawal"
            };
            transaction = _transactionService.Withdrawal(transaction);

            var notificationCreateCommand = new NotificationCreateCommand(
                  idTransaction: transaction.Id,
                  amount: transaction.Amount,
                  type: transaction.Type,
                  creationDate: transaction.CreationDate,
                  accountId: transaction.AccountId
               );

            _bus.SendCommand(notificationCreateCommand);

            return Ok(transaction);
        }
    }
}
