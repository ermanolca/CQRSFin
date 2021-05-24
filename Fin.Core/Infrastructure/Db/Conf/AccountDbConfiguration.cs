using Fin.Core.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Fin.Core.Infrastructure.Db.Conf
{
    public sealed class AccountDbConfiguration : IEntityTypeConfiguration<Account>
    {
        /// <summary>
        ///     Configure Account.
        /// </summary>
        /// <param name="builder">Builder.</param>
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Account");

            builder.Property(b => b.Id)
                .IsRequired();

            builder.HasMany(x => x.CreditTransactions)
                .WithOne(b => b.Account!)
                .HasForeignKey(b => b.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.DebitTransactions)
                .WithOne(b => b.Account!)
                .HasForeignKey(b => b.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
