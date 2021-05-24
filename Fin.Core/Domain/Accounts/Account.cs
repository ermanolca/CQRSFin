using Fin.Core.Domain.Accounts.ValueObjects;

namespace Fin.Core.Domain.Accounts
{
    public class Account : IAccount
    {

        public Account(long id)
        {
            this.Id = id;
        }
 
        public DebitCollection DebitTransactions { get; } = new DebitCollection();

        public CreditCollection CreditTransactions { get; } = new CreditCollection();

        public long Id { get; }

        public void Debit(Debit payment) => this.DebitTransactions.Add(payment);

        public void Credit(Credit adjustment) => this.CreditTransactions.Add(adjustment);

        public Money GetCurrentBalance()
        {
            Money totalCredits = this.CreditTransactions
                .GetTotal();

            Money totalDebits = this.DebitTransactions
                .GetTotal();

            Money totalAmount = totalCredits
                .Subtract(totalDebits);

            return totalAmount;
        }
    }
}
