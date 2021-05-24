namespace Fin.Core.Domain.Accounts.ValueObjects
{
    using System;
    public readonly struct Money : IEquatable<Money>
    {
        public decimal Amount { get; }

        public Money(decimal amount) =>
            this.Amount = amount;

        public override bool Equals(object? obj) =>
            obj is Money o && this.Equals(o);

        public bool Equals(Money other) =>
            this.Amount == other.Amount;

        public override int GetHashCode() =>
            HashCode.Combine(this.Amount);

        public static bool operator ==(Money left, Money right) => left.Equals(right);

        public static bool operator !=(Money left, Money right) => !(left == right);

        public static Money operator *(Money left, decimal right) => new Money(left.Amount * right);

        public bool IsZero() => this.Amount == 0;

        public Money Subtract(Money debit) =>
            new Money(Math.Round(this.Amount - debit.Amount, 2));

        public Money Add(Money amount) => new Money(Math.Round(this.Amount + amount.Amount, 2));

        public Money Rate(decimal percentage) => new Money(Math.Round(this.Amount * percentage / 100, 2));

        public override string ToString() => string.Format($"{this.Amount}");
    }
}
