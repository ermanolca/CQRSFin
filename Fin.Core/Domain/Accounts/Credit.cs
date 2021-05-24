using Fin.Core.Domain.Accounts.ValueObjects;
using System;

namespace Fin.Core.Domain.Accounts
{
    public class Credit : ITransaction
    {
        public Credit(Guid id, long accountId, Guid? parentTransactionId, Money amount, TransactionType transactionType, TransactionOrigin origin, DateTime transactionDate)
        {
            this.Id = id;
            this.Amount = amount;
            this.Origin = origin;
            this.ParentTransactionId = parentTransactionId;
            this.TransactionDate = transactionDate;
            this.TransactionType = transactionType;
            this.AccountId = accountId;
        }

        public Guid Id { get; private set; }
        public long AccountId { get; private set; }
        public Account? Account { get; set; }
        public TransactionType TransactionType { get; private set; }
        public TransactionOrigin Origin { get; private set; }
        public Money Amount { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public Guid? ParentTransactionId { get; private set; }
    }
}
