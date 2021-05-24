using Fin.Core.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fin.Core.Infrastructure.Db.Repository
{
    public sealed class AccountRepository : IAccountRepository
    {
        private readonly FinContext _context;

        public AccountRepository(FinContext context) => this._context = context ??
                                                                          throw new ArgumentNullException(
                                                                              nameof(context));

        public Task<IList<Account>> GetAccounts()
        {
            IList<Account> accounts = this._context
                .Accounts
                .ToList();

            return Task.FromResult(accounts);
        }

        public async Task<IAccount> GetAccount(long accountId)
        {
            Account account = await this._context
                .Accounts
                .Where(e => e.Id == accountId)
                .Select(e => e)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);

            if (account is Account accountFound)
            {
                await this.LoadTransactions(accountFound)
                    .ConfigureAwait(false);

                return account;
            }

            return null;
        }

        /// <inheritdoc />
        public async Task Update(IAccount account, Credit credit) => await this._context
            .Credits
            .AddAsync(credit)
            .ConfigureAwait(false);

        /// <inheritdoc />
        public async Task Update(IAccount account, Debit debit) => await this._context
            .Debits
            .AddAsync(debit)
            .ConfigureAwait(false);

        private async Task LoadTransactions(Account account)
        {
            await this._context
                .Credits
                .Where(e => e.AccountId.Equals(account.Id))
                .ToListAsync()
                .ConfigureAwait(false);

            await this._context
                .Debits
                .Where(e => e.AccountId.Equals(account.Id))
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }

}
