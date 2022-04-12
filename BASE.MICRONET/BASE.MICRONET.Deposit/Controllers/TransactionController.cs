using Microsoft.AspNetCore.Mvc;
using BASE.MICRONET.Deposit.DTOs;
using BASE.MICRONET.Deposit.Services;
using System;
using BASE.MICRONET.Cross.Event.Dir.Bus;
using BASE.MICRONET.Deposit.Messages.Commands;

namespace BASE.MICRONET.Deposit.Controllers
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

        [HttpPost("Deposit")]
        public IActionResult Deposit([FromBody] TransactionRequest request)
        {
            Models.Transaction transaction = new Models.Transaction()
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                CreationDate = DateTime.Now.ToShortDateString(),
                Type = "Deposit"
            };
            transaction = _transactionService.Deposit(transaction);

            var transactionCreateCommand = new TransactionCreateCommand(
                   idTransaction: transaction.Id,
                   amount: transaction.Amount,
                   type: transaction.Type,
                   creationDate: transaction.CreationDate,
                   accountId: transaction.AccountId
                );

            _bus.SendCommand(transactionCreateCommand);

            return Ok(transaction);
        }
    }
}
