using Fin.Api.Model;
using Fin.Api.Utils;
using Fin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Fin.Core.Handlers.Command.CreateAdjustmentTransaction;
using static Fin.Core.Handlers.Command.CreatePaymentTransaction;
using static Fin.Core.Handlers.Query.GetAccountBalance;

namespace Fin.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class FinController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FinController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Transaction([FromBody]CreateTransaction createTransaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            if(createTransaction.MessageType  == TransactionType.PAYMENT.ToString())
            {
                var result = await _mediator.Send(new CreatePaymentTransactionRequest()
                {
                    AccountId = createTransaction.AccountId,
                    Amount = createTransaction.Amount,
                    Origin = EnumUtils.GetEnumValueOrDefault<TransactionOrigin>(createTransaction.Origin).Value
                });
                return result.ToResponse();
            }
            else if (createTransaction.MessageType == TransactionType.ADJUSTMENT.ToString())
            {
                var result = await _mediator.Send(new CreateAdjustmentTransactionRequest()
                {
                    AccountId = createTransaction.AccountId,
                    Amount = createTransaction.Amount,
                    Origin = EnumUtils.GetEnumValueOrDefault<TransactionOrigin>(createTransaction.Origin).Value,
                    ParentTransactionId = createTransaction.TransactionId
                });
                return result.ToResponse();
            }
            else
            {
                return BadRequest(ValidationResult.Fail("Unsupported transaction type"));
            }
            return NotFound("Transaction not found");
        }

        public async Task<IActionResult> Get(long id)
        {
            var result = await _mediator.Send(new GetAccountBalanceRequest(id));
            return result.ToResponse();
        }

    }
}
