using Fin.Core.Domain.Accounts.ValueObjects;

namespace Fin.Core.Domain
{
    public interface ICommissionCalculator
    {
        Money Calculate(Money money, TransactionOrigin origin);
    }
}
