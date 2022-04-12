﻿using Microsoft.AspNetCore.Mvc;
using BASE.MICRONET.Withdrawal.DTOs;
using BASE.MICRONET.Withdrawal.Services;
using System;

namespace BASE.MICRONET.Withdrawal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController( ITransactionService transactionService)
        {
            _transactionService = transactionService;
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

            return Ok(transaction);
        }
    }
}
