using Fin.Core.Domain.Accounts.ValueObjects;

namespace Fin.Core.Domain.Accounts
{
    public interface IAccount
    {
        long Id { get; }

        DebitCollection DebitTransactions { get; }

        CreditCollection CreditTransactions { get; }
        void Debit(Debit payment);

        void Credit(Credit credit);       

        Money GetCurrentBalance();
    }
}
