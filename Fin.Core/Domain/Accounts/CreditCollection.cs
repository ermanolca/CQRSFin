using Fin.Core.Domain.Accounts.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Fin.Core.Domain.Accounts
{
    public sealed class CreditCollection : List<Credit>
    {
        public Money GetTotal()
        {
            if (this.Count == 0)
            {
                return new Money(0);
            }

            Money total = new Money(0);

            return this.Aggregate(total, (current, credit) =>
                new Money(current.Amount + credit.Amount.Amount));
        }
    }
}
