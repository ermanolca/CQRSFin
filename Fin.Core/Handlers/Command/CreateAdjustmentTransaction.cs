using Fin.Core.Domain.Accounts;
using Fin.Core.Domain.Accounts.ValueObjects;
using Fin.Core.Infrastructure.Db;
using Fin.Core.Infrastructure.Db.Repository;
using Fin.Core.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fin.Core.Handlers.Command
{
    public static class CreateAdjustmentTransaction
    {

        public class CreateAdjustmentTransactionRequest : IRequest<FinResponse<CreateAdjustmentTransactionResponse>>
        {
            public long AccountId { get; set; }

            public decimal Amount { get; set; }

            public TransactionType MessageType { get; set; }

            public TransactionOrigin Origin { get; set; }

            public string ParentTransactionId { get; set; }

        }

        public class CreateAdjustmentTransactionRequestValidationHandler : IValidationHandler<CreateAdjustmentTransactionRequest>
        {
            private readonly IAccountRepository repository;
            public CreateAdjustmentTransactionRequestValidationHandler(IAccountRepository repository)
            {
                this.repository = repository;
            }

            public async Task<ValidationResult> Validate(CreateAdjustmentTransactionRequest request)
            {
                if (request.AccountId <= 0)
                {
                    return ValidationResult.Fail("Invalid account number");
                }

                var accs = await repository.GetAccounts();

                var account = await repository.GetAccount(request.AccountId);
                if (account == null)
                {
                    return ValidationResult.Fail("Account not found!");
                }
                Guid parentTransactionId;
                if(!Guid.TryParse(request.ParentTransactionId, out parentTransactionId))
                {
                    return ValidationResult.Fail("Transaction number to be adjusted is required");
                }

                var paymentTransaction = account.DebitTransactions.Find(x => x.Id == parentTransactionId);
                if(paymentTransaction == null)
                {
                    return ValidationResult.Fail("Transaction to be adjusted could not be found");
                }

                var adjustmentTransaction = account.CreditTransactions.Find(x => x.ParentTransactionId == parentTransactionId);
                if(adjustmentTransaction != null)
                {
                    return ValidationResult.Fail("Transaction have already been adjusted");
                }

                if(new Money(request.Amount) != paymentTransaction.Amount)
                {
                    return ValidationResult.Fail("Partial Adjustment is not supported");
                }

                if(paymentTransaction.Origin != request.Origin)
                {
                    return ValidationResult.Fail("Transaction origin must be same with original transaction");
                }

                return ValidationResult.Success();
            }
        }


        public class CreateAdjustmentTransactionResponse 
        {
            public Guid TransacitonId { get; set; }
        }

        public class CreateAdjustmentTransactionHandler : IRequestHandler<CreateAdjustmentTransactionRequest, FinResponse<CreateAdjustmentTransactionResponse>>
        {

            private readonly IAccountRepository repository;
            private readonly IUnitOfWork unitOfWork;
            public CreateAdjustmentTransactionHandler(IAccountRepository repository, IUnitOfWork unitOfWork)
            {
                this.repository = repository;
                this.unitOfWork = unitOfWork;
            }
            public async Task<FinResponse<CreateAdjustmentTransactionResponse>> Handle(CreateAdjustmentTransactionRequest request, CancellationToken cancellationToken)
            {
                var account = await repository.GetAccount(request.AccountId);
                /**
                 * Credit account to same of parent transaction
                 */
                var credit = new Credit(Guid.NewGuid(), request.AccountId, Guid.Parse(request.ParentTransactionId), new Money(request.Amount), TransactionType.ADJUSTMENT, request.Origin, DateTime.Now);
                account.Credit(credit);
                await repository.Update((Account)account, credit);
                /**
                  * If parent transaction was subject to commission, clear it also saving same same commission amount as credit
                 */

                var commissionTransaction = account.DebitTransactions.Find(x => x.ParentTransactionId == Guid.Parse(request.ParentTransactionId) && x.TransactionType == TransactionType.COMMISSION);
                if(commissionTransaction!=null)
                {
                    var commissionCredit = new Credit(Guid.NewGuid(), request.AccountId, commissionTransaction.Id, commissionTransaction.Amount, TransactionType.COMMISSION_ADJUSTMENT, request.Origin, DateTime.Now);
                    account.Credit(commissionCredit);
                    await repository.Update((Account)account, credit);
                }

                
                await unitOfWork.Save().ConfigureAwait(false);

                return FinResponse<CreateAdjustmentTransactionResponse>.Success(new CreateAdjustmentTransactionResponse() { TransacitonId = credit.Id });
            }
        }
    }
}
