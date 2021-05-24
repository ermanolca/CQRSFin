using Fin.Core.Domain.Accounts;
using Fin.Core.Domain.Accounts.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fin.Core.Infrastructure.Db
{
    public class DataSeed
    {
        public static readonly long AccountId1 = 4755;
        public static readonly long AccountId2 = 9834;
        public static readonly long AccountId3 = 7735;

        public static readonly Guid CreditId1 = new Guid("c73c519c-8083-43e5-9fa0-37d5ab6b8ab6");
        public static readonly Guid CreditId2 = new Guid("e606e51a-2cfc-4d7e-8515-7b4bc00bba22");
        public static readonly Guid CreditId3 = new Guid("4cc0cb89-8a9e-45d1-a349-77e76dbf5a25");

        public static readonly Money balance1 = new Money(1001.88m);
        public static readonly Money balance2 = new Money(456.55m);
        public static readonly Money balance3 = new Money(89.36m);


        public static void Seed(ModelBuilder builder)
        {

            builder.Entity<Account>()
                .HasData(
                    new
                    {
                        Id = AccountId1,
                    });

            builder.Entity<Credit>()
                .HasData(
                    new
                    {
                        Id = CreditId1,
                        AccountId = AccountId1,
                        TransactionDate = DateTime.UtcNow,
                        Amount = balance1,
                        Origin = TransactionOrigin.CASH,
                        TransactionType = TransactionType.INITIAL,
                    });

            builder.Entity<Account>()
                .HasData(
                    new
                    {
                        Id = AccountId2,
                    });

            builder.Entity<Credit>()
                .HasData(
                    new
                    {
                        Id = CreditId2,
                        AccountId = AccountId2,
                        TransactionDate = DateTime.UtcNow,
                        Amount = balance2,
                        Origin = TransactionOrigin.CASH,
                        TransactionType = TransactionType.INITIAL,
                    });

            builder.Entity<Account>()
                .HasData(
                    new
                    {
                        Id = AccountId3,
                    });

            builder.Entity<Credit>()
                .HasData(
                    new
                    {
                        Id = CreditId3,
                        AccountId = AccountId3,
                        TransactionDate = DateTime.UtcNow,
                        Amount = balance3,
                        Origin = TransactionOrigin.CASH,
                        TransactionType = TransactionType.INITIAL,
                    });

        }


    }
}
