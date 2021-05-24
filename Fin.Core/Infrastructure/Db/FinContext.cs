using Fin.Core.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fin.Core.Infrastructure.Db
{
    public sealed class FinContext : DbContext
    {
        public FinContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Credit> Credits { get; set; }

        public DbSet<Debit> Debits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinContext).Assembly);
            DataSeed.Seed(modelBuilder);
        }
    }
}
