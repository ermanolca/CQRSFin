using Fin.Core.Domain.Accounts.ValueObjects;
using System;
using System.Runtime.Serialization;

namespace Fin.Core.Domain.Accounts
{
    public interface ITransaction
    {
        public Guid Id { get; }
        public long AccountId { get; }

        public Account Account { get; }

        public Guid? ParentTransactionId { get; }

        public Money Amount { get; }

        public TransactionType TransactionType { get; }

        public TransactionOrigin Origin { get; }

        public DateTime TransactionDate { get; }
    }
}

public enum TransactionType
{
    [EnumMember(Value = "INITIAL")]
    INITIAL,
    [EnumMember(Value = "PAYMENT")]
    PAYMENT,
    [EnumMember(Value = "ADJUSTMENT")]
    ADJUSTMENT,
    [EnumMember(Value = "COMMISSION")]
    COMMISSION,
    [EnumMember(Value = "COMMISSION_ADJUSTMENT")]
    COMMISSION_ADJUSTMENT,

}

public enum TransactionOrigin
{
    [EnumMember(Value = "CASH")]
    CASH,
    [EnumMember(Value = "VISA")]
    VISA,
    [EnumMember(Value = "MASTERCARD")]
    MASTERCARD,
}