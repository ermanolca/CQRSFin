using Fin.Core.Domain.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fin.Core.Infrastructure.Db.Repository
{
    public interface IAccountRepository
    {
        Task<IList<Account>> GetAccounts();
        Task<IAccount> GetAccount(long accountId);

        Task Update(IAccount account, Credit credit);

        Task Update(IAccount account, Debit debit);


    }
}