using Fin.Core.Domain.Accounts.ValueObjects;

namespace Fin.Core.Domain
{
    public class CommissionCalculator : ICommissionCalculator
    {
        public Money Calculate(Money money, TransactionOrigin origin)
        {
            if(origin == TransactionOrigin.VISA)
            {
                return money.Rate(1);
            }
            else if (origin == TransactionOrigin.MASTERCARD)
            {
                return money.Rate(2);
            }

            return new Money(0);
        }
    }
}
