using Fin.Core.Domain.Accounts;
using Fin.Core.Domain.Accounts.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Fin.Core.Infrastructure.Db.Conf
{
    public sealed class DebitConfiguration : IEntityTypeConfiguration<Debit>
    {
        /// <summary>
        ///     Configure Credit.
        /// </summary>
        /// <param name="builder">Builder.</param>
        public void Configure(EntityTypeBuilder<Debit> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Debit");

            builder.Property(debit => debit.Amount)
                .HasConversion(
                    value => value.Amount,
                    value => new Money(value))
                .IsRequired();

            builder.Property(debit => debit.Id)
                .IsRequired().ValueGeneratedNever();

            builder.Property(debit => debit.AccountId)
                .IsRequired();

            builder.Property(debit => debit.TransactionDate)
                .IsRequired();

            builder.Property(debit => debit.Origin)
                .HasConversion(
                    d => d.ToString(),
                    d => Enum.Parse<TransactionOrigin>(d)
                )
                .IsRequired();

            builder.Property(debit => debit.TransactionType)
                .HasConversion(
                    d => d.ToString(),
                    d => Enum.Parse<TransactionType>(d)
                )
                .IsRequired();

            builder.Property(debit => debit.AccountId)
                .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction).ValueGeneratedNever();
        }
    }
}
