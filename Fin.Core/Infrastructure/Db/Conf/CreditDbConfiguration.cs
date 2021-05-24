using Fin.Core.Domain.Accounts;
using Fin.Core.Domain.Accounts.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Fin.Core.Infrastructure.Db.Conf
{
    public sealed class CreditConfiguration : IEntityTypeConfiguration<Credit>
    {
        /// <summary>
        ///     Configure Credit.
        /// </summary>
        /// <param name="builder">Builder.</param>
        public void Configure(EntityTypeBuilder<Credit> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Credit");
            builder.Property(credit => credit.Amount)
                .HasConversion(
                    value => value.Amount,
                    value => new Money(value))
                .IsRequired();

            builder.Property(credit => credit.Id)
                .IsRequired().ValueGeneratedNever();

            builder.Property(credit => credit.AccountId)
                .IsRequired();

            builder.Property(credit => credit.TransactionDate)
                .IsRequired();

            builder.Property(credit => credit.Origin)
                .HasConversion(
                    d => d.ToString(),
                    d => Enum.Parse<TransactionOrigin>(d)
                )
                .IsRequired();

            builder.Property(credit => credit.TransactionType)
                .HasConversion(
                    d => d.ToString(),
                    d => Enum.Parse<TransactionType>(d)
                )
                .IsRequired();

            builder.Property(credit => credit.ParentTransactionId);

            builder.Property(credit => credit.AccountId)
                .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction).ValueGeneratedNever();
        }
    }
}
