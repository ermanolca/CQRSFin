using Fin.Core.Domain.Accounts.ValueObjects;
using System;

namespace Fin.Core.Domain.Accounts
{
    public class Debit : ITransaction
    {
        public Debit(Guid id, long accountId, Money amount, TransactionType transactionType, TransactionOrigin origin, DateTime transactionDate, Guid? parentTransactionId = null)
        {
            this.Id = id;
            this.ParentTransactionId = parentTransactionId;
            this.Amount = amount;
            this.Origin = origin;
            this.TransactionDate = transactionDate;
            this.TransactionType = transactionType;
            this.AccountId = accountId;
        }

        public Guid Id { get; private set; }
        public long AccountId { get; private set; }
        public Account Account { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public TransactionOrigin Origin { get; private set; }
        public Money Amount { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public Guid? ParentTransactionId { get; private set; }

    }
}
