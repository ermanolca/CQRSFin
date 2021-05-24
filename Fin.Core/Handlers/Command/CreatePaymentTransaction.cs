using Fin.Core.Domain;
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
    public static class CreatePaymentTransaction
    {
        public class CreatePaymentTransactionRequest : IRequest<FinResponse<CreatePaymentTransactionResponse>>
        {
            public long AccountId { get; set; }

            public decimal Amount { get; set; }

            public TransactionOrigin Origin { get; set; }

        }

        public class CreatePaymentTransactionResponse
        {
            public Guid TransacitonId { get; set; }
        }


        public class CreatePaymentTransactionRequestValidationHandler : IValidationHandler<CreatePaymentTransactionRequest>
        {
            private readonly IAccountRepository repository;
            private readonly ICommissionCalculator commissionCalculator;
            public CreatePaymentTransactionRequestValidationHandler(IAccountRepository repository, ICommissionCalculator commissionCalculator)
            {
                this.repository = repository;
                this.commissionCalculator = commissionCalculator;
            }

            public async Task<ValidationResult> Validate(CreatePaymentTransactionRequest request)
            {
                var account = await repository.GetAccount(request.AccountId);
                if (account == null)
                {
                    return ValidationResult.Fail("Account not found!");
                }

                var transactionAmount = new Money(request.Amount);
                var commissionAmount = commissionCalculator.Calculate(transactionAmount, request.Origin);

                if (account.GetCurrentBalance().Amount < transactionAmount.Add(commissionAmount).Amount)
                {
                    return ValidationResult.Fail("Balance is insufficient for this transaction");
                }
                
                return ValidationResult.Success();
            }
        }

        public class CreatePaymentTransactionHandler : IRequestHandler<CreatePaymentTransactionRequest, FinResponse<CreatePaymentTransactionResponse>>
        {
            private readonly IAccountRepository repository;
            private readonly IUnitOfWork unitOfWork;
            private readonly ICommissionCalculator commissionCalculator;
            public CreatePaymentTransactionHandler(IAccountRepository repository, IUnitOfWork unitOfWork, ICommissionCalculator commissionCalculator)
            {
                this.repository = repository;
                this.unitOfWork = unitOfWork;
                this.commissionCalculator = commissionCalculator;
            }
            public async Task<FinResponse<CreatePaymentTransactionResponse>> Handle(CreatePaymentTransactionRequest request, CancellationToken cancellationToken)
            {
                var account = await repository.GetAccount(request.AccountId);
                
                /**
                 * Create main debit transaction request and add to transaction of accounts
                 */
                var transactionAmount = new Money(request.Amount);
                var debit = new Debit(Guid.NewGuid(), request.AccountId, transactionAmount, TransactionType.PAYMENT, request.Origin, DateTime.Now);
                account.Debit(debit);
                await repository.Update((Account)account, debit);
                /**
                 * Create Commission transaction and add to debit records too
                 */
                var commissionAmount = commissionCalculator.Calculate(transactionAmount, request.Origin);
                if (commissionAmount.Amount > 0)
                {
                    var commission = new Debit(Guid.NewGuid(), request.AccountId, commissionAmount, TransactionType.COMMISSION, request.Origin, DateTime.Now, debit.Id);
                    await repository.Update((Account)account, commission);
                }
              
                /**
                 * Save context changes and return new transaction id
                */
                await unitOfWork.Save();
                return FinResponse<CreatePaymentTransactionResponse>.Success(new CreatePaymentTransactionResponse() { TransacitonId = debit.Id });
            }
        }
    }
}
