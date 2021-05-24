using Fin.Core.Infrastructure.Db.Repository;
using Fin.Core.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Fin.Core.Handlers.Query
{
    public static class GetAccountBalance
    {

        public class GetAccountBalanceRequest : IRequest<FinResponse<AccountBalanceResponse>>
        {
            public GetAccountBalanceRequest(long id)
            {
                this.AccountId = id;
            }
            public long AccountId { get; }
        }

        public class AccountBalanceResponse
        {
            public AccountBalanceResponse()
            {

            }

            public decimal Balance { get; set; }
        }

        public class GetAccountBalanceValidationHandler : IValidationHandler<GetAccountBalanceRequest>
        {
            private readonly IAccountRepository repository;
            public GetAccountBalanceValidationHandler(IAccountRepository repository)
            {
                this.repository = repository;
            }

            public async Task<ValidationResult> Validate(GetAccountBalanceRequest request)
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

                return ValidationResult.Success();
            }
        }

        public class GetAccountBalanceHandler : IRequestHandler<GetAccountBalanceRequest, FinResponse<AccountBalanceResponse>>
        {
            private readonly IAccountRepository repository;
            public GetAccountBalanceHandler(IAccountRepository repository)
            {
                this.repository = repository;
            }
            public async Task<FinResponse<AccountBalanceResponse>> Handle(GetAccountBalanceRequest request, CancellationToken cancellationToken)
            {
                var account = await repository.GetAccount(request.AccountId);
                return FinResponse.Success<AccountBalanceResponse>(new AccountBalanceResponse(){ Balance = account.GetCurrentBalance().Amount });
            }
    }
}
}
